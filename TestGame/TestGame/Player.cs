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
    public class Player : AnimatedSprite
    {
        public int health;
        public Player(Texture2D texture, int rows, int columns, ContentManager content, Vector2 location, int health)
            : base(texture, rows, columns, content, location, health)
        {
            this.health = health;
        }

        new public void checkCollision(GameObject other)
        {
            if (base.checkCollision(other))
            {
                health -= 10;
            }
        }
    }
}
