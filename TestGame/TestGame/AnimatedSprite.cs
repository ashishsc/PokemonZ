using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame {
    public class AnimatedSprite : GameObject
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Direction facingDir;
        private int JumpSize = -15;
        private int WalkSpeed = 5;
        private int currentFrame, totalFrames;
        private int frametimeout = 10;
        //for jump
        bool isJumping;
        float jumpStartY, jumpspeed; //startY to tell us //where it lands, jumpspeed to see how fast it jumps

        public int YMomentum = 0;
        public int Gravity = 1;
        public bool inAir = false;
        
        new public Rectangle BoundingBox
        {
            get { return new Rectangle((int)Location.X, (int)Location.Y, texture.Width / Columns, texture.Height / Rows); }
        }

        public AnimatedSprite(Texture2D texture, int rows, int columns, ContentManager content, Vector2 location, int health): base(texture, location)
        {
            facingDir = Direction.Right;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;

            isJumping = false;//Init jumping to false
            jumpspeed = 0;//Default no speed
        }

        public void Update()
        {
            Location.Y += YMomentum;
            YMomentum += Gravity;
            /*if (isJumping)
            {
                Location.Y += jumpspeed;
                jumpspeed += 1;
                if (Location.Y == jumpStartY)
                {
                    isJumping = false;
                }
            }*/
        }

        public void Move(bool isLeft)
        {
            frametimeout--;
            if (frametimeout == 0)
            {
                currentFrame++;
                frametimeout = 10;
            }
            if (currentFrame == totalFrames)
                currentFrame = 1;

            if (isLeft) //left
            {
                facingDir = Direction.Left;
            }
            else
            {
                facingDir = Direction.Right;
            }
        }

        public void ResetAnimation()
        {
            currentFrame = 0;
        }

        public bool isNotJumping()
        {
            return !isJumping;
        }

        public void Jump(GameObject other)
        {
            //jumpStartY = Location.Y;
            //isJumping = true;
            //jumpspeed = JumpSize;
            if (!inAir)
            {
                Location.Y -= 10;
                YMomentum = -15;
                inAir = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = texture.Width / Columns;
            int height = texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, width, height);
            spriteBatch.Begin();
            //spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            SpriteEffects flip = SpriteEffects.None;
            if (facingDir != Direction.Left)
               flip = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0,0), flip, 0);
            spriteBatch.End();
        }

        public bool checkCollision(GameObject other)
        {
            if (BoundingBox.Intersects(other.BoundingBox))
            {
                Location.Y = other.BoundingBox.Top - texture.Height + 10;
                YMomentum = 0;
                inAir = false;
                return true;
            }
            return false;
        }
    }
}
