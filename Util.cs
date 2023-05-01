using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Futhark {

    public static class Util{ 

        public static Dictionary<string, (Point[], Point[], Texture2D)> GetStructureParams(ContentManager content) {
            var structureParams = new Dictionary<string, (Point[], Point[], Texture2D)>();

            var assetFolders = Directory.GetDirectories("assets/Structures");
            
            foreach(var f in assetFolders) {
                Console.WriteLine(f);

                Texture2D texture = content.Load<Texture2D>(f + "/image");
                string jsonFile = File.ReadAllText("text_" + f + "/layers.json");
                 
                Dictionary<string, Point[]> layerDict = JsonConvert.DeserializeObject<Dictionary<string, Point[]>>(jsonFile);
                
                structureParams.Add(f.Split("/")[2], (layerDict["onGround"], layerDict["overGround"], texture));
            }

            return structureParams;
        }

        public static Dictionary<string, Texture2D> GetTileDict(ContentManager content) {
            string jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");

            var tempDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
            var tileDict = new Dictionary<string, Texture2D>();

            foreach((var key , var val) in tempDict) {
                tileDict.Add(key, content.Load<Texture2D>(val));
            }

            return tileDict;
        }

        public static Dictionary<string, string> GetStructureDict() {
            string jsonFile = File.ReadAllText("text_assets/structure_dictionary.json");

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);      

        }
            

        public static Color[,] GetColorMap(Texture2D a) {
            Color[] D1 = new Color[a.Width * a.Height];
            a.GetData<Color>(D1);

            Color[,] map = new Color[a.Width, a.Height];

            for(int i = 0; i < a.Width; i++) {
                for(int j = 0; j < a.Height; j++) {
                    map[j, i] = D1[i * a.Width + j];
                }
            }

            return map;
        }

        public static T[,] GetNew2DArray<T>(int x, int y, T initialValue)
        {
            T[,] nums = new T[x, y];
            for (int i = 0; i < x * y; i++) nums[i % x, i / x] = initialValue;
            return nums;
        }

        public static Texture2D[] Split(Texture2D original, int partWidth, int partHeight, out int xCount, out int yCount)
        {
            yCount = original.Height / partHeight;//The number of textures in each horizontal row
            xCount = original.Width / partWidth;//The number of textures in each vertical column
            
            Texture2D[] r = new Texture2D[xCount * yCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[index++] = part;
                }
            //Return the array of parts.
            return r;
        }

        
    }
    
    
}