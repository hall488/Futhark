using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Fireball : Projectile { 


        public Fireball(int posX, int posY, int vel, double unitX, double unitY) {
            this.posX = posX;
            this.posY = posY;
            this.vel = vel;
            this.unitX = unitX;
            this.unitY = unitY;
        }

        public override void Update() {
            base.Update();
        }

        public override void Draw() {
            base.Draw();
        }
    }
    
}