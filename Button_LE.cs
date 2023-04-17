using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Button_LE{ 

        protected Texture2D texture;

        public Button_LE(Texture2D texture) {       
            this.texture = texture;  
        }

        public virtual void Update() {

        }

        public virtual void Draw () {
            
        }

        
    }
    
    
}