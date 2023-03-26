using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Futhark {

    public class Tilemap {

        Color[,] bmp;

        private Dictionary<string, Texture2D> tileDict;

        public Tile[,] tilemap;

        const int tileLength = 128;

        int width;
        int height;

        bool solid;

        public Tilemap(ContentManager content, Color[,] _bmp, Game_Constants gConstants, bool _solid) {
            bmp = _bmp;

            solid = _solid;

            tileDict = new Dictionary<string, Texture2D>();
            
            foreach(var (key, value) in gConstants.tileDict) {
                tileDict.Add(key, content.Load<Texture2D>(value));
            }

            width = bmp.GetLength(1);
            height = bmp.GetLength(0);

            tilemap = new Tile[width,height];

            for(int i=0; i < width; i++) {
                for(int j=0; j < height; j++) {
                    tilemap[i,j] = new Tile(bmp[i,j], i, j, solid, tileLength, gConstants.tileColTexture);
                }
            }
        }

        public void Update() {
            
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var t in tilemap) {
                Rectangle destinationRectangle = new Rectangle(t.x, t.y, tileLength, tileLength);

                spriteBatch.Draw(tileDict[ColorHelper.ToHex(t.color)], destinationRectangle, Color.White);
                //Debug collision rectangles
                //t.Draw(spriteBatch);
            }
        }
    }
}