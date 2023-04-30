using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Futhark {

    public class Layer_Manager_LE{ 

        public Layer_LE background;

        public Layer_LE ground;
        public Layer_LE onground;

        public Layer_LE overground;

        public Dictionary<string, Texture2D> tileDict;

        public Layer_Manager_LE(int width, int height, Dictionary<string, Texture2D> tileDict) {
             background = new Layer_LE(width, height);
             ground = new Layer_LE(width, height);
             onground = new Layer_LE(width, height);
             overground = new Layer_LE(width, height);

            this.tileDict = tileDict;           

        }

        public void Update() {

        }

        public void Draw (SpriteBatch spriteBatch) {
            background.Draw(spriteBatch);
            ground.Draw(spriteBatch);
            onground.Draw(spriteBatch);
            overground.Draw(spriteBatch);            
        }

        public void AddStructure(Structure structure) {
            
            var sOnGround = structure.GetOnGround();
            var sOverGround = structure.GetOverGround();

            for (int i = 0; i < structure.width; i++) {
                for (int j = 0; j < structure.height; j++) {
                    onground.AddToLayer(sOnGround[i,j], structure.GetPos() + new Point(i,j));
                }
            }

            for (int i = 0; i < structure.width; i++) {
                for (int j = 0; j < structure.height; j++) {
                    overground.AddToLayer(sOverGround[i,j], structure.GetPos() + new Point(i,j));
                }
            }
                
        }

        public void AddTile(Tile tile) {
            ground.AddToLayer(tileDict[tile.hexcode], tile.pos);
        }

        public void SaveMap() {

        }



        
    }
    
    
}