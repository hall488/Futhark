using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Projectile : IyDraw{ 

        protected int posX;
        protected int posY;
        protected int vel;
        protected double unitX;
        protected double unitY;

        protected float rot;

        protected AnimatedSprite animation;

        protected Rectangle colRect;

        protected int width;
        protected int height;

        protected int tWidth = 128;

        int IyDraw.yPosition() => posY;
        AnimatedSprite IyDraw.animation() => animation;

        public Projectile() {
            
        }

        public virtual bool Update(Tilemap activeTiles) {
            //Console.WriteLine("{0},{1},{2},{3}", unitX, unitY, posX, posY);

            posX += (int)(unitX*vel);
            posY += (int)(unitY*vel);
            colRect = new Rectangle(posX - width / 2, posY + height/2, width, height);

            int lowerCordX = (int)Math.Round((double)(posX - tWidth)/tWidth, 0);
            int upperCordX = (int)Math.Round((double)(posX + 2*tWidth)/tWidth, 0);
            //using width here because the sprite height is taller than what the bounding box should be
            //the bounding box is one tile
            int lowerCordY = (int)Math.Round((double)(posY - tWidth)/tWidth, 0);
            int upperCordY = (int)Math.Round((double)(posY + 2*tWidth)/tWidth, 0);

            for(int i = lowerCordX; i < upperCordX; i++) {
                for(int j = lowerCordY; j < upperCordY; j++) {
                    
                    Tile t = activeTiles.tilemap[i, j];
                    // Console.Write(lowerCordX);
                    // Console.Write(" : ");
                    // Console.WriteLine(upperCordX);
                    if(t.solid) {                        
                        if(colRect.Intersects(t.tileRect)) {
                            return true;                            
                        }
                    }
                }
            }

            return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch) {

        }
    }
    
}