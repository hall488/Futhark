using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using sysD = System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace Futhark {

    public class Tilemap {

        sysD.Bitmap bmp;

        private Dictionary<string, Texture2D> tileDict;

        Tile[,] tilemap;

        const int tileLength = 128;

        int width;
        int height;

        public Tilemap(ContentManager content, sysD.Bitmap _bmp, Dictionary<string, string> _tileDict) {
            bmp = _bmp;

            tileDict = new Dictionary<string, Texture2D>();
            
            foreach(var (key, value) in _tileDict) {
                tileDict.Add(key, content.Load<Texture2D>(value));
            }

            width = bmp.Width;
            height = bmp.Height;

            tilemap = new Tile[width,height];

            for(int i=0; i < width; i++) {
                for(int j=0; j < height; j++) {
                    tilemap[i,j] = new Tile(bmp.GetPixel(i,j), i, j);
                }
            }
        }

        public void Update() {
            
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var t in tilemap) {
                Rectangle destinationRectangle = new Rectangle(t.x*tileLength, t.y*tileLength, tileLength, tileLength);

                spriteBatch.Draw(tileDict[t.color.ToString()], destinationRectangle, Color.White);
            }
        }
    }
}