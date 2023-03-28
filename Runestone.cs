using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Runestone {

        Texture2D[] aettsTextures;
        private Dictionary<string, (Keys, bool)> pressedRunes;

        public Runestone(Texture2D[] _aettsTextures, Dictionary<string, (Keys, bool)> _pressedRunes) {

            aettsTextures = _aettsTextures;
            pressedRunes = _pressedRunes;
            
        }

        public void Update(KeyboardState keyboardState) {   
            
            if(keyboardState.IsKeyDown(pressedKeys["Rune 1"].Item1)) {

            } else if(keyboardState.IsKeyDown(pressedKeys["Rune 2"].Item1)) {

            } else if(keyboardState.IsKeyDown(pressedKeys["Rune 3"].Item1)) {

            } else if(keyboardState.IsKeyDown(pressedKeys["Rune 4"].Item1)) {

            } else if(keyboardState.IsKeyDown(pressedKeys["Rune 5"].Item1)) {

            } else if(keyboardState.IsKeyDown(pressedKeys["Rune 6"].Item1)) {

            } else if(keyboardState.IsKeyDown(pressedKeys["Rune 7"].Item1)) {

            } else if(keyboardState.IsKeyDown(pressedKeys["Rune 8"].Item1)) {

            }

        }

        public void Draw() {

        }
    }
}