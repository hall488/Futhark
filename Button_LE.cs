using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Button_LE{ 

        protected Texture2D texture;
        protected Rectangle rect;

        public Button_LE(Texture2D texture, int ratio) {       
            this.texture = texture;  
        }

        public virtual void Update(Point mousePos) {
            if(rect.Contains(mousePos) && InputUtil.SingleLeftClick()) {
                    OnClick();
                }
        }

        public virtual void Draw (SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rect, Color.White);
        }

        public virtual void OnClick(){

        }

        
    }
    
    
}