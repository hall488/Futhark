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
        Dictionary<string, (Texture2D, string)> items;


        public LevelEditor(ContentManager Content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
            this.Content = Content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;
        }

        public void LoadContent() {
            overlay = new Dictionary<string, (Texture2D, Rectangle)>();
            items = new Dictionary<string, (Texture2D, string)>();
            string jsonFile = File.ReadAllText("text_assets/Level_Editor_Assets.json");
            Dictionary<string, string[]> tempDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonFile);            

            foreach((var key, var val) in tempDict) {                
                if(key == "border") {
                    Texture2D texture = Content.Load<Texture2D>(val[0]);
                    overlay.Add(key, (texture, Rectangle.Empty));
                } else if(key == "container") {
                    Texture2D texture = Content.Load<Texture2D>(val[0]);
                    overlay.Add(key, (texture, new Rectangle(Futhark_Game.screenWidth - Futhark_Game.screenHeight * texture.Width / texture.Height, 
                                                            0, 
                                                            Futhark_Game.screenHeight * texture.Width / texture.Height, 
                                                            Futhark_Game.screenHeight)));
                    
                } else if(key == "objects") {
                    foreach(var i in val) {
                        Texture2D texture = Content.Load<Texture2D>(i);
                        items.Add(i, (texture, key));
                    }
                } else if(key == "tiles") {
                    foreach(var i in val) {
                        Texture2D texture = Content.Load<Texture2D>(i);
                        items.Add(i, (texture, key));
                    }
                }
            }
           
        }

        public int Update() {
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.M))
                return (int) Futhark_Game.gameStates.mainMenu;

            return (int) Futhark_Game.gameStates.levelEditor;
        }

        public void Draw() {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach((var key, var val) in overlay) {
                spriteBatch.Draw (val.Item1, val.Item2, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
            }

            int itemCount = 0; 

            
            
            foreach((var key, var val) in items) {
                var x = itemCount % 2 == 0 ? 0 : 1;
                var y = itemCount / 2;
                var bTexture = overlay["border"].Item1;
                var context = overlay["container"].Item1;

                Rectangle bRectangle = new Rectangle(Futhark_Game.screenWidth - Futhark_Game.screenHeight * context.Width / context.Height + overlay["container"].Item2.Width / context.Width * (4+(2+bTexture.Width)*x),
                                                                                overlay["container"].Item2.Height / context.Height * (11+(2+bTexture.Height)*y),
                                                                                overlay["container"].Item2.Width / context.Width * bTexture.Width,
                                                                                overlay["container"].Item2.Height / context.Height * bTexture.Height);
                spriteBatch.Draw(bTexture, bRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);

                var iTexture = val.Item1;
                double scale = iTexture.Width < iTexture.Height ? (double)iTexture.Height : (double)iTexture.Width;
                Rectangle iRectangle = new Rectangle(   bRectangle.X + 2*bRectangle.Width / bTexture.Width + (int)(bRectangle.Width / bTexture.Width * 16) / 2,
                                                        bRectangle.Y + 2*bRectangle.Height / bTexture.Height + (int)(bRectangle.Height / bTexture.Height * 16) / 2,
                                                        (int)(bRectangle.Width / bTexture.Width *  iTexture.Width / scale * 16),
                                                        (int)(bRectangle.Height / bTexture.Height * iTexture.Height / scale * 16));
                spriteBatch.Draw(iTexture, iRectangle, null, Color.White, 0, new Vector2(iTexture.Width / 2, iTexture.Height / 2), SpriteEffects.None, 0f);
                Console.WriteLine("{0}, {1}, {2}", key, iRectangle.Width, iRectangle.Height);
                itemCount += 1;
            }


            spriteBatch.End();
        }
    }
}