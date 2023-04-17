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
            Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
            foreach((var key, var val) in JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonFile)) {
                foreach(var i in val)
                    textureDict.Add(i, content.Load<Texture2D>(i));           
            }


            container = new Container_LE(textureDict);
        }

        public override void Update(MouseState mouseState) {
            base.Update(mouseState);

            
        }

        public override void Draw () {
            
        }

        
    }
    
    
}