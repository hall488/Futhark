using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public abstract class Entity : IyDraw{ 



        int health;
        protected double posX;
        protected double posY;

        public int CompareTo(IyDraw other)
        {
            // implement your custom comparison here...

            return posY.CompareTo(other.yPosition()); // e.g.
        }

        double IyDraw.yPosition() => posY;
        double IyDraw.xPosition() => posX;

        float IyDraw.rotation() => 0;

        AnimatedSprite upAnimation;
        AnimatedSprite downAnimation;
        AnimatedSprite leftAnimation;
        AnimatedSprite rightAnimation;
        protected AnimatedSprite castAnimation;
        protected AnimatedSprite currentAnimation;

        protected double unitX;
        protected double unitY;

        AnimatedSprite IyDraw.animation() => currentAnimation;
        protected int vel;

        

        protected enum ActionState {
            idle,
            attack,
            follow,
            patrol,
            combat
        }

        protected enum Movement {
            up,
            right,
            down,
            left,
            pause
        }

        protected ActionState currentAS;

        protected Player player;

        protected TimeSpan movementTimerZero;

        protected Movement direction;


        public Entity(int health, Point pos, int vel, Player player, Texture2D textures) {
            this.health = health;
            this.posX = pos.X;
            this.posY = pos.Y;
            this.vel = vel;
            this.player = player;

            downAnimation = new AnimatedSprite(textures, 5, 4, 0, false);
            upAnimation = new AnimatedSprite(textures, 5, 4, 1, false);
            rightAnimation = new AnimatedSprite(textures, 5, 4, 2, false);
            leftAnimation = new AnimatedSprite(textures, 5, 4, 3, false);
            castAnimation = new AnimatedSprite(textures, 5, 4, 4, false);

            currentAnimation = downAnimation;

            this.currentAS = ActionState.patrol;

            movementTimerZero = TimeSpan.Zero;
            
        }

        public virtual void Update(GameTime gameTime) {
            currentAS = handleAS(gameTime);

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
            } else if(currentAnimation != castAnimation){
                currentAnimation.stopAnimation();
            } else {
                currentAnimation.playAnimation();
            }

            

            if(!(unitX == 0 && unitY == 0)) {

                double mag = Math.Sqrt(unitX*unitX + unitY*unitY);

                unitX = unitX / mag;
                unitY = unitY / mag;

            }

            posX += unitX * vel;
            posY += unitY * vel;

            unitX = 0;
            unitY = 0;
            currentAnimation.Update();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            currentAnimation.Draw(spriteBatch, (int)posX, (int)posY, 0);
        }

        public Point GetEntityPos(){
            return new Point((int)posX, (int) posY);
        }

        private ActionState handleAS(GameTime gameTime) {
            switch(this.currentAS) {
                case ActionState.idle:
                    return idleAS();
                case ActionState.attack:
                    return attackAS(gameTime);
                case ActionState.follow:
                    return followAS();
                case ActionState.patrol:
                    return patrolAS(gameTime);
                case ActionState.combat:
                    return combatAS(gameTime);
                default:
                    throw new Exception("No action state found");
            }
        }

        private ActionState idleAS() {
            

            return ActionState.idle;            
        }

        private ActionState patrolAS(GameTime gameTime) {
            var dt = gameTime.TotalGameTime - movementTimerZero;
            //Console.WriteLine("gt {0}, zt {1}", gameTime.TotalGameTime, zeroTime);


            if(dt > TimeSpan.FromSeconds(1)) {
                movementTimerZero = gameTime.TotalGameTime;
                Array values = Enum.GetValues(typeof(Movement));
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

            if(Util.tilesTo(new Point((int)player.posX, (int)player.posY), this.GetEntityPos()) < 5) {
                Console.WriteLine("player is within 5 tiles");
                return ActionState.attack;
            }

            return ActionState.patrol;
        }

        protected virtual ActionState combatAS(GameTime gameTime) {

            return ActionState.combat;
        }

        protected virtual ActionState attackAS(GameTime gameTime) {
            return ActionState.attack;
        }

        private ActionState followAS() {


            unitX += player.posX - player.prevPosX;
            unitY += player.posY - player.prevPosY;

            return ActionState.follow;
        }

        
    }
    
    
}