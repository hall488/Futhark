using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using MES = MonoGame.Extended.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace Futhark {

    public class LevelEditor { 

        private ContentManager Content;

        private SpriteBatch spriteBatch;

        private GraphicsDeviceManager graphics;

        private GraphicsDevice graphicsDevice;

        Dictionary<string, (Texture2D, Rectangle)> overlay;
        List<Item> items;

        RenderTarget2D sidePanel;
        Rectangle sideRect;
        RenderTarget2D mainPanel;
        Rectangle mainRect;

        Point sideMousePosition;
        Point mainMousePosition;
        Item activeItem;

        RenderTarget2D activePanel;

        List<(Texture2D, Rectangle)> placedItems;

        Camera_LE camera;

        Texture2D grid;


        public LevelEditor(ContentManager Content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
            this.Content = Content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;

            activeItem = null;

        }

        public void LoadContent() {

            camera = new Camera_LE(graphicsDevice.Viewport);

            overlay = new Dictionary<string, (Texture2D, Rectangle)>();
            items = new List<Item>();
            placedItems = new List<(Texture2D, Rectangle)>();
            string jsonFile = File.ReadAllText("text_assets/Level_Editor_Assets.json");
            Dictionary<string, string[]> tempDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonFile);   

            grid = Content.Load<Texture2D>("grid_pattern");      

            foreach((var key, var val) in tempDict) {                
                if(key == "border") {
                    Texture2D texture = Content.Load<Texture2D>(val[0]);
                    overlay.Add(key, (texture, Rectangle.Empty));
                } else if(key == "container") {
                    Texture2D texture = Content.Load<Texture2D>(val[0]);
                    overlay.Add(key, (texture, new Rectangle(0, 
                                                            0, 
                                                            Futhark_Game.screenHeight * texture.Width / texture.Height, 
                                                            Futhark_Game.screenHeight)));
                    
                } else if(key == "objects") {
                    foreach(var i in val) {
                        Texture2D texture = Content.Load<Texture2D>(i);
                        items.Add(new Item(key, texture, overlay["border"].Item1));
                    }
                } else if(key == "tiles") {
                    foreach(var i in val) {
                        Texture2D texture = Content.Load<Texture2D>(i);
                        items.Add(new Item(key, texture, overlay["border"].Item1));
                    }
                }
            }

            sidePanel = new RenderTarget2D(this.graphicsDevice,
                                                overlay["container"].Item2.Width,
                                                graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            mainPanel = new RenderTarget2D(this.graphicsDevice,
                                                this.graphicsDevice.PresentationParameters.BackBufferWidth - overlay["container"].Item2.Width,
                                                graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            sideRect = new Rectangle(Futhark_Game.screenWidth - sidePanel.Width, 0, sidePanel.Width, sidePanel.Height);
            mainRect = new Rectangle(0, 0, Futhark_Game.screenWidth - sideRect.Width, Futhark_Game.screenHeight);
           
        }

        public int Update() {
            camera.UpdateCamera(graphicsDevice.Viewport);

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            InputUtil.GetButtonState();

            
            if(keyboardState.IsKeyDown(Keys.M))
                return (int) Futhark_Game.gameStates.mainMenu;

            if(sideRect.Contains(mouseState.Position)) {
                activePanel = sidePanel;
                sideMousePosition.X = mouseState.Position.X - sideRect.X;
                sideMousePosition.Y = mouseState.Position.Y - sideRect.Y;
            } else {
                sideMousePosition = Point.Zero;
            }

            
            

            if(mainRect.Contains(mouseState.Position)) {
                activePanel = mainPanel;
                mainMousePosition.X = mouseState.Position.X;
                mainMousePosition.Y = mouseState.Position.Y;

                //wtf is this magic, math is cool
                var vector = mainMousePosition.ToVector2();
                var transVector = Vector2.Transform(vector, Matrix.Invert(camera.Transform));
                mainMousePosition = transVector.ToPoint();

            } else {
                mainMousePosition = Point.Zero;
            }


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

           

            foreach(var val in items) {
                if(val.borderRect.Contains(sideMousePosition)) {
                    if(InputUtil.SingleMouseClick()) {
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

            var gridMousePos = new Point();
            gridMousePos.X = (int)Math.Round(mainMousePosition.X / 8d / grid.Width, 0) * 8 * grid.Width;
            gridMousePos.Y = (int)Math.Round(mainMousePosition.Y  / 8d / grid.Height, 0) * 8 * grid.Height;


            if(activePanel == mainPanel && activeItem != null && InputUtil.SingleMouseClick()) {
                placedItems.Add(new (activeItem.itemTexture, new Rectangle(gridMousePos.X, gridMousePos.Y, activeItem.itemTexture.Width*8, activeItem.itemTexture.Height*8)));
                Console.WriteLine(placedItems.ToString());
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

            spriteBatch.Draw(overlay["container"].Item1, overlay["container"].Item2, Color.White);

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
            gridMousePos.X = (int)Math.Round(mainMousePosition.X / 8d / grid.Width, 0) * 8 * grid.Width;
            gridMousePos.Y = (int)Math.Round(mainMousePosition.Y  / 8d / grid.Height, 0) * 8 * grid.Height;

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
    }
}