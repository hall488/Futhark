using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
//using sysD = System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace Futhark {
    
    public class Game_Constants {

        public Dictionary<string,string> tileDict;

        public Dictionary<Keys, bool> keysDict;

        public Texture2D tileColTexture;

        public Game_Constants(Texture2D _tileColTexture) {
            
            tileColTexture = _tileColTexture;
            tileDict = new Dictionary<string, string>();
            keysDict = new Dictionary<Keys, bool>();

            string[] txtFile = File.ReadAllLines("text_assets/tile_dictionary.txt");
            
            foreach(string line in txtFile) {
                var cols = line.Split("=");
                tileDict.Add(cols[0], cols[2]);
            }

            txtFile = File.ReadAllLines("text_assets/keys_dictionary.txt");
            
            foreach(string line in txtFile) {
                var cols = line.Split("=");
                keysDict.Add(cols[0], cols[1]);
            }
            
        }

    }

}