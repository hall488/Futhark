using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class InputUtil{ 

        static ButtonState currentButtonState;
        static ButtonState previousButtonState; 

        public InputUtil() {
            currentButtonState = Mouse.GetState().LeftButton;            
        }

        public static ButtonState GetButtonState() {
            previousButtonState = currentButtonState;
            currentButtonState = Mouse.GetState().LeftButton;
            return currentButtonState;
        }
        public static bool SingleMouseClick() {
            return currentButtonState == ButtonState.Pressed && previousButtonState != ButtonState.Pressed;
        }
           

        
    }
    
    
}