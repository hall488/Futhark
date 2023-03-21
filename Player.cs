using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace Futhark {

    public class Player {

        public int posX;
        public int posY;
        int velX;
        int velY;
        public int width = 128;
        public int height = 192;

        Texture2D texture;

        AnimatedSprite upAnimation;
        AnimatedSprite downAnimation;
        AnimatedSprite leftAnimation;
        AnimatedSprite rightAnimation;
        AnimatedSprite currentAnimation;

        bool[] pressedKeys = {false, false, false, false};
        bool[] previousKeys = {false, false, false, false};

        Tilemap activeTiles;

        public Player(Texture2D _texture, int x, int y, Tilemap _activeTiles) {
            texture = _texture;
            posX = x;
            posY = y;
            activeTiles = _activeTiles;

            velX = 0;
            velY =0;
            
            downAnimation = new AnimatedSprite(texture, 4, 4, 0);
            upAnimation = new AnimatedSprite(texture, 4, 4, 1);
            rightAnimation = new AnimatedSprite(texture, 4, 4, 2);
            leftAnimation = new AnimatedSprite(texture, 4, 4, 3);
            
            
            

            currentAnimation = downAnimation;
        }

        public void Update() {

            var keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.W)) {
                pressedKeys[0] = true;
                velY -= 1;
            } else {
                pressedKeys[0] = false;
            }
            if(keyboardState.IsKeyDown(Keys.A)) {
                pressedKeys[1] = true;
                velX -= 1;
            } else {
                pressedKeys[1] = false;
            }
            if(keyboardState.IsKeyDown(Keys.S)) {
                pressedKeys[2] = true;
                velY += 1;
            } else {
                pressedKeys[2] = false;
            }
            if(keyboardState.IsKeyDown(Keys.D)) {
                pressedKeys[3] = true;
                velX += 1;
            } else {
                pressedKeys[3] = false;
            }


            if(velY < 0) {
                currentAnimation = upAnimation;
                currentAnimation.playAnimation();
            } else if (velY > 0) {
                currentAnimation = downAnimation;
                currentAnimation.playAnimation();
            } else if (velX > 0) {
                currentAnimation = rightAnimation;
                currentAnimation.playAnimation();
            } else if (velX < 0) {
                currentAnimation = leftAnimation;
                currentAnimation.playAnimation();
            } else {
                currentAnimation.stopAnimation();
            }

            
            int lowerCordX = (int)Math.Round((double)(posX - width)/width, 0);
            int upperCordX = (int)Math.Round((double)(posX + width)/width, 0);
            //using width here because the sprite height is taller than what the bounding box should be
            //the bounding box is one tile
            int lowerCordY = (int)Math.Round((double)(posY - width)/width, 0);
            int upperCordY = (int)Math.Round((double)(posY + width)/width, 0);

            for(int i = lowerCordX; i < upperCordX; i++) {
                for(int j = lowerCordY; j < upperCordY; j++) {
                    Debug.Write(lowerCordY);
                    Debug.Write(":");
                    Debug.WriteLine(upperCordY);
                    Tile t = activeTiles.tilemap[i, j];
                    if(t.solid) {
                        int testPosX = posX + velX*2;
                        int testPosY = posY + velY*2;
                                             

                        Rectangle testRect = new Rectangle(testPosX, testPosY, width, width);
                        
                        if(testRect.Intersects(t.tileRect)) {
                            velX = 0;
                            velY = 0;
                        }
                    }
                }
            }

            posX += velX*2;
            posY += velY*2;

            pressedKeys.CopyTo(previousKeys, 0);
            velX = 0;
            velY = 0;
            
            currentAnimation.Update();
        }

        public void Draw(SpriteBatch spriteBatch) {
            currentAnimation.Draw(spriteBatch, posX, posY);
        }
    }
}