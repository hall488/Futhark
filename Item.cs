using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Item{ 

        public Rectangle borderRect;
        public Rectangle itemRect;

        public Texture2D borderTexture;

        public Texture2D itemTexture;

        public string identifier;

        public Color highlight;


        public Item(string identifier, Texture2D itemTexture, Texture2D borderTexture) {
            this.identifier = identifier;
            this.itemTexture = itemTexture;
            this.borderTexture = borderTexture;
            this.highlight = Color.White;
        }

        public void Update() {
        }

        public void Draw(SpriteBatch spriteBatch) {
            if(borderTexture != null)
                spriteBatch.Draw(borderTexture, borderRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(itemTexture, itemRect, null, highlight, 0, new Vector2(itemTexture.Width / 2, itemTexture.Height / 2), SpriteEffects.None, 0f);
        }

        public void setRectangles(int x, int y, Texture2D cTexture, Rectangle cRect){
            borderRect = new Rectangle( cRect.Width / cTexture.Width * (4+(2+borderTexture.Width)*x),
                                            cRect.Height / cTexture.Height * (11+(2+borderTexture.Height)*y),
                                            cRect.Width / cTexture.Width * borderTexture.Width,
                                            cRect.Height / cTexture.Height * borderTexture.Height);
                

                
            double scale = itemTexture.Width < itemTexture.Height ? (double)itemTexture.Height : (double)itemTexture.Width;
            itemRect = new Rectangle(   borderRect.X + 2*borderRect.Width / borderTexture.Width + (int)(borderRect.Width / borderTexture.Width * 16) / 2,
                                            borderRect.Y + 2*borderRect.Height / borderTexture.Height + (int)(borderRect.Height / borderTexture.Height * 16) / 2,
                                            (int)(borderRect.Width / borderTexture.Width *  itemTexture.Width / scale * 16),
                                            (int)(borderRect.Height / borderTexture.Height * itemTexture.Height / scale * 16));
        }

           

        
    }
    
    
}