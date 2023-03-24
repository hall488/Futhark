using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Runestone {

        Texture2D[] aettsTextures;
        private Dictionary<Keys, bool> pressedKeys;

        public Runestone(Texture2D[] _aettsTextures) {

            aettsTextures = _aettsTextures;
            pressedKeys = new Dictionary<Keys, bool>();
            
        }

        public void Update(KeyboardState keyboardState) {   
            
            Console.WriteLine(Keys.Space);

        }

        public void Draw() {

        }
    }
}