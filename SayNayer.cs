using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class SayNayer : Entity{ 

        

        public Runestone.Rune toCast;

        public Runestone.Rune disabledRune;
        Texture2D[] runeTextures;

        protected TimeSpan castTimerZero;

        protected TimeSpan castAnimationTimerZero;
        int moveCounter = 0;


        public SayNayer(int health, Point pos, int velocity, Player player, Texture2D textures, Texture2D[] runeTextures)  : base(health, pos, velocity, player, textures){
            toCast = Runestone.Rune.None;
            disabledRune = Runestone.Rune.None;
            this.runeTextures = runeTextures;
        }

        public override bool Update(GameTime gameTime, Rectangle[,] collidable) {

            

            return base.Update(gameTime, collidable);            

            //Console.WriteLine(currentAS);
        }

        public Runestone.Rune getDisabledRune() {
            return disabledRune;
        }

        public override void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
            
            if(toCast != Runestone.Rune.None) {
                var castAnimationTimer = gameTime.TotalGameTime - castAnimationTimerZero;

                if(castAnimationTimer > TimeSpan.FromSeconds(1)) {
                    disabledRune = toCast;
                    toCast = Runestone.Rune.None;
                    castAnimationTimerZero = gameTime.TotalGameTime;
                } else {
                    
                    double scale = castAnimationTimer.TotalMilliseconds / 1000;
                    var rTexture = runeTextures[(int)toCast];
                    var width = rTexture.Width;
                    var height = rTexture.Height;
                    var destinationRectangle = new Rectangle((int)posX, (int)posY, (int)(width * 8 * scale), (int)(height * 8 * scale));
                    //spriteBatch.Draw (rTexture, destinationRectangle, Color.White);
                    spriteBatch.Draw (rTexture, destinationRectangle, null, Color.Gold, 0, new Vector2(width / 2, 0), SpriteEffects.None, 0f);
                }


            }


            
            
        }

        protected override ActionState combatAS(GameTime gameTime) {

            if(resetMovementTimer) {
                movementTimerZero = gameTime.TotalGameTime;
                resetMovementTimer = false;
            }

            var movementTimer = gameTime.TotalGameTime - movementTimerZero;
            //Console.WriteLine("mt {0}", movementTimer);


            if(movementTimer > TimeSpan.FromSeconds(1)) {
                resetMovementTimer = true;
                ++ moveCounter;

                var moveX = posX < player.posX ? Movement.left : Movement.right;
                var moveY = posY < player.posY ? Movement.up : Movement.down;

                Movement[] values = {moveX, moveY};
                Random random = new Random();
                if(direction != Movement.pause) {
                    direction = Movement.pause;
                } else {
                    direction = (Movement)values.GetValue(random.Next(values.Length));
                }
            } else {
                switch(direction) {
                    case Movement.up:
                        unitY -= 1;
                        break;
                    case Movement.right:
                        unitX += 1;
                        break;
                    case Movement.down:
                        unitY += 1;
                        break;
                    case Movement.left:
                        unitX -= 1;
                        break;
                    case Movement.pause:
                        break;
                }
            }

            if(moveCounter == 5) {
                moveCounter = 0;
                castTimerZero = gameTime.TotalGameTime;
                castAnimationTimerZero = gameTime.TotalGameTime;
                Array values = Enum.GetValues(typeof(Runestone.Rune));
                Random random = new Random();
                toCast = (Runestone.Rune)values.GetValue(random.Next(values.Length));
                return ActionState.attack;
            }

            

            return ActionState.combat;
        }

        protected override ActionState attackAS(GameTime gameTime) {
            
            var castTimer = gameTime.TotalGameTime - castTimerZero;

            currentAnimation = castAnimation;

            if(castTimer > TimeSpan.FromSeconds(1)) {               
                
                return ActionState.combat;
            }



            return ActionState.attack;
        }

        
    }
    
    
}