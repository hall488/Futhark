using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Collections.Generic;
//using sysD = System.Drawing; //only works on windows


namespace Futhark
{
    public class Futhark_Game : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Menu mainMenu;

        private LevelManager levelManager;

       

        public static int screenWidth;

        public static int screenHeight;

        enum gameStates {
            mainMenu,
            levelManager
        }

        int gameState = 0;
        int prevGameState = 0;
        

        public Futhark_Game()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //_graphics.PreferredBackBufferWidth = 700;  // set this value to the desired width of your window
            //_graphics.PreferredBackBufferHeight = 700;   // set this value to the desired height of your window
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D buttonTexture = new Texture2D(GraphicsDevice, 1, 1);

            mainMenu = new Menu(Content, spriteBatch, graphics, GraphicsDevice);
            levelManager = new LevelManager(Content, spriteBatch, graphics, GraphicsDevice);


            switch(gameState) {
                    case (int) gameStates.mainMenu:
                        mainMenu.LoadContent();
                        break;
                    case (int) gameStates.levelManager:
                        levelManager.LoadContent();
                        break;
                        
                }
        }

        void LoadGameContent()
        {
            

          

            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(gameState != prevGameState) {
                switch(gameState) {
                    case (int) gameStates.mainMenu:
                        mainMenu.LoadContent();
                        break;
                    case (int) gameStates.levelManager:
                        levelManager.LoadContent();
                        break;
                        
                }
            }


            // TODO: Add your update logic here
            switch(gameState) {
                case (int) gameStates.mainMenu:
                    gameState = mainMenu.Update();
                    break;
                case (int) gameStates.levelManager:
                    levelManager.Update();
                    break;
            }            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            

            switch(gameState) {
                case (int) gameStates.mainMenu:
                    mainMenu.Draw();
                    break;
                case (int) gameStates.levelManager:
                    levelManager.Draw();
                    break;
            }            

            prevGameState = gameState;
            base.Draw(gameTime);
        }

        

        
    }
}
