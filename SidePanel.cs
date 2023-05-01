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

        public Container_LE container;

        private Item_LE activeItem;

        

        public SidePanel(RenderTarget2D renderTarget, Rectangle rect, ContentManager content) : base(renderTarget, rect, content) {

            string jsonFile = File.ReadAllText("text_assets/assets_LE.json");
            Dictionary<string, Texture2D> assetsDict = new Dictionary<string, Texture2D>();
            foreach((var key, var val) in JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonFile)) {
                foreach(var i in val)
                    assetsDict.Add(i, content.Load<Texture2D>(i));           
            }

            jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");
            Dictionary<string, string> tileDict = new Dictionary<string, string>();
            tileDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);

            Dictionary<string, Texture2D> tileTextureDict = new Dictionary<string, Texture2D>();

            foreach((var key, var val) in tileDict) {
                tileTextureDict.Add(key, content.Load<Texture2D>(val));           
            }
            
             var assetFolders = Directory.GetDirectories("assets/Structures");
            
            
            jsonFile = File.ReadAllText("text_assets/structures_dictionary.json");
            Dictionary<string, string> structureDict = new Dictionary<string, string>();
            structureDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
            Dictionary<string, Texture2D> structureTextureDict = new Dictionary<string, Texture2D>();
            foreach((var key, var val) in structureDict) {
                structureTextureDict.Add(key, content.Load<Texture2D>("assets/Structures/" + val + "/image"));           
            }




            container = new Container_LE(assetsDict, tileTextureDict, structureTextureDict, tileDict, structureDict);
        }

        public override void Update(MouseState mouseState) {
            base.Update(mouseState);
            activeItem = container.Update(mousePos);
            
        }

        public Item_LE GetActiveItem () { return activeItem; }

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