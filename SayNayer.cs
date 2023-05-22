using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class SayNayer : Entity{ 

        enum Rune {
            Fehu, Hagalaz, Tiwaz,
            Uruz, Nauthiz, Berkana,
            Thurisaz, Isa, Ehwaz,
            Ansuz, Jera, Mannaz,
            Raido, Eihwaz, Laguz,
            Kenaz, Perthro, Ingwaz,
            Gebo, Algiz, Dagaz,
            Wunjo, Sowilo, Othala,
            None
        }

        Rune toCast;
        Texture2D[] runeTextures;

        protected TimeSpan castTimerZero;

        protected TimeSpan castAnimationTimerZero;
        int moveCounter = 0;


        public SayNayer(int health, Point pos, int velocity, Player player, Texture2D textures, Texture2D[] runeTextures)  : base(health, pos, velocity, player, textures){
            toCast = Rune.None;
            this.runeTextures = runeTextures;
        }

        public override void Update(GameTime gameTime, Rectangle[,] collidable) {
            base.Update(gameTime, collidable);

            //Console.WriteLine(currentAS);
        }

        public override void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
            
            if(toCast != Rune.None) {
                var castAnimationTimer = gameTime.TotalGameTime - castAnimationTimerZero;

                if(castAnimationTimer > TimeSpan.FromSeconds(1)) {
                    toCast = Rune.None;
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
                Array values = Enum.GetValues(typeof(Rune));
                Random random = new Random();
                toCast = (Rune)values.GetValue(random.Next(values.Length));
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