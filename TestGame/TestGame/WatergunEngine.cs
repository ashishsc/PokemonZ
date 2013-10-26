using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public class WatergunEngine : GameObject
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<FlamethrowerParticle> particles;
        private List<Texture2D> textures;
        private List<Color> colors;
        private int direction;

        public WatergunEngine(List<Texture2D> textures, Vector2 location)  : base(textures[0], location)
        {
            direction = 0;
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<FlamethrowerParticle>();
            random = new Random();

            colors = new List<Color>();
            colors.Add(Color.Blue);
            colors.Add(Color.Aqua);
        }

        private FlamethrowerParticle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(7f * direction, (float)(random.NextDouble() * 1 - .5));
            float angle = 0;
            float angularVelocity = 0;
            Color color = colors[random.Next(colors.Count)];
            float size = (float)random.NextDouble();
            int ttl = 20 + random.Next(10);

            return new FlamethrowerParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }


        public void Update(Direction dir)
        {
            if (dir != Direction.Left)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            int total = 10;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        new public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)EmitterLocation.X, (int)EmitterLocation.Y, 150, 50);
            }
        }

        new public Direction IsColliding(GameObject other)
        {
            foreach (FlamethrowerParticle p in particles)
            {
                Rectangle r = new Rectangle((int)p.Position.X, (int)p.Position.Y, p.texture.Width, p.texture.Height);
                if (r.Intersects(other.BoundingBox))
                {
                    return Direction.Left;
                }
            }
            return Direction.none;
        }
    }
}
