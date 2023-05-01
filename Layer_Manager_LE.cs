using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
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

        public Dictionary<string, (Point[], Point[], Texture2D)> structureParams;
        public Dictionary<string, Texture2D> tileDict;
        public Dictionary<string, string> structureDict;

        

        private HexMap_LE structures;
        private HexMap_LE tiles;
        private HexMap_LE items;

        private ContentManager content;

        private int width;
        private int height;

        public Layer_Manager_LE(int width, int height, ContentManager content) {
            this.width = width;
            this.height = height;
            background = new Layer_LE(width, height);
            ground = new Layer_LE(width, height);
            onground = new Layer_LE(width, height);
            overground = new Layer_LE(width, height);

            structures = new HexMap_LE(width, height);
            tiles = new HexMap_LE(width, height);

            this.content = content;     

            structureParams = Util.GetStructureParams(content);
            tileDict = Util.GetTileDict(content);
            structureDict = Util.GetStructureDict();      

        }

        public void Update() {

        }

        public void DrawLE (SpriteBatch spriteBatch) {
            background.Draw(spriteBatch);
            ground.Draw(spriteBatch);
            onground.Draw(spriteBatch);
            overground.Draw(spriteBatch);            
        }

        public void DrawGame (SpriteBatch spriteBatch, Player player) {
            background.Draw(spriteBatch);
            ground.Draw(spriteBatch);
            onground.Draw(spriteBatch);
            player.Draw(spriteBatch);
            overground.Draw(spriteBatch);            
        }

        public void LoadMap (Texture2D structures, Texture2D tiles) {
            Color[,] sMap = Util.GetColorMap(structures);
            Color[,] tMap = Util.GetColorMap(tiles);

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    string sHex = ColorHelper.ToHex(sMap[i,j]);
                    if (sHex != "#00000000") {
                        (var onGroundPoints, var overGroundPoints, var texture) = structureParams[structureDict[sHex]];
                        AddStructure(new Structure(sHex, onGroundPoints, overGroundPoints, texture, new Point(i,j)));
                    }

                    string tHex = ColorHelper.ToHex(tMap[i,j]);
                    if (tHex != "#00000000") {
                        AddTile(new Tile(tHex, new Point(i,j)));
                    }
                }
            }
            
        }



        public void AddStructure(Structure structure) {
            
            var sOnGround = structure.GetOnGround();
            var sOverGround = structure.GetOverGround();

            for (int i = 0; i < structure.width; i++) {
                for (int j = 0; j < structure.height; j++) {
                    if(sOnGround[i,j] != null)
                        onground.AddToLayer(sOnGround[i,j], structure.GetPos() + new Point(i,j));
                    if(sOverGround[i,j] != null)
                        overground.AddToLayer(sOverGround[i,j], structure.GetPos() + new Point(i,j));
                }
            }

            structures.AddHexcode(structure.hexcode, structure.GetPos());
                
        }

        public void AddTile(Tile tile) {
            ground.AddToLayer(tileDict[tile.hexcode], tile.pos);
            tiles.AddHexcode(tile.hexcode, tile.pos);
        }

        public void SaveHexMaps() {
            structures.SaveMap("testStructures");
            tiles.SaveMap("testTiles");

        }



        
    }
    
    
}