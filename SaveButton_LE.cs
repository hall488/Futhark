using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class SaveButton_LE : Button_LE{ 


        public SaveButton_LE(Texture2D texture, int ratio) : base(texture, ratio) {         
            rect =  new Rectangle(2 * ratio, 2 * ratio, 8 * ratio, 4 * ratio);
        }

        public override void Update(Point mousePos) {
            base.Update(mousePos);
        }

        public override void Draw (SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }

        public override void OnClick() {
            //SaveToBMP
        }

        
    }
    
    
}