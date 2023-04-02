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
        int runePos = 0;
        private Dictionary<string, (Keys, bool)> castKeyStates;
        private Dictionary<string, (Keys, bool)> runeKeyStates;
        private Dictionary<string, string> spellDict;

        private Dictionary<string, string[]> keyToRune;

        List<Keys> runesPrev;
        List<Keys> castsPrev;

        List<Keys> runesPressed;
        List<Keys> castsPressed;

        List<string> spellOrder;

        bool castActive = false;

        int width = 64;
        int height = 64;

        public Runestone(Texture2D[] _aettsTextures, Game_Constants gConstants) {

            aettsTextures = _aettsTextures;
            castKeyStates = gConstants.castKeys;
            runeKeyStates = gConstants.runeKeys;
            spellDict = gConstants.spellDict;
            keyToRune = new Dictionary<string, string[]>();
            spellOrder = new List<string>();
            runesPressed = new List<Keys>();
            castsPressed = new List<Keys>();
            runesPrev = new List<Keys>();
            castsPrev = new List<Keys>();

            keyToRune.Add("Rune 1", new String[] {"Fehu", "Hagalaz", "Tiwaz"});
            keyToRune.Add("Rune 2", new String[] {"Uruz", "Nauthiz", "Berkana"});
            keyToRune.Add("Rune 3", new String[] {"Thurisaz", "Isa", "Ehwaz"});
            keyToRune.Add("Rune 4", new String[] {"Ansuz", "Jera", "Mannaz"});
            keyToRune.Add("Rune 5", new String[] {"Raido", "Eihwaz", "Laguz"});
            keyToRune.Add("Rune 6", new String[] {"Kenaz", "Perthro", "Ingwaz"});
            keyToRune.Add("Rune 7", new String[] {"Gebo", "Algiz", "Dagaz"});
            keyToRune.Add("Rune 8", new String[] {"Wunjo", "Sowilo", "Othala"});

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

            // foreach(var t in runesPressed) {
            //     Console.Write("{0}, ", t);
            // }

            // Console.WriteLine();

            // foreach(var t in runesPrev) {
            //     Console.Write("{0}, ", t);
            // }

            // Console.WriteLine();

            singleDictKeyTouch(runeKeyStates, runesPressed, runesPrev);            
            
            runesPrev = new List<Keys>(runesPressed);
            runesPressed.Clear();

            
            if(castActive) {
                runePos = 0;
                foreach((var key, var val) in runeKeyStates) {
                    if(aettType == 0 && val.Item2) {
                        if(key == "Rune 1") {
                            aettType = 1;
                            runePos = 0;
                        } else if(key == "Rune 2") {
                            aettType = 2;
                            runePos = 1;
                        } else if(key == "Rune 3") {
                            aettType = 3;
                            runePos = 2;
                        }
                        break;
                    } else if(val.Item2) {
                        if(key == "Rune 1") {
                            runePos = 0;
                        } else if(key == "Rune 2") {
                            runePos = 1;
                        } else if(key == "Rune 3") {
                            runePos = 2;
                        } else if(key == "Rune 4") {
                            runePos = 3;
                        } else if(key == "Rune 5") {
                            runePos = 4;
                        } else if(key == "Rune 6") {
                            runePos = 5;
                        } else if(key == "Rune 7") {
                            runePos = 6;
                        } else if(key == "Rune 8") {
                            runePos = 7;
                        }
                        spellOrder.Add(keyToRune[key][aettType-1]);
                        aettType = 0;
                    }                    
                }                    
                
            }

            if(castKeyStates["Cast"].Item2) {
                if(castActive) {
                    //Console.WriteLine("End Cast");
                    castActive = false;
                    var spell = String.Join(", ", spellOrder.ToArray());
                    if(spellDict.ContainsKey(spell)) {
                        //Console.WriteLine(spellDict[spell]);
                    } else {
                        //Console.WriteLine("Invalid spell!");
                    }
                    spellOrder.Clear();
                } else {
                    //Console.WriteLine("Begin Cast");
                    castActive = true;
                    aettType = 0;                    
                }
            }


            // foreach((var a, var b) in runeKeyStates) {
            //     Console.Write("{0}: {1}, ", a, b);
                
            // }
            // Console.WriteLine();
            
            // foreach(var i in spellOrder) {
            //     Console.Write(i);
            //     Console.Write(", ");
            // }
            // Console.WriteLine();

        }

        public void Draw(SpriteBatch spriteBatch, int playerX, int playerY) {

            int tempCol = 0;

            if(runePos >= 5) {
                runePos = runePos - 5;
                tempCol = 1;
            }

            Rectangle sourceRectangle = new Rectangle(width * runePos, height * tempCol, width, height);
            Rectangle destinationRectangle = new Rectangle(150 + playerX, 150 + playerY, width*4, height*4);
            
            if(castActive)
                spriteBatch.Draw(aettsTextures[aettType], destinationRectangle, sourceRectangle, Color.White);
        }

        private void singleDictKeyTouch(Dictionary<string, (Keys, bool)> dictPressed, List<Keys> pressedKeys, List<Keys> prevPressed) {           

            // foreach(var k in pressedKeys) {                
            //     foreach((var key, var val) in dictPressed) {
            //         if(k == val.Item1) {
            //             if(val.Item2 == false && !prevPressed.Contains(k)) {
            //                 //Have to set whole tuple again because it isnt mutable
            //                 dictPressed[key] = (val.Item1, true);
            //                 //Console.WriteLine("{0} with key {1} has been set to true", key, val.Item1);
            //             } else if(val.Item2 == true){
            //                 dictPressed[key] = (val.Item1, false);
            //                 //Console.WriteLine("{0} with key {1} has been set to false1", key, val.Item1);
            //             }
            //         }
            //     }
            // }

            foreach(var k in prevPressed) {                
                if(!pressedKeys.Contains(k)) {
                    foreach((var key, var val) in dictPressed) {
                        if(k == val.Item1) {
                            dictPressed[key] = (val.Item1, true);
                            Console.WriteLine("{0} : true", val.Item1);
                        }
                    }                    
                }
            }

            foreach((var key, var val) in dictPressed) {
                if(!prevPressed.Contains(val.Item1)) {
                    dictPressed[key] = (val.Item1, false);
                }
            }

            // foreach((var key, var val) in dictPressed) {
            //     if(prevPressed.Contains(val.Item1) && val.Item2 == true) {
            //         dictPressed[key] = (val.Item1, false);
            //         //Console.WriteLine("{0} with key {1} has been set to false2", key, val.Item1);
            //     }
            // }

        }

        private void castSpell(string spell) {
            
        }
    }
}