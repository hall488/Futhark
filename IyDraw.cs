using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public interface IyDraw : IComparable<IyDraw> {
        double yPosition();
        double xPosition();
        float rotation();

        AnimatedSprite animation();
    }
    
}