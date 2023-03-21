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

    public void Follow(Player target)
    {
      var position = Matrix.CreateTranslation(
        -target.posX - (target.width / 2),
        -target.posY - (target.height / 2),
        0);

      var offset = Matrix.CreateTranslation(
          Futhark_Game.screenWidth / 2,
          Futhark_Game.screenHeight / 2,
          0);

      Transform = position * offset;
    }
  }
}