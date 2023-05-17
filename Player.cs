using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Linq;
using System.Collections.Generic;
using TiledCS;

namespace Futhark {

    public class Player : IyDraw {

        public double posX;
        public double posY;

        public double prevPosX;
        public double prevPosY;

        double IyDraw.yPosition() => posY;
        double IyDraw.xPosition() => posX;

        float IyDraw.rotation() => 0;
        double unitX;
        double unitY;
        public double vel;
        public int width = 128;
        public int height = 192;

        public int collisionHeight = 64;

        Texture2D texture;

        AnimatedSprite upAnimation;
        AnimatedSprite downAnimation;
        AnimatedSprite leftAnimation;
        AnimatedSprite rightAnimation;
        AnimatedSprite currentAnimation;

        AnimatedSprite IyDraw.animation() => currentAnimation;

        Rectangle colRectB;
        Rectangle colRectX;
        Rectangle colRectY;

        Texture2D colRectTexture;

        bool[] pressedKeys = {false, false, false, false};
        bool[] previousKeys = {false, false, false, false};

        //Tilemap activeTiles;
        //Layer_LE collidable;

        Rectangle[,] collidable;

        private Dictionary<string, Texture2D> spellTextures;

        public List<string> spellQueue;
        List<Fireball> fireballs;

        public Camera camera;

        List<IyDraw> drawQueue;

        Dictionary<Rectangle, string> doorDict;

        public string currentMap;
        

        public int CompareTo(IyDraw other)
        {
            // implement your custom comparison here...

            return posY.CompareTo(other.yPosition()); // e.g.
        }


        public Player(Camera camera, Game_Dicts gDicts, Texture2D _texture, Texture2D[] _aettsTextures, int x, int y, LevelManager.Direction facing, Rectangle[,] collidable, Dictionary<Rectangle, string> doorDict, Texture2D _colRectTexture) {
            
            

            drawQueue = new List<IyDraw>();

            this.camera = camera;
            
            texture = _texture;

            this.collidable = collidable;
            this.doorDict = doorDict;

            spellTextures = gDicts.spellTextures;

            spellQueue = new List<string>();



            
            
            downAnimation = new AnimatedSprite(texture, 4, 4, 0, false);
            upAnimation = new AnimatedSprite(texture, 4, 4, 1, false);
            rightAnimation = new AnimatedSprite(texture, 4, 4, 2, false);
            leftAnimation = new AnimatedSprite(texture, 4, 4, 3, false);

            var spawnOffset = Point.Zero;
            switch((int) facing) {
                case (int) LevelManager.Direction.Up:
                    currentAnimation = upAnimation;
                    spawnOffset.Y = 64-128;
                    break;
                case (int) LevelManager.Direction.Right:
                    currentAnimation = rightAnimation;
                    break;
                case (int) LevelManager.Direction.Down:
                    currentAnimation = downAnimation;
                    break;
                case (int) LevelManager.Direction.Left:
                    currentAnimation = leftAnimation;
                    break;
            }

            posX = x + spawnOffset.X;
            posY = y + spawnOffset.Y;
            prevPosX = posX;
            prevPosY = posY;

            //activeTiles = _activeTiles;

            unitX = 0;
            unitY = 0;

            vel = 4;        
            
            colRectTexture = _colRectTexture;
            colRectTexture.SetData(new[] {new Color(255, 0, 0, 128)});

            

            //runestone = new Runestone(this, _aettsTextures, gConstants);

            fireballs = new List<Fireball>(); 
        }

