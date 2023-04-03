using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Player {

        public int posX;
        public int posY;
        double unitX;
        double unitY;
        public int vel;
        public int width = 128;
        public int height = 192;

        Texture2D texture;

        AnimatedSprite upAnimation;
        AnimatedSprite downAnimation;
        AnimatedSprite leftAnimation;
        AnimatedSprite rightAnimation;
        AnimatedSprite currentAnimation;

        Rectangle colRectX;
        Rectangle colRectY;

        Texture2D colRectTexture;

        bool[] pressedKeys = {false, false, false, false};
        bool[] previousKeys = {false, false, false, false};

        Tilemap activeTiles;

        Runestone runestone;


        public Player(Game_Constants gConstants, Texture2D _texture, Texture2D[] _aettsTextures, int x, int y, Tilemap _activeTiles, Texture2D _colRectTexture) {
            texture = _texture;

            

            posX = x;
            posY = y;
            activeTiles = _activeTiles;

            unitX = 0;
            unitY = 0;

            vel = 3;
            
            downAnimation = new AnimatedSprite(texture, 4, 4, 0);
            upAnimation = new AnimatedSprite(texture, 4, 4, 1);
            rightAnimation = new AnimatedSprite(texture, 4, 4, 2);
            leftAnimation = new AnimatedSprite(texture, 4, 4, 3);
            
            
            colRectTexture = _colRectTexture;
            colRectTexture.SetData(new[] {new Color(255, 0, 0, 128)});

            currentAnimation = downAnimation;

            runestone = new Runestone(this, _aettsTextures, gConstants);
        }

        public void Update() {

            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            runestone.Update(keyboardState, mouseState);

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
            

            int testPosX = posX + (int)(unitX*vel);
            int testPosY = (posY+height-width) + (int)(unitY*vel);
                                             
                        
            colRectX = new Rectangle(testPosX+10, posY+height-width+20, width-20, width-20);
            colRectY = new Rectangle(posX+10, testPosY+20, width-20, width-20);

            
            int lowerCordX = (int)Math.Round((double)(posX - width)/width, 0);
            int upperCordX = (int)Math.Round((double)(posX + 2*width)/width, 0);
            //using width here because the sprite height is taller than what the bounding box should be
            //the bounding box is one tile
            int lowerCordY = (int)Math.Round((double)((posY+height-width) - width)/width, 0);
            int upperCordY = (int)Math.Round((double)((posY+height-width) + 2*width)/width, 0);

            for(int i = lowerCordX; i < upperCordX; i++) {
                for(int j = lowerCordY; j < upperCordY; j++) {
                    
                    Tile t = activeTiles.tilemap[i, j];
                    // Console.Write(lowerCordX);
                    // Console.Write(" : ");
                    // Console.WriteLine(upperCordX);
                    if(t.solid) {                        
                        if(colRectX.Intersects(t.tileRect)) {
                            unitX = 0;                            
                        }
                        if(colRectY.Intersects(t.tileRect)) {
                            unitY = 0;
                        }
                    }
                }
            }

            posX += (int)(unitX*vel);
            posY += (int)(unitY*vel);

            pressedKeys.CopyTo(previousKeys, 0);
            unitX = 0;
            unitY = 0;
            
            currentAnimation.Update();
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(colRectTexture, colRectY, Color.White);
            currentAnimation.Draw(spriteBatch, posX, posY);
            runestone.Draw(spriteBatch, posX, posY);
        }
    }
}