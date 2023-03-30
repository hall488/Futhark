using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Futhark {

    public class Runestone {

        Texture2D[] aettsTextures;
        int aettType = 0;
        private Dictionary<string, (Keys, bool)> pressedRunes;

        List<Keys> prevPressed;

        List<Keys> pressedKeys;

        List<string> spellOrder;

        public Runestone(Texture2D[] _aettsTextures, Dictionary<string, (Keys, bool)> _pressedRunes) {

            aettsTextures = _aettsTextures;
            pressedRunes = _pressedRunes;
            spellOrder = new List<string>();
            pressedKeys = new List<Keys>();
            prevPressed = new List<Keys>();
        }

        public void Update(KeyboardState keyboardState) {   
            
            foreach((var key, var val) in pressedRunes) {
                if(keyboardState.IsKeyDown(val.Item1)) {
                    pressedKeys.Add(val.Item1);
                }
            }            

            foreach(var k in pressedKeys) {
                foreach((var key, var val) in pressedRunes) {
                    if(k == val.Item1 && val.Item2 == false && !prevPressed.Contains(k)) {
                        //Have to set whole tuple again because it isnt mutable
                        pressedRunes[key] = (val.Item1, true);
                    } else {
                        pressedRunes[key] = (val.Item1, false);
                    }
                }
            }

            foreach(var k in prevPressed) {
                if(!pressedKeys.Contains(k)) {
                    foreach((var key, var val) in pressedRunes) {
                        pressedRunes[key] = (val.Item1, false);
                    }
                }
            }

            prevPressed = new List<Keys>(pressedKeys);
            pressedKeys.Clear();

            foreach((var key , var val) in pressedRunes) {
                if(val.Item2) {
                    spellOrder.Add(key);
                }
            }
            
            foreach(var i in spellOrder) {
                Console.Write(i);
                Console.Write(", ");
            }
            Console.WriteLine();

        }

        public void Draw() {

        }
    }
}