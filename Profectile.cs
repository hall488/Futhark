using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Projectile { 

        protected int posX;
        protected int posY;
        protected int vel;
        protected double unitX;
        protected double unitY;

        protected AnimatedSprite animation;

        protected Rectangle colRect;

        public Projectile() {

        }

        public virtual void Update() {

            if(!(unitX == 0 && unitY == 0)) {

                double mag = Math.Sqrt(unitX*unitX + unitY*unitY);

                unitX = unitX / mag;
                unitY = unitY / mag;

            }
        }

        public virtual void Draw() {

        }
    }
    
}