using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Futhark {

    public class Container_LE{ 

        Texture2D texture;
        Rectangle rect;

        List<Item_LE> items;

        SaveButton_LE saveButton;

        public Item_LE activeItem;



        public Container_LE(Dictionary<string, Texture2D> assetsDict, Dictionary<string, Texture2D> textureDict) {   
            //See assets_LE.json for file names    
            this.texture = assetsDict["container_LE"];
            this.rect = new Rectangle(  0, 
                                        0, 
                                        Futhark_Game.screenHeight * texture.Width / texture.Height, 
                                        Futhark_Game.screenHeight);

            int ratio = rect.Width / texture.Width;

            Texture2D borderTexture = assetsDict["border_LE"];

            saveButton = new SaveButton_LE(assetsDict["save_LE"], ratio);   

            items = new List<Item_LE>();

            int itemCount = 0;             
            
            foreach((var key, var val) in textureDict) {
                var x = itemCount % 2 == 0 ? 0 : 1;
                var y = itemCount / 2;

                items.Add(new Item_LE("tile", val, borderTexture, x, y, ratio));
                
                itemCount += 1;
            }
                
                    
            Console.WriteLine("t{0}",items.Count());
                
                
        }

        public void Update(Point mousePos) {
            saveButton.Update(mousePos);
            foreach(var i in items) {
                if(i.Update(mousePos)) {
                    items.ForEach(i => {i.highlight = Color.White;});
                    i.highlight = Color.Green;
                    break;
                }

                items.ForEach(i => {if(i.highlight == Color.Green) activeItem = i;});
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rect, Color.White);
            saveButton.Draw(spriteBatch);
            foreach(var i in items)
                i.Draw(spriteBatch);
        }
           

        
    }
    
    
}