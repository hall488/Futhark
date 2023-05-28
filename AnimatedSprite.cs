using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
 
namespace Futhark
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        private int currentFrame;
        private int totalFrames;

        private int RowNumber;

        public int x;

        private bool play;

        public int frameWidth;
        public int frameHeight;
 
        public AnimatedSprite(Texture2D texture, int rows, int columns, int rowNumber, bool play)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = columns;
            RowNumber = rowNumber;

            frameWidth = texture.Width / columns;
            frameHeight = texture.Height / rows;
            
            this.play = play;
        }
 
        public void Update()
        {
            //currentFrame = stop ? 0 : currentFrame;
            //x = stop ? 0 : x;
            if(play) {
                if(x > 10) {
                    currentFrame++;
                    if (currentFrame == totalFrames)
                        currentFrame = 0;
                    x = 0;
                }
                x++;
            }
        }
 
        public void Draw(SpriteBatch spriteBatch, int x, int y, float rot)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = RowNumber;
            int column = currentFrame % Columns;
 
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(x, y, width*8, height*8);
 
            //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.Draw (Texture, destinationRectangle, sourceRectangle, Color.White, rot, new Vector2(width / 2, height / 2), SpriteEffects.None, 0f);
           
        }

        public void playAnimation() {
            play = true;
        }

        public void stopAnimation() {
            currentFrame = 0;
            x = 0;
            play = false;
        }
    }
}