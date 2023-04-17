using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Overlay_Object{ 

        public string type;
        public Texture2D texture;
        public Rectangle rect;


        public Overlay_Object(string type, Texture2D texture) {     
            this.type = type;
            this.texture = texture;

            if(type == "border") {
                    Texture2D texture = Content.Load<Texture2D>(val[0]);
                    overlayObjects.Add(new Overlay_Object(key, texture, Rectangle.Empty));
                } else if(type == "container") {
                    Texture2D texture = Content.Load<Texture2D>(val[0]);
                    overlayObjects.Add(new Overlay_Object(key, texture, new Rectangle(0, 
                                                            0, 
                                                            Futhark_Game.screenHeight * texture.Width / texture.Height, 
                                                            Futhark_Game.screenHeight)));
                    
                } else if (key == "buttons") {
                    Texture2D texture = Content.Load<Texture2D>(val[0]);
                    Texture2D cTexture = overlay["container"].Item1;
                    Rectangle cRect = overlay["container"].Item2;
                    overlay.Add(val[0], (texture, new Rectangle(   2 * cRect.Width / cTexture.Width,
                                                                2 * cRect.Height / cTexture.Height,
                                                                8 * cRect.Width / cTexture.Width,
                                                                4 * cRect.Height / cTexture.Height)));
                } else 
        }

        
           

        
    }
    
    
}