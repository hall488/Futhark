using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using SI = SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Futhark {

    public class HexMap_LE{ 


        string[,] hexcodes;

        private int width;

        private int height;

        public HexMap_LE(int width, int height) { 
            this.width = width;
            this.height = height;
            //Image sharp loses alpha value on save
            hexcodes = Util.GetNew2DArray<string>(width, height, "#00000000");
        }

        public void Update() {

        }

        public void AddHexcode(string hexcode, Point pos) {
            hexcodes[pos.X, pos.Y] = hexcode;
        }

        public void SaveMap(string saveAs) {   
            

            using (SI.Image<Rgba32> map = new SI.Image<Rgba32>(width, height)) {
                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < height; j++) {
                        Color color = ColorHelper.FromHex(hexcodes[i,j]);
                        //Console.WriteLine("{0},{1},{2},{3}", color.R, color.G, color.B, color.A);
                        map[i, j] = new Rgba32(color.R, color.G, color.B, color.A);
                    }
                }
                                       
                SI.ImageExtensions.SaveAsPng(map, "assets/Maps/" + saveAs + ".Png");
            }
                
            
            
        }

        
    }
    
    
}