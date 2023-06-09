using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TiledCS;

namespace Futhark {

    public class LevelManager { 

        //  private Tilemap tilemap_back;

        // private Tilemap tilemap_mid;
        // private Tilemap tilemap_over;

        // private Layer_Manager_LE layerManager;
        private Player player;

        private Game_Dicts gDicts;

        private Camera camera;

        private ContentManager Content;

        private SpriteBatch spriteBatch;

        private GraphicsDeviceManager graphics;

        private GraphicsDevice graphicsDevice;

        private TiledMap map;
        private Dictionary<int, TiledTileset> tilesets;
        private Dictionary<int, Texture2D> tsTextures;
        private TiledLayer collisionLayer;

        private TiledLayer aboveLayer;
        private TiledLayer belowLayer;

        private TiledLayer groundLayer;
        private TiledLayer ongroundLayer;

        private TiledLayer spawns;
        private TiledLayer doors;

        private TiledObject playerSpawn;

        private TiledObject[] enemySpawns;

        private RenderTarget2D sceneRT;

        public static Rectangle sceneRect;
        private RenderTarget2D overlayRT;

        public static Rectangle overlayRect;

        public static Point screenOffset;
        Rectangle[,] collidable;

        Dictionary<Rectangle, string> doorDict;

        string currentMap;

        Runestone runestone;

        List<Entity> entities;

        public enum Direction {
            Up,
            Left,
            Down,
            Right
        }


