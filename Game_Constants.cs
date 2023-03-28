using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using MES = MonoGame.Extended.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



namespace Futhark {
    
    public class Game_Constants {

        public Dictionary<string,string> tileDict;

        public Dictionary<string, int> keysDict;

        public Dictionary<string,(Keys, bool)> pressedKeys;

        public Texture2D tileColTexture;

        public Game_Constants(Texture2D _tileColTexture) {
            
            tileColTexture = _tileColTexture;
            keysDict = new Dictionary<string, int>();
            pressedKeys = new Dictionary<string, (Keys, bool)>();

            string jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");

            tileDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);

            jsonFile = File.ReadAllText("text_assets/key_dictionary.json");
            
            keysDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonFile);

            foreach((var key, var val) in keysDict) {
                pressedKeys.Add(key, ((Keys)val, false));
            }
            
            
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