using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Structure{ 

        Tile[,] tiles;
        Texture2D texture;

        public Structure(Texture2D texture) {
            this.texture = texture;
            int width = texture.Width;
            int height = texture.Height;
            int cols;
            int rows;

            if(width / 16 == 0 && height / 16 == 0) {
                cols = width / 16;
                rows = height / 16;
            } else {
                throw new Exception("Texture must be multiples of 16");
            }
        }

        public void Update() {

        }

        public void Draw () {
            
        }

        
    }
    
    
}