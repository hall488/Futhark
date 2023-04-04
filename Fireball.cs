using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Fireball : Projectile { 


        public Fireball(int posX, int posY, int vel, double unitX, double unitY, Texture2D texture) {
            this.posX = posX;
            this.posY = posY;
            this.vel = vel;
            this.unitX = unitX;
            this.unitY = unitY;
            this.animation = new AnimatedSprite(texture, 1, 3, 0);
            this.width = texture.Width / 3;
            this.height = texture.Height;
            
            this.rot = (float)Math.Atan2(unitY, unitX);

        }

        public override bool Update(Tilemap activeTiles) {
            animation.Update();
            return base.Update(activeTiles);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            animation.Draw(spriteBatch, posX, posY, rot + (float)Math.PI / 2);
            base.Draw(spriteBatch);
        }
    }
    
}