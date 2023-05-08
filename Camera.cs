using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futhark
{
  public class Camera
  {
    public Matrix Transform { get; private set; }

    private int screenWidth;
    private int screenHeight;

    public Camera(int screenWidth, int screenHeight) {
      this.screenWidth = screenWidth;
      this.screenHeight = screenHeight;
    }
    public void Follow(Player target)
    {
      var posX = (int)target.posX;
      var posY = (int)target.posY;

      var position = Matrix.CreateTranslation(
        // -posX - (target.width / 2),
        //-posY - (target.height / 2),
        -posX,
        -posY,
        0);

      var offset = Matrix.CreateTranslation(
          screenWidth / 2,
          screenHeight / 2,
          0);

      Transform = position * offset;
    }

    public Vector2 ScreenToWorldSpace(in Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(Transform);
            return Vector2.Transform(point, invertedMatrix);
        }
  }
}