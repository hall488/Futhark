using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using sysD = System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace Futhark {
    
    public class Game_Constants {

        public Dictionary<string,string> tileDict;

        public Texture2D tileColTexture;

        public Game_Constants(Texture2D _tileColTexture) {
            
            tileColTexture = _tileColTexture;
            tileDict = new Dictionary<string, string>();

            string[] txtFile = File.ReadAllLines("assets/tile_dictionary.txt");
            
            foreach(string line in txtFile) {
                var cols = line.Split(":");
                tileDict.Add(cols[0], cols[2]);
            }
            
        }

    }

}