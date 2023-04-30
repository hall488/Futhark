using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System;
using System.Text.Json;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using SI = SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Futhark {

    public class StructureImporter{ 

        ContentManager content;

        Texture2D[] parts;
        string[] hexcodes;
        Texture2D texture;

        List<Point> ground;
        List<Point> overground;

        string filename;

        int cols;
        int rows;
        Dictionary<string, string> textureNames;

        public StructureImporter(ContentManager content, string filename, Texture2D texture, List<Point> ground, List<Point> overground) {
            this.content = content;
            this.filename = filename;
            this.texture = texture;
            this.ground = ground;
            this.overground = overground;
            int width = texture.Width;
            int height = texture.Height;

            if(!(width % 16 == 0 && height % 16 == 0)) {
                throw new Exception("Texture must be multiples of 16");
            }

            
            parts = Util.Split(texture, 16, 16, out cols, out rows);

            string jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");
            textureNames = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
                
            

            int partIndex = 1;

            hexcodes = new string[parts.Count()];

            foreach(var l in parts) {
                if(textureNames.Values.Contains(filename + "_" + partIndex)) {
                    var rKey = textureNames.FirstOrDefault(x => x.Value == filename + "_" + partIndex).Key;
                    textureNames.Remove(rKey);
                }

                string hexcode = uniqueHex(textureNames.Keys.ToArray(), new Color(0,0,0, 255));
                hexcodes[partIndex - 1] = hexcode;
                
                string partFileName = filename + "_" + partIndex;

                Stream stream = File.Create(Directory.GetCurrentDirectory() + "/assets/Tiles/" + partFileName + ".png");

                l.SaveAsPng(stream, 16, 16);

                stream.Dispose();

                textureNames.Add(hexcode, partFileName);

                    
                
                partIndex++;
            }

            File.WriteAllText("text_assets/tile_dictionary.json", JsonConvert.SerializeObject(textureNames));

            
            
            
        }

        public void SendToBmp() {
            

            Color[] rawData = new Color[16 * 16];
            
            

            using (SI.Image<Rgba32> groundBMP = new SI.Image<Rgba32>(cols, rows)) {
                foreach(var p in ground) {
                    Texture2D tempText = parts[p.X + p.Y * cols];
                    tempText.GetData(rawData);
                    Color color = ColorHelper.FromHex(hexcodes[p.X + p.Y * cols]);
                    
                    groundBMP[p.X, p.Y] = new Rgba32(color.R, color.G, color.B, color.A);
                }
                SI.ImageExtensions.SaveAsBmp(groundBMP, "Assets/Structures/" + filename + "_ground.Bmp");
            }

            using (SI.Image<Rgba32> overgroundBMP = new SI.Image<Rgba32>(cols, rows)) {
                foreach(var p in overground) {
                    Texture2D tempText = parts[p.X + p.Y * cols];
                    tempText.GetData(rawData);
                    Color color = ColorHelper.FromHex(hexcodes[p.X + p.Y * cols]);
                    overgroundBMP[p.X, p.Y] = new Rgba32(color.R, color.G, color.B, color.A);
                }
                SI.ImageExtensions.SaveAsBmp(overgroundBMP, "Assets/Structures/" + filename + "_overground.Bmp");
            }

            string jsonFile = File.ReadAllText("text_assets/structures_dictionary.json");
            Dictionary<string, string[]> structureNames = new Dictionary<string, string[]>();
            structureNames = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonFile);
            string[] structureExtensions = new string[2];
            structureExtensions[0] = filename + "_ground.Bmp";
            structureExtensions[1] = filename + "_overground.Bmp";

            if(structureNames.Keys.Contains(filename))
                structureNames.Remove(filename);
            structureNames.Add(filename, structureExtensions);

            File.WriteAllText("text_assets/structures_dictionary.json", JsonConvert.SerializeObject(structureNames));
        }

        

        public void Update() {

        }

        public void Draw () {
            
        }

        public static string uniqueHex(string[] keys, Color color) {
            
            if(keys.Contains(ColorHelper.ToHex(color))) {
                if(color.R != 255)
                    color.R += 1;
                else {
                    color.R = 0;
                    if(color.G != 255)
                        color.G += 1;
                    else {
                        color.G = 0;
                        if(color.B != 255)
                            color.B += 1;
                        else {
                            throw new Exception("Hex values have been maxed out!");
                        }
                    }
                }

                return uniqueHex(keys, color);

            } else {
                return ColorHelper.ToHex(color);
            }
        }

        /// <summary>
        /// Splits a texture into an array of smaller textures of the specified size.
        /// </summary>
        /// <param name="original">The texture to be split into smaller textures</param>
        /// <param name="partWidth">The width of each of the smaller textures that will be contained in the returned array.</param>
        /// <param name="partHeight">The height of each of the smaller textures that will be contained in the returned array.</param>
        

        
    }
    
    
}