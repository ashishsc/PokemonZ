using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class Enemy : AnimatedSprite
    {
        public Enemy(Texture2D texture, int rows, int columns, ContentManager content, Vector2 location, int health)
            : base(texture, rows, columns, content, location, health)
        {

        }

        public void UpdateMovement(Viewport gd)
        {
            //Pick a direction
            if (this.Location.X > gd.Width / 2)
                this.Location.X -= 1f;
            else
                this.Location.X += 1f;
        }
    }
}
