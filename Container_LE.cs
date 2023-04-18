using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Container_LE{ 

        Texture2D texture;
        Rectangle rect;

        List<Item_LE> items;

        SaveButton_LE saveButton;



        public Container_LE(Dictionary<string, Texture2D> textureDict) {   
            //See assets_LE.json for file names    
            this.texture = textureDict["container_LE"];
            this.rect = new Rectangle(  0, 
                                        0, 
                                        Futhark_Game.screenHeight * texture.Width / texture.Height, 
                                        Futhark_Game.screenHeight);

            Texture2D borderTexture = textureDict["border_LE"];

            saveButton = new SaveButton_LE(textureDict["save_LE"]);             
                
                    
                    
                
                items.Add(new Item_LE(_type, _texture, borderTexture));
                if(key == "objects") {
                    foreach(var i in val) {
                        Texture2D texture = Content.Load<Texture2D>(i);
                        items.Add(new LE_Item(i, texture.Width / overlay["border"].Item1.Width));
                    }
                } else if(key == "tiles") {
                    foreach(var i in val) {
                        
                    }
                }
        }

        public void Update() {

        }

        public void Draw() {
            
        }
           

        
    }
    
    
}