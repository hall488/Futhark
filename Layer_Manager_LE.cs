using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class Layer_Manager_LE{ 

        public Layer_LE background;

        public Layer_LE ground;
        public Layer_LE onground;

        public Layer_LE overground;

        public Layer_Manager_LE() {
             background = new Layer_LE();
             ground = new Layer_LE();
             onground = new Layer_LE();
             overground = new Layer_LE();

        }

        public void Update() {

        }

        public void Draw (SpriteBatch spriteBatch) {
            
        }

        public void AddSingleItem(Item_LE item) {
            if(item.identifier == "background")
                background.AddPlaced(item);
            else if(item.identifier == "ground")
                ground.AddPlaced(item);
            else if(item.identifier == "onground")
                onground.AddPlaced(item);
            else if(item.identifier == "overground");
                overground.AddPlaced(item);
                
                
        }

        private void AddCollection(Item_LE[,] items) {
            foreach(var i in items)
                AddSingleItem(i);
        }

        
    }
    
    
}