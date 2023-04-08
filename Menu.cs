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

namespace Futhark {

    public class Menu { 

        private ContentManager Content;

        private SpriteBatch spriteBatch;

        private GraphicsDeviceManager graphics;

        private GraphicsDevice graphicsDevice;

        public Dictionary<string, (Texture2D, Rectangle)> buttons; 

        Texture2D titleImage;


        public Menu(ContentManager Content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
            this.Content = Content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;
        }

        public void LoadContent() {
            
            buttons = new Dictionary<string, (Texture2D, Rectangle)>();
            string jsonFile = File.ReadAllText("text_assets/mainMenu.json");
            Dictionary<string, string[]> tempDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonFile);

            foreach((var key, var val) in tempDict) {
                if(key == "title")
                    titleImage = Content.Load<Texture2D>(val[0]);
                if(key == "buttons") {
                    int hIncrement = 0;
                    foreach(var i in val) {
                        Texture2D texture = Content.Load<Texture2D>(i);
                        buttons.Add(i, (texture, new Rectangle(Futhark_Game.screenWidth / 2, Futhark_Game.screenHeight / 2 + hIncrement, texture.Width*4, texture.Height*4)));
                        hIncrement += texture.Height*6;
                    }
                }
            }
        }

        public int Update() {
            var mouseState = Mouse.GetState();

            if(buttons["mainMenu_SG_Button"].Item2.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) {
                return 1;
            }
            else {
                return 0;
            }
        }

        public void Draw() {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spriteBatch.Draw(titleImage, new Rectangle(Futhark_Game.screenWidth / 2, Futhark_Game.screenHeight / 2 - (int)((double)titleImage.Height*5), titleImage.Width*4, titleImage.Height*4), 
                                                        null, Color.White, 0, new Vector2(titleImage.Width / 2, titleImage.Height / 2), SpriteEffects.None, 0f);
            
            foreach((var key, var val) in buttons) {
                spriteBatch.Draw (val.Item1, val.Item2, null, Color.White, 0, new Vector2(val.Item1.Width / 2, val.Item1.Height / 2), SpriteEffects.None, 0f);
            }
            spriteBatch.End();
        }
    }
}