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
        private Dictionary<string, (Keys, bool)> castKeyStates;
        private Dictionary<string, (Keys, bool)> runeKeyStates;

        List<Keys> runesPrev;
        List<Keys> castsPrev;

        List<Keys> runesPressed;
        List<Keys> castsPressed;

        List<string> spellOrder;

        bool castActive = false;

        public Runestone(Texture2D[] _aettsTextures, Dictionary<string, (Keys, bool)> _castKeyStates, Dictionary<string, (Keys, bool)> _runeKeyStates) {

            aettsTextures = _aettsTextures;
            castKeyStates = _castKeyStates;
            runeKeyStates = _runeKeyStates;
            spellOrder = new List<string>();
            runesPressed = new List<Keys>();
            castsPressed = new List<Keys>();
            runesPrev = new List<Keys>();
            castsPrev = new List<Keys>();
        }

        public void Update(KeyboardState keyboardState) {   

            foreach((var key, var val) in castKeyStates) {
                if(keyboardState.IsKeyDown(val.Item1)) {
                    castsPressed.Add(val.Item1);
                }
            }   

            singleDictKeyTouch(castKeyStates, castsPressed, castsPrev);

            castsPrev = new List<Keys>(castsPressed);
            castsPressed.Clear();
            
            foreach((var key, var val) in runeKeyStates) {
                if(keyboardState.IsKeyDown(val.Item1)) {
                    runesPressed.Add(val.Item1);
                }
            } 

            singleDictKeyTouch(runeKeyStates, runesPressed, runesPrev);            
            
            runesPrev = new List<Keys>(runesPressed);
            runesPressed.Clear();

            
            if(castActive) {
                foreach((var key , var val) in runeKeyStates) {
                    if(val.Item2) {
                        spellOrder.Add(key);
                    }
                }
            }

            if(castKeyStates["Cast"].Item2) {
                if(castActive) {
                    //Console.WriteLine("End Cast");
                    castActive = false;
                    spellOrder.Clear();
                } else {
                    //Console.WriteLine("Begin Cast");
                    castActive = true;
                }
            }

            foreach((var a, var b) in runeKeyStates) {
                Console.Write("{0}: {1}, ", a, b);
                
            }
            Console.WriteLine();
            
            // foreach(var i in spellOrder) {
            //     Console.Write(i);
            //     Console.Write(", ");
            // }
            // Console.WriteLine();

        }

        public void Draw() {

        }

        private void singleDictKeyTouch(Dictionary<string, (Keys, bool)> dictPressed, List<Keys> pressedKeys, List<Keys> prevPressed) {           

            foreach(var k in pressedKeys) {                
                foreach((var key, var val) in dictPressed) {
                    if(k == val.Item1 && val.Item2 == false && !prevPressed.Contains(k)) {
                        //Have to set whole tuple again because it isnt mutable
                        dictPressed[key] = (val.Item1, true);
                    } else {
                        dictPressed[key] = (val.Item1, false);
                    }
                }
            }

            foreach(var k in prevPressed) {
                if(!pressedKeys.Contains(k)) {
                    foreach((var key, var val) in dictPressed) {
                        dictPressed[key] = (val.Item1, false);
                    }
                }
            }

        }
    }
}