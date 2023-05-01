using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using SI = SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Futhark {

    public class Layer_LE{ 

        Texture2D[,] textures;

        string[,] hexcodes;

        private int width;

        private int height;

        public Layer_LE(int width, int height) { 
            this.width = width;
            this.height = height;
            textures = new Texture2D[width, height];
            hexcodes = Util.GetNew2DArray<string>(width, height, "#00000000");
        }

        public void Update() {

        }

        public void Draw (SpriteBatch spriteBatch) {
            for (int i = 0; i < textures.GetLength(1); i++) {
                for(int j = 0; j < textures.GetLength(0); j++) {
                    if(textures[i,j] != null) {
                        Texture2D texture = textures[i,j];
                        spriteBatch.Draw(texture, new Rectangle(texture.Width * i * 8, texture.Height * j * 8, texture.Width * 8, texture.Height * 8), Color.White);
                    }
                }
            }
        }

        public void AddToLayer(Texture2D texture, Point pos) {
            Console.WriteLine("{0},{1}", new Point(textures.GetLength(1), textures.GetLength(0)), pos);
            textures[pos.X, pos.Y] = texture;
        }

        public void AddHexcode(string hexcode, Point pos) {
            hexcodes[pos.X, pos.Y] = hexcode;
        }

        public void SaveLayer() {

            

            
        }

        
    }
    
    
}