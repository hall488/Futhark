using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace Futhark {

    public class Tile {

        public string hexcode { get; set; }
        public int x {get; set; }
        public int y {get; set; }

        public Point pos;

        public bool solid {get; set; }

        public Rectangle tileRect {get; set;}

        public int tileLength {get; set;}
        public Texture2D tileDebugTexture;

        public Tile(string hexcode, Point pos) {
            this.hexcode = hexcode;
            this.pos = pos;
            tileLength = 16;
            x = pos.X * tileLength;
            y= pos.Y * tileLength;

            tileRect = new Rectangle(x, y, tileLength, tileLength);
        }

        public void Update() {

        }

        public void Draw(SpriteBatch spriteBatch) {

        }
    }
}