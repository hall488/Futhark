using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Futhark {

    public class MainPanel : Panel{ 

        Camera_LE camera;

        Item_LE activeItem;
        Texture2D grid;

        Point minSelect;
        Point maxSelect;

        bool selectPass;

        List<Item_LE> placedItems;

        Layer_Manager_LE manager;

        

        public MainPanel(RenderTarget2D renderTarget, Rectangle rect, ContentManager content, Camera_LE camera) : base(renderTarget, rect, content) {   
            this.camera = camera;

            activeItem = null;
            selectPass = false;

            minSelect = new Point();
            maxSelect = new Point();

            grid = content.Load<Texture2D>("grid_pattern");

            manager = new Layer_Manager_LE();

        }

        public override void Update(MouseState mouseState) {
            base.Update(mouseState);

            var vector = mousePos.ToVector2();
                var transVector = Vector2.Transform(vector, Matrix.Invert(camera.Transform));
                mousePos = transVector.ToPoint();

            var gridMousePos = new Point();
            gridMousePos.X = (int)Math.Round((mousePos.X - grid.Width * 4) / 8d / grid.Width, 0) * 8 * grid.Width;
            gridMousePos.Y = (int)Math.Round((mousePos.Y - grid.Height * 4)  / 8d / grid.Height, 0) * 8 * grid.Height;

            
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
                            placedItems.Add(new Item_LE("xx", activeItem.itemTexture, null, 0, 0, 1));
                            placedItems.Last().placeItem(new Rectangle(i, j, activeItem.itemTexture.Width*8, activeItem.itemTexture.Height*8));
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

            foreach(var val in placedItems) {
                spriteBatch.Draw(val.itemTexture, val.itemRect, Color.White);
            }

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
    
    
