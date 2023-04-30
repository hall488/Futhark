using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Futhark {

    public class MainPanel : Panel{ 

        Camera_LE camera;

        Item_LE activeItem;
        Texture2D grid;

        Point minSelect;
        Point maxSelect;

        bool selectPass;

        List<Item_LE> placedItems = new List<Item_LE>();

        Layer_Manager_LE manager;

        Dictionary<string, (Point[], Point[], Texture2D)> structureParams; 

        Dictionary<string, Texture2D> textureDict;

        public MainPanel(RenderTarget2D renderTarget, Rectangle rect, ContentManager content, Camera_LE camera) : base(renderTarget, rect, content) {   
            this.camera = camera;

            activeItem = null;
            selectPass = false;

            minSelect = new Point();
            maxSelect = new Point();

            grid = content.Load<Texture2D>("grid_pattern");

            string jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");
            textureDict = new Dictionary<string, Texture2D>();
            foreach((var key, var val) in JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile)) {
                textureDict.Add(key, content.Load<Texture2D>(val));           
            }

            manager = new Layer_Manager_LE(16, 16, textureDict);

            structureParams = new Dictionary<string, (Point[], Point[], Texture2D)>();

            var assetFolders = Directory.GetDirectories("assets/Structures");
            
            foreach(var f in assetFolders) {
                Console.WriteLine(f);

                Texture2D texture = content.Load<Texture2D>(f + "/image");
                jsonFile = File.ReadAllText("text_" + f + "/layers.json");
                 
                Dictionary<string, Point[]> layerDict = JsonConvert.DeserializeObject<Dictionary<string, Point[]>>(jsonFile);
                
                structureParams.Add(f.Split("\\")[1], (layerDict["onGround"], layerDict["overGround"], texture));
            }

        }


        public override void Update(MouseState mouseState) {
            base.Update(mouseState);

            var vector = mousePos.ToVector2();
                var transVector = Vector2.Transform(vector, Matrix.Invert(camera.Transform));
                mousePos = transVector.ToPoint();

            var gridMousePos = new Point();
            gridMousePos.X = (int)Math.Round((mousePos.X - grid.Width * 4) / 8d / grid.Width, 0);
            gridMousePos.Y = (int)Math.Round((mousePos.Y - grid.Height * 4)  / 8d / grid.Height, 0);

            
            if(activeItem != null) {
                if(InputUtil.SingleLeftClick()) {
                    minSelect = gridMousePos;
                    selectPass = true;
                }

                if(selectPass && mouseState.LeftButton == ButtonState.Released) {
                    maxSelect = gridMousePos;
                    selectPass = false;

                    for(int i = minSelect.X; i <= maxSelect.X; i++) {
                        for(int j = minSelect.Y; j <= maxSelect.Y; j++) {
                            //placedItems.Add(new Item_LE("xx", activeItem.itemTexture, null, 0, 0, 1));
                            //placedItems.Last().placeItem(new Rectangle(i, j, activeItem.itemTexture.Width*8, activeItem.itemTexture.Height*8));

                            if(activeItem.category == "structure") {
                                Console.WriteLine(activeItem.fileName);
                                (var onGroundPoints, var overGroundPoints, var texture) = structureParams[activeItem.fileName];
                                manager.AddStructure(new Structure(onGroundPoints, overGroundPoints, texture, new Point(i, j)));
                            } else if (activeItem.category == "tile") {
                                manager.AddTile(new Tile(activeItem.hexcode, new Point(i, j)));
                            }
                        }
                    }
                    
                }
            } else {
                var ToBeRemoved = new List<Item_LE>();

                if(InputUtil.SingleRightClick()) {
                    foreach(var val in placedItems) {
                        if(val.itemRect.Contains(gridMousePos)) {
                            ToBeRemoved.Add(val);
                        }
                    }
                }
                ToBeRemoved.Reverse();
                foreach(var i in ToBeRemoved) {
                    placedItems.Remove(i);
                    break;
                }

                ToBeRemoved.Clear();
            }
                
        }

        public void DrawMainPanel(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() {DepthBufferEnable = true};

            graphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, transformMatrix: camera.Transform);
            
            
            Draw(spriteBatch);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        public override void Draw (SpriteBatch spriteBatch) {
            for(int i = 0; i < 16; i++) {
                for(int j =0; j < 16; j++) {
                    var rect = new Rectangle(i*grid.Width*8, j*grid.Height*8, grid.Width*8, grid.Height*8);
                    spriteBatch.Draw(grid, rect, Color.White);
                }
            }

            manager.Draw(spriteBatch);

            var gridMousePos = new Point();
            gridMousePos.X = (int)Math.Round((mousePos.X - grid.Width * 4) / 8d / grid.Width, 0) * 8 * grid.Width;
            gridMousePos.Y = (int)Math.Round((mousePos.Y - grid.Height * 4)  / 8d / grid.Height, 0) * 8 * grid.Height;

            if(activeItem != null) {
                spriteBatch.Draw(activeItem.itemTexture, new Rectangle(gridMousePos.X, gridMousePos.Y, activeItem.itemTexture.Width*8, activeItem.itemTexture.Height*8), Color.White);
            }
        }

        public void SetActiveItem(Item_LE activeItem) {
            this.activeItem = activeItem;
        }

    }
}
    
    
