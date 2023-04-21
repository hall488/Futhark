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

    public class SidePanel : Panel{ 

        Container_LE container;

        public SidePanel(RenderTarget2D renderTarget, Rectangle rect, ContentManager content) : base(renderTarget, rect, content) {

            string jsonFile = File.ReadAllText("text_assets/assets_LE.json");
            Dictionary<string, Texture2D> assetsDict = new Dictionary<string, Texture2D>();
            foreach((var key, var val) in JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonFile)) {
                foreach(var i in val)
                    assetsDict.Add(i, content.Load<Texture2D>(i));           
            }

            jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");
            Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
            foreach((var key, var val) in JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile)) {
                textureDict.Add(key, content.Load<Texture2D>(val));           
            }




            container = new Container_LE(assetsDict, textureDict);
        }

        public override void Update(MouseState mouseState) {
            base.Update(mouseState);
            container.Update(mousePos);
            
        }

        public void DrawSidePanel(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() {DepthBufferEnable = true};

            graphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);  
            
            Draw(spriteBatch);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        public override void Draw (SpriteBatch spriteBatch) {
            container.Draw(spriteBatch);
        }

        public Item_LE getActiveItem() {
            return container.activeItem;
        }

        
    }
    
    
}