using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Layer_LE{ 

        Texture2D[,] textures;

        public Layer_LE(int width, int height) { 
            textures = new Texture2D[width, height];
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

        
    }
    
    
}