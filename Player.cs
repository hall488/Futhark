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

        Rectangle colRectX;
        Rectangle colRectY;

        Texture2D colRectTexture;

        bool[] pressedKeys = {false, false, false, false};
        bool[] previousKeys = {false, false, false, false};

        Tilemap activeTiles;

        public Player(Texture2D _texture, int x, int y, Tilemap _activeTiles, Texture2D _colRectTexture) {
            texture = _texture;
            posX = x;
            posY = y;
            activeTiles = _activeTiles;

            velX = 0;
            velY = 0;
            
            downAnimation = new AnimatedSprite(texture, 4, 4, 0);
            upAnimation = new AnimatedSprite(texture, 4, 4, 1);
            rightAnimation = new AnimatedSprite(texture, 4, 4, 2);
            leftAnimation = new AnimatedSprite(texture, 4, 4, 3);
            
            
            colRectTexture = _colRectTexture;
            colRectTexture.SetData(new[] {new Color(255, 0, 0, 128)});

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

            int testPosX = posX + velX*2;
            int testPosY = (posY+height-width) + velY*2;
                                             
                        
            colRectX = new Rectangle(testPosX, posY+height-width, width, width);
            colRectY = new Rectangle(posX, testPosY, width, width);

            
            int lowerCordX = (int)Math.Round((double)(posX - width)/width, 0);
            int upperCordX = (int)Math.Round((double)(posX + 2*width)/width, 0);
            //using width here because the sprite height is taller than what the bounding box should be
            //the bounding box is one tile
            int lowerCordY = (int)Math.Round((double)((posY+height-width) - width)/width, 0);
            int upperCordY = (int)Math.Round((double)((posY+height-width) + 2*width)/width, 0);

            for(int i = lowerCordX; i < upperCordX; i++) {
                for(int j = lowerCordY; j < upperCordY; j++) {
                    
                    Tile t = activeTiles.tilemap[i, j];
                    Console.Write(lowerCordX);
                    Console.Write(" : ");
                    Console.WriteLine(upperCordX);
                    if(t.solid) {                        
                        if(colRectX.Intersects(t.tileRect)) {
                            velX = 0;                            
                        }
                        if(colRectY.Intersects(t.tileRect)) {
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
            spriteBatch.Draw(colRectTexture, colRectY, Color.White);
            currentAnimation.Draw(spriteBatch, posX, posY);

        }
    }
}