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

        private Game_Constants gConstants;

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

        private TiledLayer spawns;
        private TiledLayer doors;

        private TiledObject playerSpawn;

        private RenderTarget2D renderTarget;

        public static Rectangle rectRT;

        public static Point screenOffset;
        Rectangle[,] collidable;

        Dictionary<Rectangle, string> doorDict;

        string currentMap;

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

            renderTarget = new RenderTarget2D(this.graphicsDevice,
                                                graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            // var rtWidth = graphicsDevice.PresentationParameters.BackBufferHeight / 2 * 3;
            // var screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            // screenOffset = new Point((screenWidth - rtWidth)/2,0);
            rectRT = new Rectangle(0,0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        private void resetRT() {
            renderTarget.Dispose();
            renderTarget = new RenderTarget2D(this.graphicsDevice,
                                                graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight,
                                                false,
                                                graphicsDevice.PresentationParameters.BackBufferFormat,
                                                DepthFormat.Depth24);

            // var rtWidth = graphicsDevice.PresentationParameters.BackBufferHeight / 2 * 3;
            // var screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            // screenOffset = new Point((screenWidth - rtWidth)/2,0);
            rectRT = new Rectangle(0,0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public void LoadContent() {
            camera = new Camera(renderTarget.Width, renderTarget.Height);
            

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

            Dictionary<string, Texture2D> spellTextures = new Dictionary<string, Texture2D>();

            Texture2D playerTexture = Content.Load<Texture2D>("young_skald");
            spellTextures.Add("fireball", Content.Load<Texture2D>("fireball"));
            Texture2D[] aettsTextures = new Texture2D[4];
            aettsTextures[0] = Content.Load<Texture2D>("Aetts");
            aettsTextures[1] = Content.Load<Texture2D>("Frey_Aett");
            aettsTextures[2] = Content.Load<Texture2D>("Hagal_Aett");
            aettsTextures[3] = Content.Load<Texture2D>("Tyr_Aett");
            
            gConstants = new Game_Constants(new Texture2D(graphicsDevice, 1, 1), spellTextures, camera);

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
            groundLayer = map.Layers.First(l => l.name == "Ground");
            belowLayer = map.Layers.First(l => l.name == "Below");
            collisionLayer = map.Layers.First(l => l.name == "Collisions");
            spawns = map.Layers.First(l => l.name == "Spawns");
            playerSpawn = spawns.objects.First(o => o.name == playerSpawnName);
            doors = map.Layers.First(l => l.name == "Doors");

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


            player = new Player(gConstants, playerTexture, aettsTextures, 
                                            (int) (playerSpawn.x + 8)*8, 
                                            (int) (playerSpawn.y  - 12)*8, (Direction)Int32.Parse(playerSpawn.properties[0].value), 
                                            collidable, doorDict, new Texture2D(graphicsDevice, 1, 1));
            player.currentMap = currentMap;

            
        }

        public int Update() {
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.M))
                return (int) Futhark_Game.gameStates.mainMenu;

            var levelReturn = player.Update();
            if(levelReturn != currentMap) {
                resetRT();
                loadLevel(levelReturn);
            }
            camera.Follow(player);
            return (int) Futhark_Game.gameStates.levelManager;
        }

        public void Draw() {


            DrawRT();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spriteBatch.Draw(renderTarget, rectRT, Color.White);            

            spriteBatch.End();
            
            //back layer
            //layerManager.DrawGame(spriteBatch, player);
            //over_layer
            //tilemap_buildings.Draw(_spriteBatch);
        }

        public void DrawRT() {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() {DepthBufferEnable = true};

            graphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, transformMatrix: camera.Transform);            
            
            DrawLayers(belowLayer);
            DrawLayers(groundLayer);
            player.Draw(spriteBatch);
            DrawLayers(aboveLayer);

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