        public string Update() {

            var keyboardState = Keyboard.GetState();
            
            

            List<Fireball> fireballToRemove = new List<Fireball>();

            foreach(var f in fireballs) {

                if(f.Update(collidable)) {
                    fireballToRemove.Add(f);
                }
            }

            foreach(var f in fireballToRemove) {
                fireballs.Remove(f);
            }

            fireballToRemove.Clear();

            if(keyboardState.IsKeyDown(Keys.W)) {
                pressedKeys[0] = true;
                unitY -= 1;
            } else {
                pressedKeys[0] = false;
            }
            if(keyboardState.IsKeyDown(Keys.A)) {
                pressedKeys[1] = true;
                unitX -= 1;
            } else {
                pressedKeys[1] = false;
            }
            if(keyboardState.IsKeyDown(Keys.S)) {
                pressedKeys[2] = true;
                unitY += 1;
            } else {
                pressedKeys[2] = false;
            }
            if(keyboardState.IsKeyDown(Keys.D)) {
                pressedKeys[3] = true;
                unitX += 1;
            } else {
                pressedKeys[3] = false;
            }  

            //if()          

            if(unitY < 0) {
                currentAnimation = upAnimation;
                currentAnimation.playAnimation();
            } else if (unitY > 0) {
                currentAnimation = downAnimation;
                currentAnimation.playAnimation();
            } else if (unitX > 0) {
                currentAnimation = rightAnimation;
                currentAnimation.playAnimation();
            } else if (unitX < 0) {
                currentAnimation = leftAnimation;
                currentAnimation.playAnimation();
            } else {
                currentAnimation.stopAnimation();
            }

            

            if(!(unitX == 0 && unitY == 0)) {

                double mag = Math.Sqrt(unitX*unitX + unitY*unitY);

                unitX = unitX / mag;
                unitY = unitY / mag;

            }
            

            double testPosX = posX + (unitX*vel);
            double testPosY = posY + (unitY*vel);
                                             
                        
            //colRectX = new Rectangle(testPosX+10, posY+height-width+20, width-20, width-20);
            //colRectY = new Rectangle(posX+10, testPosY+20, width-20, width-20);

            colRectB = new Rectangle((int)posX - width / 2 + 8, (int)posY + height/2 - width/2, width - 16, width/2);
            colRectX = new Rectangle((int)testPosX - width / 2 + 8, (int)posY + height/2 - width/2, width - 16, width/2);
            colRectY = new Rectangle((int)posX - width / 2 + 8, (int)testPosY + height/2 - width/2, width - 16, width/2);

            
            int lowerCordX = (int)Math.Round((double)(posX - 2*width)/width, 0);
            int upperCordX = (int)Math.Round((double)(posX + 2*width)/width, 0);
            //using width here because the sprite height is taller than what the bounding box should be
            //the bounding box is one tile
            int lowerCordY = (int)Math.Round((double)((posY+height/2-width) - 2*width)/width, 0);
            int upperCordY = (int)Math.Round((double)((posY+height/2-width) + 2*width)/width, 0);

            lowerCordX = lowerCordX < 0 ? 0 : lowerCordX;
            lowerCordY = lowerCordY < 0 ? 0 : lowerCordY;
            upperCordX = upperCordX > collidable.GetLength(0) ? collidable.GetLength(0) : upperCordX;
            upperCordY = upperCordY > collidable.GetLength(1) ? collidable.GetLength(1) : upperCordY;

            for(int i = lowerCordX; i < upperCordX; i++) {
                for(int j = lowerCordY; j < upperCordY; j++) {
                    var r = collidable[i,j];
                    // Console.Write(posX + ",");
                    // Console.WriteLine(posY);
                    // Console.Write(lowerCordX);
                    // Console.Write(" : ");
                    // Console.WriteLine(upperCordX);
                    if(r != Rectangle.Empty) {                        
                        if(colRectX.Intersects(r)) {
                            unitX = 0;
                            Console.WriteLine("x intersect");                            
                        }
                        if(colRectY.Intersects(r)) {
                            unitY = 0;
                            Console.WriteLine("y intersect");
                        }
                    }
                }
            }

            prevPosX = posX;
            prevPosY = posY;

            posX += (unitX*vel);
            posY += (unitY*vel);

            pressedKeys.CopyTo(previousKeys, 0);
            unitX = 0;
            unitY = 0;
            
            currentAnimation.Update();

            var mu = playerToMouse();


            if(spellQueue.Count != 0 && spellQueue[0] == "Fireball") {
                fireballs.Add(new Fireball(new Point((int)posX, (int)posY), 3, mu, spellTextures["fireball"]));
                spellQueue.RemoveAt(0);
            }

            foreach((var r, var name) in doorDict) {
                if(colRectB.Intersects(r)) {
                    Console.WriteLine("enter door");
                    return name;
                }
            }

            return currentMap;
        }

        public void Draw(SpriteBatch spriteBatch) {
            //spriteBatch.Draw(colRectTexture, colRectY, Color.White);
            //spriteBatch.Draw(colRectTexture, colRectX, Color.Blue);


            drawQueue.Add(this);
            foreach(var f in fireballs) {
                drawQueue.Add(f);
            }

            drawQueue.Sort();

            foreach(var e in drawQueue) {
                e.animation().Draw(spriteBatch, (int)e.xPosition(), (int)e.yPosition(), e.rotation());
            }

            drawQueue.Clear();

            //currentAnimation.Draw(spriteBatch, posX, posY, 0f);

            
            

            //drawCollisionBoundaries(spriteBatch);

            
        }

        public void drawCollisionBoundaries(SpriteBatch spriteBatch){
            int lowerCordX = (int)Math.Round((double)(posX - 2*width)/width, 0);
            int upperCordX = (int)Math.Round((double)(posX + 2*width)/width, 0);
            //using width here because the sprite height is taller than what the bounding box should be
            //the bounding box is one tile
            int lowerCordY = (int)Math.Round((double)((posY+height/2-width) - 2*width)/width, 0);
            int upperCordY = (int)Math.Round((double)((posY+height/2-width) + 2*width)/width, 0);

            lowerCordX = lowerCordX < 0 ? 0 : lowerCordX;
            lowerCordY = lowerCordY < 0 ? 0 : lowerCordY;
            upperCordX = upperCordX > collidable.GetLength(0) ? collidable.GetLength(0) : upperCordX;
            upperCordY = upperCordY > collidable.GetLength(1) ? collidable.GetLength(1) : upperCordY;


            for(int i = lowerCordX; i < upperCordX; i++) {
                for(int j = lowerCordY; j < upperCordY; j++) {
                    var r = collidable[i,j];
                    spriteBatch.Draw(colRectTexture, r, Color.Green);
                }
            }

            for(int i = 0; i < collidable.GetLength(0); i++) {
                for(int j = 0; j < collidable.GetLength(1); j++) {
                    if(collidable[i,j] != Rectangle.Empty)
                        spriteBatch.Draw(colRectTexture, collidable[i,j], Color.White);
                }
            }

            foreach((var r, var name) in doorDict) {
                spriteBatch.Draw(colRectTexture, r, Color.Teal);
            }
        }

        public Vector2 playerToMouse () {
            var mouseState = Mouse.GetState();

            var mousePos = camera.ScreenToWorldSpace(mouseState.Position.ToVector2() - new Vector2(LevelManager.sceneRect.X, LevelManager.sceneRect.Y));
            
            
            
            mousePos = new Vector2(mousePos.X - (int)posX, mousePos.Y - (int)posY);

            var mouseUnit = new Vector2(0,0);


            if(!(mousePos.X == 0 && mousePos.Y == 0)) {

                double mag = Math.Sqrt(mousePos.X*mousePos.X + mousePos.Y*mousePos.Y);

                mouseUnit.X = (float)(mousePos.X / mag);
                mouseUnit.Y = (float)(mousePos.Y / mag);

            }

            return mouseUnit;

        }

        
    }
}