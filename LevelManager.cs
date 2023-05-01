using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Futhark {

    public class LevelManager { 

         private Tilemap tilemap_back;

        private Tilemap tilemap_mid;
        private Tilemap tilemap_over;

        private Layer_Manager_LE layerManager;
        private Player player;

        private Game_Constants gConstants;

        private Camera _camera;

        private ContentManager Content;

        private SpriteBatch spriteBatch;

        private GraphicsDeviceManager graphics;

        private GraphicsDevice graphicsDevice;

        
        

        public LevelManager(ContentManager Content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
            this.Content = Content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;

            
        }

        public void LoadContent() {
            _camera = new Camera();

            // TODO: use this.Content to load your game content here
            Dictionary<string, Texture2D> spellTextures = new Dictionary<string, Texture2D>();

            Texture2D playerTexture = Content.Load<Texture2D>("young_skald");
            spellTextures.Add("fireball", Content.Load<Texture2D>("fireball"));
            Texture2D[] aettsTextures = new Texture2D[4];
            aettsTextures[0] = Content.Load<Texture2D>("Aetts");
            aettsTextures[1] = Content.Load<Texture2D>("Frey_Aett");
            aettsTextures[2] = Content.Load<Texture2D>("Hagal_Aett");
            aettsTextures[3] = Content.Load<Texture2D>("Tyr_Aett");
            
            gConstants = new Game_Constants(new Texture2D(graphicsDevice, 1, 1), spellTextures, _camera);


            Texture2D mid_layer = Content.Load<Texture2D>("test_mid_layer");
            Texture2D back_layer = Content.Load<Texture2D>("test_back_layer");
            Texture2D over_layer = Content.Load<Texture2D>("building_layer");

            string currentMapName = "assets/Maps/test";

            Texture2D sMap = Content.Load<Texture2D>(currentMapName + "Structures");
            Texture2D tMap = Content.Load<Texture2D>(currentMapName + "Tiles");


            layerManager = new Layer_Manager_LE(sMap.Width, sMap.Height, Content);
            layerManager.LoadMap(sMap, tMap);
            

            // tilemap_mid = new Tilemap(Content, GetColorMap(mid_layer), gConstants, true);

            // tilemap_back = new Tilemap(Content, GetColorMap(back_layer), gConstants, false);      

            // tilemap_over = new  Tilemap(Content, GetColorMap(over_layer), gConstants, false);
            

            player = new Player(gConstants, playerTexture, aettsTextures, 800, 800, layerManager.onground, new Texture2D(graphicsDevice, 1, 1));
        }

        public int Update() {
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.M))
                return (int) Futhark_Game.gameStates.mainMenu;
            player.Update();
            _camera.Follow(player);
            return (int) Futhark_Game.gameStates.levelManager;
        }

        public void Draw() {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, transformMatrix: _camera.Transform);

            //back layer
            layerManager.DrawGame(spriteBatch, player);
            //over_layer
            //tilemap_buildings.Draw(_spriteBatch);
            spriteBatch.End();
        }

        
    }
}