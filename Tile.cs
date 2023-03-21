using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using sysD = System.Drawing;

namespace Futhark {

    public class Tile {

        public sysD.Color color { get; set; }
        public int x {get; set; }
        public int y {get; set; }

        public bool solid {get; set; }

        public Rectangle tileRect {get; set;}

        public int tileLength {get; set;}

        public Tile(sysD.Color _color, int _x, int _y, bool _solid, int _tileLength) {
            color = _color;
            x = _x;
            y= _y;
            tileLength = _tileLength;

            if(color != sysD.Color.White) {
                solid = _solid;
            } else {
                solid = false;
            }

            tileRect = new Rectangle(x, y, tileLength, tileLength);
            
        }

        public void Update() {

        }

        public void Draw() {

        }
    }
}