        public LevelManager(ContentManager Content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
            
            this.Content = Content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;

            Console.WriteLine("{0},{1}", graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            sceneRT = new RenderTarget2D(this.graphicsDevice,
                                                graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            // var rtWidth = graphicsDevice.PresentationParameters.BackBufferHeight / 2 * 3;
            // var screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            // screenOffset = new Point((screenWidth - rtWidth)/2,0);
            sceneRect = new Rectangle(0,0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            overlayRT = new RenderTarget2D(this.graphicsDevice,
                                                graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            overlayRect = new Rectangle(0,0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        private void resetScene() {
            sceneRT.Dispose();
            sceneRT = new RenderTarget2D(this.graphicsDevice,
                                                graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            // var rtWidth = graphicsDevice.PresentationParameters.BackBufferHeight / 2 * 3;
            // var screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            // screenOffset = new Point((screenWidth - rtWidth)/2,0);
            sceneRect = new Rectangle(0,0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public void LoadContent() {
            camera = new Camera(sceneRT.Width, sceneRT.Height);
            

            // TODO: use this.Content to load your game content here
            


            // Texture2D mid_layer = Content.Load<Texture2D>("test_mid_layer");
            // Texture2D back_layer = Content.Load<Texture2D>("test_back_layer");
            // Texture2D over_layer = Content.Load<Texture2D>("building_layer");

            // string currentMapName = "assets/Maps/test";

            // Texture2D sMap = Content.Load<Texture2D>(currentMapName + "Structures");
            // Texture2D tMap = Content.Load<Texture2D>(currentMapName + "Tiles");


            // layerManager = new Layer_Manager_LE(sMap.Width, sMap.Height, Content);
            // layerManager.LoadMap(sMap, tMap);
            

            // tilemap_mid = new Tilemap(Content, GetColorMap(mid_layer), gConstants, true);

            // tilemap_back = new Tilemap(Content, GetColorMap(back_layer), gConstants, false);      

            // tilemap_over = new  Tilemap(Content, GetColorMap(over_layer), gConstants, false);

            currentMap = "test";

            loadLevel(currentMap);

            
        }

        public void loadLevel(string name) {

            var playerSpawnName = currentMap;
            currentMap = name;

            

            Texture2D playerTexture = Content.Load<Texture2D>("young_skald");
            
            Texture2D[] aettsTextures = new Texture2D[4];
            aettsTextures[0] = Content.Load<Texture2D>("Aetts");
            aettsTextures[1] = Content.Load<Texture2D>("Frey_Aett");
            aettsTextures[2] = Content.Load<Texture2D>("Hagal_Aett");
            aettsTextures[3] = Content.Load<Texture2D>("Tyr_Aett");

            Texture2D[] runeTextures = new Texture2D[24];
            runeTextures = Util.Split(Content.Load<Texture2D>("assets/Runes"), 7, 11, out int xCount, out int yCount);

            Texture2D sayNayyerTexture = Content.Load<Texture2D>("SayNayer");
            
            gDicts = new Game_Dicts(Content);

            map = new TiledMap("assets/Maps/"+name+".tmx");
            // var castleTS = new TiledTileset("assets/Tile Sets/castle.tsx");
            // var roadTS = new TiledTileset("assets/Tile Sets/Road.tsx");
            // var specialTS = new TiledTileset("assets/Tile Sets/Special.tsx");
            tilesets = map.GetTiledTilesets("assets/Tile Sets/");

            tsTextures = new Dictionary<int, Texture2D>();
            foreach((var key, var val) in tilesets) {
                Console.WriteLine(val.Name);
                tsTextures.Add(key, Content.Load<Texture2D>("assets/Tile Sets/" + val.Name));
            }
            

            aboveLayer = map.Layers.First(l => l.name == "Above");
            ongroundLayer = map.Layers.First(l => l.name == "Onground");
            groundLayer = map.Layers.First(l => l.name == "Ground");
            belowLayer = map.Layers.First(l => l.name == "Below");
            collisionLayer = map.Layers.First(l => l.name == "Collisions");
            spawns = map.Layers.First(l => l.name == "Spawns");
            playerSpawn = spawns.objects.First(o => o.name == playerSpawnName);
            doors = map.Layers.First(l => l.name == "Doors");
            
            var enemies = map.Layers.First(l => l.name == "Enemies");

            Console.WriteLine("map w {0}", map.Width);
            Console.WriteLine("map height {0}", map.Height);
            collidable = new Rectangle[map.Width, map.Height];
            for (var y = 0; y < collisionLayer.height; y++) {
                for (var x = 0; x < collisionLayer.width; x++) {
                    if(collisionLayer.data[x + y * collisionLayer.width] != 0)
                        collidable[(int)x, (int)y] = new Rectangle((int)x * map.TileWidth * 8, (int)y * map.TileHeight * 8, map.TileWidth * 8, map.TileHeight * 8);
                }
            }

            doorDict = new Dictionary<Rectangle, string>();

            foreach(var d in doors.objects) {
                doorDict.Add(new Rectangle((int)d.x * 8, (int)(d.y - 16) * 8, (int)d.width * 8, (int)d.height * 8), d.name);
                Console.WriteLine(d.name + " added");
            }


            player = new Player(camera, gDicts, playerTexture, aettsTextures, 
                                            (int) (playerSpawn.x + 8)*8, 
                                            (int) (playerSpawn.y  - 12)*8, (Direction)Int32.Parse(playerSpawn.properties[0].value), 
                                            collidable, doorDict, new Texture2D(graphicsDevice, 1, 1));
            player.currentMap = currentMap;

            entities = new List<Entity>();
            foreach(var e in enemies.objects) {
                var enemyPoint = new Point((int) (e.x + 8)*8, (int) (e.y  - 16)*8);
                entities.Add(new SayNayer(10, enemyPoint, 4, player, sayNayyerTexture, runeTextures));
            }

            runestone = new Runestone(player, aettsTextures, gDicts);

            
        }

        public int Update(GameTime gameTime) {
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.M))
                return (int) Futhark_Game.gameStates.mainMenu;

            var levelReturn = player.Update();
            runestone.Update(keyboardState);

            var entitiesToRemove = new List<Entity>();

            foreach(var e in entities) {

                if(e is SayNayer s) {
                    runestone.disabledRunes.Add(s.getDisabledRune());
                }

                if(e.Update(gameTime, collidable)) {
                    entitiesToRemove.Add(e);
                    Console.WriteLine("Entity to be removed");
                }
            }


            foreach(var e in entitiesToRemove) {
                entities.Remove(e);
                Console.WriteLine("Entity removed");
            }

            entitiesToRemove.Clear();

            if(levelReturn != currentMap) {
                resetScene();
                loadLevel(levelReturn);
            }
            camera.Follow(player);


            return (int) Futhark_Game.gameStates.levelManager;
        }

        public void Draw(GameTime gameTime) {


            DrawScene(gameTime);
            DrawOverlay();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spriteBatch.Draw(sceneRT, sceneRect, Color.White);
            spriteBatch.Draw(overlayRT, overlayRect, Color.White);            

            spriteBatch.End();
            
            //back layer
            //layerManager.DrawGame(spriteBatch, player);
            //over_layer
            //tilemap_buildings.Draw(_spriteBatch);
        }

        public void DrawScene(GameTime gameTime) {
            graphicsDevice.SetRenderTarget(sceneRT);
            graphicsDevice.DepthStencilState = new DepthStencilState() {DepthBufferEnable = true};

            graphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, transformMatrix: camera.Transform);            
            
            DrawLayers(belowLayer);
            DrawLayers(groundLayer);
            DrawLayers(ongroundLayer);
            player.Draw(spriteBatch);
            foreach(var e in entities) {
                e.Draw(gameTime, spriteBatch);
            }
            DrawLayers(aboveLayer);
            //runestone.Draw(spriteBatch);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        public void DrawOverlay() {
            graphicsDevice.SetRenderTarget(overlayRT);
            graphicsDevice.DepthStencilState = new DepthStencilState() {DepthBufferEnable = true};

            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);            
            
            runestone.Draw(spriteBatch);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        public void DrawLayers(TiledLayer layer) {
            for (var y = 0; y < layer.height; y++) {
                for (var x = 0; x < layer.width; x++) {
                    var index = x + y * layer.width;

                    var gid = layer.data[index];
                    var tileX = x * map.TileWidth;
                    var tileY = y * map.TileHeight;

                    if (gid == 0) {
                        continue;
                    }

                    var mapTileset = map.GetTiledMapTileset(gid);

                        // Retrieve the actual tileset based on the firstgid property of the connection object we retrieved just now
                    var tileset = tilesets[mapTileset.firstgid];
                    var texture = tsTextures[mapTileset.firstgid];

                    // Use the connection object as well as the tileset to figure out the source rectangle
                    var rect = map.GetSourceRect(mapTileset, tileset, gid);

                    // Create destination and source rectangles
                    var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                    var destination = new Rectangle(tileX*8, tileY*8, map.TileWidth*8, map.TileHeight*8);
                    spriteBatch.Draw(texture, destination, source, Color.White);
                }
            }

        }

        
    }
}