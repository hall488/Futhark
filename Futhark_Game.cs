using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using SI = SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
//using sysD = System.Drawing; //only works on windows


namespace Futhark
{
    public class Futhark_Game : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Menu mainMenu;
        private LevelManager levelManager;
        private LevelEditor levelEditor;
        public static int screenWidth;
        public static int screenHeight;
        ContentManager mainMenuContent;
        ContentManager levelManagerContent;
        ContentManager levelEditorContent;

        public enum gameStates {
            mainMenu,
            levelManager,
            levelEditor
        }

        int gameState = 0;
        int prevGameState = 0;

        
        

        public Futhark_Game()
        {
            graphics = new GraphicsDeviceManager(this);

            mainMenuContent = new ContentManager(Content.ServiceProvider);
            levelManagerContent = new ContentManager(Content.ServiceProvider);
            levelEditorContent = new ContentManager(Content.ServiceProvider);
            
            Content.RootDirectory = "Content";
            mainMenuContent.RootDirectory = "Content";
            levelManagerContent.RootDirectory = "Content";
            levelEditorContent.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //_graphics.PreferredBackBufferWidth = 700;  // set this value to the desired width of your window
            //_graphics.PreferredBackBufferHeight = 700;   // set this value to the desired height of your window
            
            
            graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;

            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;

            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D buttonTexture = new Texture2D(GraphicsDevice, 1, 1);

            mainMenu = new Menu(mainMenuContent, spriteBatch, graphics, GraphicsDevice);
            levelManager = new LevelManager(levelManagerContent, spriteBatch, graphics, GraphicsDevice);
            levelEditor = new LevelEditor(levelEditorContent, spriteBatch, graphics, GraphicsDevice);


            switch(gameState) {
                    case (int) gameStates.mainMenu:
                        mainMenu.LoadContent();
                        break;
                    case (int) gameStates.levelManager:
                        levelManager.LoadContent();
                        break;
                    case (int) gameStates.levelEditor:
                        levelEditor.LoadContent();
                        break;
                        
                }

            //test code ##$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            

            // TODO: Add your update logic here
            switch(gameState) {
                case (int) gameStates.mainMenu:
                    gameState = mainMenu.Update();
                    break;
                case (int) gameStates.levelManager:
                    gameState = levelManager.Update();
                    break;
                case (int) gameStates.levelEditor:
                    gameState = levelEditor.Update();
                    break;
                
            }       

            if(gameState != prevGameState) {
                switch(prevGameState) {
                    case (int) gameStates.mainMenu:
                        mainMenuContent.Unload();
                        break;
                    case (int) gameStates.levelManager:
                        levelManagerContent.Unload();
                        break;
                    case (int) gameStates.levelEditor:
                        levelEditorContent.Unload();
                        break;

                }

                switch(gameState) {
                    case (int) gameStates.mainMenu:
                        mainMenu.LoadContent();
                        break;
                    case (int) gameStates.levelManager:
                        levelManager.LoadContent();
                        break;
                    case (int) gameStates.levelEditor:
                        levelEditor.LoadContent();
                        break;
                        
                }
            }     

            prevGameState = gameState;
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
                case (int) gameStates.levelEditor:
                    levelEditor.Draw();
                    break;
            }            

            
            base.Draw(gameTime);
        }

        

        
    }
}
