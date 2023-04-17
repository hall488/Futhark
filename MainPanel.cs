using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class MainPanel : Panel{ 

        Camera_LE camera;

        public MainPanel(RenderTarget2D renderTarget, Rectangle rect, ContentManager content, Camera_LE camera) : base(renderTarget, rect, content) {   
            this.camera = camera;
        }

        public override void Update(MouseState mouseState) {
            base.Update(mouseState);

            var vector = mousePos.ToVector2();
                var transVector = Vector2.Transform(vector, Matrix.Invert(camera.Transform));
                mousePos = transVector.ToPoint();

        }

        public override void Draw () {
            
        }

        
    }
    
    
}