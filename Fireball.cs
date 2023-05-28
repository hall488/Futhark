using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using TiledCS;

namespace Futhark {

    public class Fireball : Projectile { 

        public int damage = 5;

        public Fireball(Point pos, int vel, Vector2 direction, Texture2D texture) {
            this.posX = pos.X;
            this.posY = pos.Y;
            this.vel = vel;
            this.unitX = direction.X;
            this.unitY = direction.Y;
            this.animation = new AnimatedSprite(texture, 1, 3, 0, true);
            this.width = texture.Width / 3;
            this.height = texture.Height;
            
            this.rot = (float)Math.Atan2(unitY, unitX) + (float)Math.PI / 2;

        }

        public override bool Update(Rectangle[,] collidable) {
            animation.Update();
            //Console.WriteLine("{0}", animation.x);
            return base.Update(collidable);
        }
    }
    
}