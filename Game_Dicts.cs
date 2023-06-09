using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using MES = MonoGame.Extended.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



namespace Futhark {
    
    public class Game_Dicts {

        public Dictionary<string,string> tileDict;

        public Dictionary<string, Dictionary<string, int>> keysDict;

        public Dictionary<string,(Keys, bool)> runeKeys;

        public Dictionary<string,(Keys, bool)> castKeys;

        public Dictionary<string, string> spellDict;

        public Texture2D tileColTexture;

        public Dictionary<string, Texture2D> spellTextures;


        public Game_Dicts (ContentManager content) {
            
            
            spellTextures = new Dictionary<string, Texture2D>();
            keysDict = new Dictionary<string, Dictionary<string, int>>();
            runeKeys = new Dictionary<string, (Keys, bool)>();
            castKeys = new Dictionary<string, (Keys, bool)>();
            spellDict = new Dictionary<string, string>();

            spellTextures.Add("fireball", content.Load<Texture2D>("fireball"));

            string jsonFile = File.ReadAllText("text_assets/tile_dictionary.json");

            tileDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);

            jsonFile = File.ReadAllText("text_assets/keys_dictionary.json");
            
            keysDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(jsonFile);

            jsonFile = File.ReadAllText("text_assets/spells_dictionary.json");

            spellDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);

            foreach((var key, var val) in keysDict) {
                if(key == "Cast Keys") {
                    foreach((var subKey, var subVal) in val) {
                        castKeys.Add(subKey, ((Keys)subVal, false));
                    }
                } else if(key == "Rune Keys") {
                    foreach((var subKey, var subVal) in val) {
                        runeKeys.Add(subKey, ((Keys)subVal, false));
                    }
                }
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