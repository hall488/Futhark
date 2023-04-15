using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class InputUtil{ 

        static MouseState currentButtonState;
        static MouseState previousButtonState; 

        public InputUtil() {
            currentButtonState = Mouse.GetState();            
        }

        public static MouseState GetButtonState() {
            previousButtonState = currentButtonState;
            currentButtonState = Mouse.GetState();
            return currentButtonState;
        }
        public static bool SingleLeftClick() {
            return currentButtonState.LeftButton == ButtonState.Pressed && previousButtonState.LeftButton != ButtonState.Pressed;
        }

        public static bool SingleRightClick() {
            return currentButtonState.RightButton == ButtonState.Pressed && previousButtonState.RightButton != ButtonState.Pressed;
        }
           

        
    }
    
    
}