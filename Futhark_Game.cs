using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using sysD = System.Drawing;

namespace Futhark
{
    public class Futhark_Game : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Tilemap tilemap_back;

        private Tilemap tilemap_mid;
        private Player player;

        private Game_Constants gConstants;

        private Camera _camera;

        public static int screenWidth;

        public static int screenHeight;


        public Futhark_Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 512;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 512;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _camera = new Camera();

            // TODO: use this.Content to load your game content here
            Texture2D playerTexture = Content.Load<Texture2D>("young_skald");
            
            gConstants = new Game_Constants();

            
            //tile = new Tile(roadTexture);
            using(sysD.Bitmap layer_back = new sysD.Bitmap("assets/test_back_layer.bmp")) {
                tilemap_back = new Tilemap(Content, layer_back, gConstants.tileDict, false);
            }
            sysD.Bitmap layer_mid = new sysD.Bitmap("assets/test_mid_layer.bmp");
            
            tilemap_mid = new Tilemap(Content, layer_mid, gConstants.tileDict, true);

            player = new Player(playerTexture, 200, 200, tilemap_mid);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update();
            _camera.Follow(player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, transformMatrix: _camera.Transform);
            tilemap_back.Draw(_spriteBatch);
            tilemap_mid.Draw(_spriteBatch);
            
            player.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
