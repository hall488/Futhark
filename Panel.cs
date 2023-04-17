using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Panel{ 

        protected RenderTarget2D renderTarget;
        protected Rectangle rect;
        protected Point mousePos;
        protected bool activePanel;


        public Panel(RenderTarget2D renderTarget, Rectangle rect, ContentManager content) {  
            this.renderTarget = renderTarget;
            this.rect = rect;         
        }

        public virtual void Update(MouseState mouseState) {
            if(rect.Contains(mouseState.Position)) {
                activePanel = true;
                mousePos.X = mouseState.Position.X - rect.X;
                mousePos.Y = mouseState.Position.Y - rect.Y;
            } else {
                activePanel = false;
                mousePos = Point.Zero;
            }
        }

        public virtual void Draw () {
            
        }

        
    }
    
    
}