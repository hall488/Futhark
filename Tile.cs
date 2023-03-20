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

        public Tile(sysD.Color _color, int _x, int _y) {
            color = _color;
            x = _x;
            y= _y;
            
        }

        public void Update() {

        }

        public void Draw() {

        }
    }
}