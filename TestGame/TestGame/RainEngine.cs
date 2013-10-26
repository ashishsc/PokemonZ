using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public class RainEngine
    {
        private Random random;
        //public Vector2 EmitterLocation { get; set; }
        private List<RainParticle> particles;
        private List<Texture2D> textures;
        private List<Color> colors;

        public RainEngine(List<Texture2D> textures, List<Color> colors, Vector2 location)
        {
            //EmitterLocation = location;
            this.textures = textures;
            this.colors = colors;
            this.particles = new List<RainParticle>();
            random = new Random();
        }

        private RainParticle GenerateNewParticle()
        {
            Texture2D texture = textures[0];
            Vector2 position = new Vector2((float)random.NextDouble() * 750, 0f);
            Vector2 velocity = new Vector2(0f, 5f + (float)random.NextDouble() * 2 - 1);
            float angle = 0;
            float angularVelocity = 0;
            Color color = Color.Red;//colors[random.Next(colors.Count)];
            float size = 0.2f;
            int ttl = 300 + random.Next(10);

            return new RainParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Update()
        {
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

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(0, 0, 150, 50);
            }
        }
    }
}
