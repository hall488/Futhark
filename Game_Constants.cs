using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



namespace Futhark {
    
    public class Game_Constants {

        public Dictionary<Color,string> tileDict;

        public Dictionary<Keys, bool> keysDict;

        public Texture2D tileColTexture;

        public Game_Constants(Texture2D _tileColTexture) {
            
            tileColTexture = _tileColTexture;
            keysDict = new Dictionary<Keys, bool>();

            string jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");

            Dictionary<Color, string> tileDict = JsonConvert.DeserializeObject<Dictionary<Color, string>>(jsonFile, new ColorJsonConverter());
            
            
            // foreach(string line in txtFile) {
            //     var cols = line.Split("=");
            //     tileDict.Add(cols[0], cols[2]);
            // }

            // txtFile = File.ReadAllLines("text_assets/keys_dictionary.txt");
            
            // foreach(string line in txtFile) {
            //     var cols = line.Split("=");
            // }
            
        }

    }

}