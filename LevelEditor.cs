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

        Point sideMousePosition;
        Point mainMousePosition;
        Item_LE activeItem;
        Panel activePanel;

        List<(Texture2D, Rectangle, string)> placedItems;

        Camera_LE camera;

        

        Container_LE container;

        MainPanel mainPanel;

        SidePanel sidePanel;


        public LevelEditor(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
            this.content = content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;

            

        }

        public void LoadContent() {

            camera = new Camera_LE(graphicsDevice.Viewport);

            


            

            Texture2D containerTexture = content.Load<Texture2D>("container_LE");

            int ratio = graphicsDevice.PresentationParameters.BackBufferHeight / containerTexture.Height;

            
            

            RenderTarget2D sideRT = new RenderTarget2D(this.graphicsDevice,
                                                55 * ratio,
                                                graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            RenderTarget2D mainRT = new RenderTarget2D(this.graphicsDevice,
                                                this.graphicsDevice.PresentationParameters.BackBufferWidth - 55 * ratio,
                                                graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            Rectangle sideRect = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - sideRT.Width, 0, sideRT.Width, sideRT.Height);
            Rectangle mainRect = new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth - sideRect.Width, graphicsDevice.PresentationParameters.BackBufferHeight);

            
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
            

            if(sidePanel.rect.Contains(mouseState.Position)) {
                sidePanel.Update(mouseState);
            }

            if(mainPanel.rect.Contains(mouseState.Position)) {
                mainPanel.SetActiveItem(sidePanel.getActiveItem());             
                mainPanel.Update(mouseState);                
            }

            if(sidePanel.container.saveButton.Update(sidePanel.mousePos)) {
                mainPanel.manager.SaveHexMaps();
            }

            


            return (int) Futhark_Game.gameStates.levelEditor;
        }

        

        

        public void Draw() {
            
            sidePanel.DrawSidePanel(spriteBatch, graphicsDevice);
            mainPanel.DrawMainPanel(spriteBatch, graphicsDevice);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            
            spriteBatch.Draw(sidePanel.renderTarget, sidePanel.rect, Color.White);

            spriteBatch.Draw(mainPanel.renderTarget, mainPanel.rect, Color.White);
            

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