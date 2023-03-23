using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using sysD = System.Drawing;

namespace Futhark {

    public class Tile {

        public sysD.Color color { get; set; }
        public int x {get; set; }
        public int y {get; set; }

        public int xCord {get; set;}

        public int yCord {get; set;}

        public bool solid {get; set; }

        public Rectangle tileRect {get; set;}

        public int tileLength {get; set;}
        public Texture2D tileDebugTexture;

        public Tile(sysD.Color _color, int _xCord, int _yCord, bool _solid, int _tileLength, Texture2D _tileDebugTexture) {
            tileLength = _tileLength;
            color = _color;
            xCord = _xCord;
            yCord = _yCord;
            x = xCord * tileLength;
            y= yCord * tileLength;
            tileDebugTexture = _tileDebugTexture;

            if(color.ToArgb() != sysD.Color.White.ToArgb()) {
                solid = _solid;                
            } else {
                solid = false;
            }

            tileRect = new Rectangle(x, y, tileLength, tileLength);
            
            tileDebugTexture.SetData(new[] {new Color(0, 0, 255, 64)});
        }

        public void Update() {

        }

        public void Draw(SpriteBatch spriteBatch) {
            if(solid)
                spriteBatch.Draw(tileDebugTexture, tileRect, Color.White);
        }
    }
}