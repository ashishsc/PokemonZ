using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestGame 
{
    public enum Direction { Left, Up, Down, Right, none}; 
    public class GameObject
    {
        public Texture2D texture;
        public Vector2 Location;
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)Location.X, (int)Location.Y, texture.Width, texture.Height); }
        }

        public GameObject(Texture2D texture, Vector2 location)
        {
            Location = location;
            this.texture = texture;
        }

        public Direction IsColliding(GameObject other)
        {
            /*if (BoundingBox.Right >= other.BoundingBox.Left && BoundingBox.Left < other.BoundingBox.Left && BoundingBox.Bottom > other.BoundingBox.Top) //this object colliding on left of other
                return Direction.Left;
            if (BoundingBox.Bottom >= other.BoundingBox.Top && ((BoundingBox.Right >= other.BoundingBox.Left && BoundingBox.Left <= other.BoundingBox.Left) || (BoundingBox.Left <= other.BoundingBox.Right && BoundingBox.Right >= other.BoundingBox.Right)))//collide on top
                return Direction.Up;
            if (BoundingBox.Top <= other.BoundingBox.Bottom && BoundingBox.Top > other.BoundingBox.Bottom)// collide right
                return Direction.Right;
            return Direction.none; //no collision*/
            if (BoundingBox.Intersects(other.BoundingBox))
                return Direction.Left;
            return Direction.none;
        }
    }
}
