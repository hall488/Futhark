using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Futhark {

    public class Structure{ 

        public string hexcode;
        private Texture2D[,] onGround;
        private Texture2D[,] overGround;

        private Point pos;

        public int width;
        public int height;


        public Structure(string hexcode, Point[] onPoints, Point[] overPoints, Texture2D texture, Point pos) {
            this.hexcode = hexcode;

            Texture2D[] textures = Util.Split(texture, 16, 16, out width, out height);
            this.onGround = new Texture2D[width, height];
            this.overGround = new Texture2D[width, height];
            foreach(var p in onPoints) {
                Console.WriteLine("{0}", p);
                this.onGround[p.X, p.Y] = textures[p.X + width*p.Y];
            }
            foreach(var p in overPoints) {
                this.overGround[p.X, p.Y] = textures[p.X + width*p.Y];
            }            
            
            this.pos = pos;
        }

        public void Update() {

        }

        public Texture2D[,] GetOnGround() {
            return onGround;
        }

        public Texture2D[,] GetOverGround() {
            return overGround;
        }

        public Point GetPos() {
            return pos;
        }


        
    }
    
    
}