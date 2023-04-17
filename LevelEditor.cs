using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;
using SI = SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Futhark {

    public class LevelEditor { 

        private ContentManager content;
        
        

        private SpriteBatch spriteBatch;

        private GraphicsDeviceManager graphics;

        private GraphicsDevice graphicsDevice;

        List<Overlay_Object> overlayObjects;
        List<LE_Item> items;


        Point sideMousePosition;
        Point mainMousePosition;
        LE_Item activeItem;

        RenderTarget2D activePanel;

        List<(Texture2D, Rectangle, string)> placedItems;

        Camera_LE camera;

        Texture2D grid;

        Point minSelect;
        Point maxSelect;

        bool selectPass;

        Container_LE container;

        MainPanel mainPanel;

        SidePanel sidePanel;


        public LevelEditor(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
            this.content = content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;

            activeItem = null;
            selectPass = false;

            minSelect = new Point();
            maxSelect = new Point();

        }

        public void LoadContent() {

            camera = new Camera_LE(graphicsDevice.Viewport);

            overlayObjects = new List<Overlay_Object>();
            items = new List<LE_Item>();
            placedItems = new List<(Texture2D, Rectangle, string)>();
            


            grid = content.Load<Texture2D>("grid_pattern");

            

            
            

            RenderTarget2D sideRT = new RenderTarget2D(this.graphicsDevice,
                                                55 * 8,
                                                graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            RenderTarget2D mainRT = new RenderTarget2D(this.graphicsDevice,
                                                this.graphicsDevice.PresentationParameters.BackBufferWidth - 55 * 8,
                                                graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            Rectangle sideRect = new Rectangle(Futhark_Game.screenWidth - sideRT.Width, 0, sideRT.Width, sideRT.Height);
            Rectangle mainRect = new Rectangle(0, 0, Futhark_Game.screenWidth - sideRect.Width, Futhark_Game.screenHeight);

            
            sidePanel = new SidePanel(sideRT, sideRect, content);
            mainPanel = new MainPanel(mainRT, mainRect, content, camera);

            
           
        }

        public int Update() {
            camera.UpdateCamera(graphicsDevice.Viewport);

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            InputUtil.GetButtonState();

            
            if(keyboardState.IsKeyDown(Keys.M))
                return (int) Futhark_Game.gameStates.mainMenu;

            

            
            

            

                //wtf is this magic, math is cool
                

            


            int itemCount = 0; 

            
            
            foreach(var val in items) {
                var x = itemCount % 2 == 0 ? 0 : 1;
                var y = itemCount / 2;
                var bTexture = overlay["border"].Item1;
                var cTexture = overlay["container"].Item1;
                var cRect = overlay["container"].Item2;

                val.setRectangles(x, y, cTexture, cRect);

                //Console.WriteLine("{0}, {1}, {2}", key, iRectangle.Width, iRectangle.Height);
                itemCount += 1;
            }

            if(activePanel == sidePanel) {
                foreach(var val in items) {
                    if(val.borderRect.Contains(sideMousePosition)) {
                        if(InputUtil.SingleLeftClick()) {
                            if(val.highlight == Color.White) {
                                items.ForEach(i => {i.highlight = Color.White;});
                                val.highlight = Color.Green;
                                activeItem = val;
                            }
                            else {
                                val.highlight = Color.White;
                                activeItem = null;
                            }
                        }
                    }
                }

                if(overlay["save_button"].Item2.Contains(sideMousePosition) && InputUtil.SingleLeftClick()) {
                    SaveLevelToBMP(placedItems);
                }
            }

           

            

            var gridMousePos = new Point();
            gridMousePos.X = (int)Math.Round((mainMousePosition.X - grid.Width * 4) / 8d / grid.Width, 0) * 8 * grid.Width;
            gridMousePos.Y = (int)Math.Round((mainMousePosition.Y - grid.Height * 4)  / 8d / grid.Height, 0) * 8 * grid.Height;


            if(activePanel == mainPanel) {
                if(activeItem != null) {
                    if(InputUtil.SingleLeftClick()) {
                        minSelect = gridMousePos;
                        selectPass = true;
                    }

                    if(selectPass && mouseState.LeftButton == ButtonState.Released) {
                        maxSelect = gridMousePos;
                        selectPass = false;

                        for(int i = minSelect.X; i <= maxSelect.X; i = i + 8*grid.Width) {
                            for(int j = minSelect.Y; j <= maxSelect.Y; j = j + 8*grid.Height) {
                                placedItems.Add(new (activeItem.itemTexture, new Rectangle(i, j, activeItem.itemTexture.Width*8, activeItem.itemTexture.Height*8), activeItem.identifier));
                            }
                        }
                        
                    }
                } else {
                    var ToBeRemoved = new List<(Texture2D, Rectangle, string)>();

                    if(InputUtil.SingleRightClick()) {
                        foreach(var val in placedItems) {
                            if(val.Item2.Contains(gridMousePos)) {
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

            


            return (int) Futhark_Game.gameStates.levelEditor;
        }

        public void DrawSidePanel(RenderTarget2D renderTarget) {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() {DepthBufferEnable = true};

            graphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);  
            
            foreach(var val in items) {
                val.Draw(spriteBatch);
            }

            foreach((var key, var val) in overlay){
                spriteBatch.Draw(val.Item1, val.Item2, Color.White);
            }

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

        }

        public void DrawMainPanel(RenderTarget2D renderTarget) {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() {DepthBufferEnable = true};

            graphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, transformMatrix: camera.Transform);
            
            for(int i = 0; i < 16; i++) {
                for(int j =0; j < 16; j++) {
                    var rect = new Rectangle(i*grid.Width*8, j*grid.Height*8, grid.Width*8, grid.Height*8);
                    spriteBatch.Draw(grid, rect, Color.White);
                }
            }

            foreach(var val in placedItems) {
                spriteBatch.Draw(val.Item1, val.Item2, Color.White);
            }

            var gridMousePos = new Point();
            gridMousePos.X = (int)Math.Round((mainMousePosition.X - grid.Width * 4) / 8d / grid.Width, 0) * 8 * grid.Width;
            gridMousePos.Y = (int)Math.Round((mainMousePosition.Y - grid.Height * 4)  / 8d / grid.Height, 0) * 8 * grid.Height;

            if(activeItem != null && activePanel == mainPanel) {
                spriteBatch.Draw(activeItem.itemTexture, new Rectangle(gridMousePos.X, gridMousePos.Y, activeItem.itemTexture.Width*8, activeItem.itemTexture.Height*8), Color.White);
            }
            

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        public void Draw() {
            
            DrawSidePanel(sidePanel);
            DrawMainPanel(mainPanel);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            
            spriteBatch.Draw(sidePanel, sideRect, Color.White);

            spriteBatch.Draw(mainPanel, mainRect, Color.White);
            

            spriteBatch.End();
        }

        private void SaveLevelToBMP(List<(Texture2D, Rectangle, string)> level) {

            //items.ForEach(i => {i.highlight = Color.White;});

            string jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");

            Dictionary<string, string> tileDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
            
            
            using (SI.Image<Rgba32> image = new SI.Image<Rgba32>(16,16)) {
                foreach(var val in level) {
                    int i = val.Item2.X / 8 / val.Item1.Width;
                    int j = val.Item2.Y / 8 / val.Item1.Height;

                    Console.WriteLine("{0}, {0}", i, j);

                    var refs = tileDict.FirstOrDefault(x => x.Value == val.Item3).Key;
                    var color = ColorHelper.FromHex(refs);

                    image[i,j] = new Rgba32(color.R, color.G, color.B, color.A);
                }

                SI.ImageExtensions.SaveAsBmp(image, "testFile.Bmp");
            }
        }
    }
}