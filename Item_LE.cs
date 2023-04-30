using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Item_LE{ 

        public Rectangle borderRect;
        public Rectangle itemRect;

        public Texture2D borderTexture;

        public Texture2D itemTexture;

        public string category;

        public string fileName;

        public string hexcode;

        public Color highlight;


        public Item_LE(string category, Texture2D itemTexture, Texture2D borderTexture, int x, int y, int ratio, string hexcode, string fileName) {
            this.category = category;
            this.itemTexture = itemTexture;
            this.borderTexture = borderTexture;
            this.highlight = Color.White;
            this.hexcode = hexcode;
            this.fileName = fileName;


            if(borderTexture != null) setRectangles(x, y, ratio);
        }

        public bool Update(Point mousePos) {
            if(borderRect.Contains(mousePos)) {
                if(InputUtil.SingleLeftClick()) {
                    if(highlight == Color.White) {
                        return true;
                    }
                    else {
                        highlight = Color.White;
                        return false;
                    }
                }
            }

            return false;
                
        }

        public void Draw(SpriteBatch spriteBatch) {
            if(borderTexture != null)
                spriteBatch.Draw(borderTexture, borderRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(itemTexture, itemRect, null, highlight, 0, new Vector2(itemTexture.Width / 2, itemTexture.Height / 2), SpriteEffects.None, 0f);
        }

        public void setRectangles(int x, int y, int ratio){
            borderRect = new Rectangle(     ratio * (4+(2+borderTexture.Width)*x),
                                            ratio * (18+(2+borderTexture.Height)*y),
                                            ratio * borderTexture.Width,
                                            ratio * borderTexture.Height);
                

                
            double scale = itemTexture.Width < itemTexture.Height ? (double)itemTexture.Height : (double)itemTexture.Width;
            itemRect = new Rectangle(   borderRect.X + 2*borderRect.Width / borderTexture.Width + (int)(borderRect.Width / borderTexture.Width * 16) / 2,
                                            borderRect.Y + 2*borderRect.Height / borderTexture.Height + (int)(borderRect.Height / borderTexture.Height * 16) / 2,
                                            (int)(borderRect.Width / borderTexture.Width *  itemTexture.Width / scale * 16),
                                            (int)(borderRect.Height / borderTexture.Height * itemTexture.Height / scale * 16));
        }

        public void placeItem(Rectangle rect) {
            this.itemRect = rect;
        }

           

        
    }
    
    
}