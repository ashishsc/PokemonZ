using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private GameObject ground;
        private List<Texture2D> background;
        private List<Vector2> bgVect;
        private SpriteFont font;
        private int score;
        private Player player;
        private List<GameObject> spriteList;
        private FlamethrowerEngine flamethrowerEngine;
        private WatergunEngine watergunEngine;
        private RainEngine rainEngine;
        private int InitialZombies = 25;
        private List<Enemy> enemies;
        private int timeSinceLastSpawn = 0;
        private int spawnDelay = 50;
        private int WalkSpeed = 5;
        private Song song;
        SpriteFont dialogue;
        Vector2 dialoguePos;
        string dialogueOutput = "";

        List<GameObject> platforms;

        public Game1()
        {
            score = 0;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            enemies = new List<Enemy>();
            background = new List<Texture2D>();
            bgVect = new List<Vector2>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            song = Content.Load<Song>("ZombieTheme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);

            dialogue = Content.Load<SpriteFont>("dialogueLC");
            dialoguePos = new Vector2(50, 50);

            spriteList = new List<GameObject>();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("dialogueLC");
            ground = new GameObject(Content.Load<Texture2D>("ground"), new Vector2(0, 400));
            background.Add(Content.Load<Texture2D>("background"));
            background.Add(Content.Load<Texture2D>("background"));
            background.Add(Content.Load<Texture2D>("background"));
            bgVect.Add(new Vector2(-background[0].Width, 0));
            bgVect.Add(new Vector2(bgVect[0].X + background[0].Width, 0));
            bgVect.Add(new Vector2(bgVect[1].X + background[1].Width, 0));
            player = new Player(Content.Load<Texture2D>("brockfinal"), 1, 7, Content, new Vector2(380, 200), 100);
            InitializeZombies();

            platforms = new List<GameObject>();
            Random r = new Random();
            for (int i = 0; i < 200; i++)
            {
                int x = i * 75;
                int y = r.Next(75, 350);
                GameObject p = new GameObject(Content.Load<Texture2D>("platform"), new Vector2(x, y));
                platforms.Add(p);
            }

           
            platforms.Add(ground);


            List<Texture2D> flamethrowerTextures = new List<Texture2D>();
            flamethrowerTextures.Add(Content.Load<Texture2D>("flamethrower-particle"));
            flamethrowerEngine = new FlamethrowerEngine(flamethrowerTextures, new Vector2(400, 240));

            List<Texture2D> watergunTextures = new List<Texture2D>();
            watergunTextures.Add(Content.Load<Texture2D>("watergun-particle"));
            watergunEngine = new WatergunEngine(watergunTextures, new Vector2(400, 240));

            List<Texture2D> rainTextures = new List<Texture2D>();
            rainTextures.Add(Content.Load<Texture2D>("rain-particle"));
            List<Color> rainColors = new List<Color>();
            rainColors.Add(Color.White);
            rainEngine = new RainEngine(rainTextures, rainColors, new Vector2(0, 0));


            //spriteList.Add(testPlayer);
            //spriteList.Add(ground);
            spriteList.Add(player);
            spriteList.Add(watergunEngine);
            spriteList.Add(flamethrowerEngine);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }

        private void CheckMovement()
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
            {
                player.Move(false);
                Move(WalkSpeed);
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                player.Move(true);
                Move(-WalkSpeed);
            }
            if (ks.IsKeyDown(Keys.Space) && player.isNotJumping())
                for (int i = 0; i < platforms.Count; i++)
                {
                    player.Jump(platforms[i]);
                }
            if (ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Right))
                player.ResetAnimation();
        }

        private void Move(int dX)
        {
            if (bgVect[0].X <= -background[0].Width)
            {
                Vector2 v = new Vector2(background[0].Width * 2, bgVect[0].Y);
                bgVect[0] = bgVect[1];
                bgVect[1] = bgVect[2];
                bgVect[2] = v;
            }
            else if (bgVect[2].X >= background[2].Width * 2)
            {
                Vector2 v = new Vector2(-background[0].Width, bgVect[0].Y);
                bgVect[2] = bgVect[1];
                bgVect[1] = bgVect[0];
                bgVect[0] = v;
            }

            bgVect[0] = new Vector2(bgVect[0].X + dX -2, bgVect[0].Y);
            bgVect[1] = new Vector2(bgVect[1].X + dX -2, bgVect[1].Y);
            bgVect[2] = new Vector2(bgVect[2].X + dX -2, bgVect[2].Y);
            foreach (Enemy e in enemies)
            {
                e.Location.X += dX;
            }
            foreach (GameObject o in platforms)
            {
                o.Location.X += dX;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (ks.IsKeyDown(Keys.X))
            {
                flamethrowerEngine.EmitterLocation = new Vector2(player.Location.X + player.BoundingBox.Width / 2, player.Location.Y + player.BoundingBox.Height / 2);
                flamethrowerEngine.Update(player.facingDir);
            }
            else
            {
                List<Texture2D> flamethrowerTextures = new List<Texture2D>();
                flamethrowerTextures.Add(Content.Load<Texture2D>("flamethrower-particle"));
                flamethrowerEngine = new FlamethrowerEngine(flamethrowerTextures, new Vector2(0, 0));
            }
            if (ks.IsKeyDown(Keys.Z))
            {
                watergunEngine.EmitterLocation = new Vector2(player.Location.X + player.BoundingBox.Width / 2, player.Location.Y + player.BoundingBox.Height / 2);
                watergunEngine.Update(player.facingDir);
            }
            else
            {
                List<Texture2D> watergunTextures = new List<Texture2D>();
                watergunTextures.Add(Content.Load<Texture2D>("watergun-particle"));
                watergunEngine = new WatergunEngine(watergunTextures, new Vector2(0, 0));
            }
            rainEngine.Update();

            CheckMovement();
            CheckCollision();
            //KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Z) || ks.IsKeyDown(Keys.X))
            {
                CheckAttack();
            }
            if (timeSinceLastSpawn > 0)
            {
                timeSinceLastSpawn--;
            }
            if (timeSinceLastSpawn == 0 && enemies.Count < InitialZombies)
            {
                timeSinceLastSpawn = spawnDelay;
                spawnNewEnemies(1);
            }
            CheckEnemies();

            score++;
            player.Update();
            foreach (Enemy e in enemies)
            {
                e.Update();
                e.UpdateMovement(GraphicsDevice.Viewport);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
              

            spriteBatch.Begin();           

            spriteBatch.Draw(background[0], new Rectangle((int)bgVect[0].X, (int)bgVect[0].Y, background[0].Width + 20, background[0].Height), Color.White);
            spriteBatch.Draw(background[1], new Rectangle((int)bgVect[1].X, (int)bgVect[1].Y, background[1].Width + 20, background[1].Height), Color.White);
            spriteBatch.Draw(background[2], new Rectangle((int)bgVect[2].X, (int)bgVect[2].Y, background[2].Width + 20, background[2].Height), Color.White);
            //spriteBatch.Draw(platform1.texture, new Rectangle(200, 300, platform1.BoundingBox.Width, platform1.BoundingBox.Height), Color.White);
            //spriteBatch.DrawString(font, "Score " + score, new Vector2(100, 100), Color.Green);
            //spriteBatch.DrawString(font, "Health " + player.health, new Vector2(100, 100), Color.White);
            //spriteBatch.DrawString(font, dialogueOutput, new Vector2(player.Location.X, player.Location.Y - 20), Color.White);


            Vector2 FontOrigin = dialogue.MeasureString(dialogueOutput) / 2;
            spriteBatch.DrawString(dialogue, dialogueOutput, dialoguePos, Color.Yellow);


            foreach (Enemy e in enemies)
            {
                //   DrawRectangle(e.BoundingBox, Color.Red);
            }

            foreach (GameObject o in platforms)
            {
                spriteBatch.Draw(o.texture, new Vector2(o.Location.X, o.Location.Y), Color.White);
            }
            //spriteBatch.DrawString(font, "Red Box: L:" + player.BoundingBox.Left + ", R: " + player.BoundingBox.Right + ", B: " + player.BoundingBox.Bottom + ", T: " + player.BoundingBox.Top, new Vector2(100, 0), Color.Green);
            //spriteBatch.DrawString(font, "Gre Box: L:" + testPlayer.BoundingBox.Left + ", R: " + testPlayer.BoundingBox.Right + ", B: " + testPlayer.BoundingBox.Bottom + ", T: " + testPlayer.BoundingBox.Top, new Vector2(100, 25), Color.Green);
            //DrawRectangle(ground.BoundingBox, Color.Red);

            spriteBatch.End();
            player.Draw(spriteBatch);//, new Vector2(400, 200));
            //testPlayer.Draw(spriteBatch);
            KeyboardState ks = Keyboard.GetState();
            if(ks.IsKeyDown(Keys.Z) || ks.IsKeyDown(Keys.X))
            {
                flamethrowerEngine.Draw(spriteBatch);
                watergunEngine.Draw(spriteBatch);
            }
            rainEngine.Draw(spriteBatch);
            DrawZombies();

            base.Draw(gameTime);
        }

        private void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            spriteBatch.Draw(rect, coords, color);
        }

        private void DrawZombies()
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
        }

        private void CheckEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Location.X < -200 || enemies[i].Location.X > 1000)
                {
                    enemies.Remove(enemies[i]);
                    spawnNewEnemies(1);
                }
            }
        }

        private void InitializeZombies()
        {
            spawnNewEnemies(InitialZombies);
        }

        public void CheckAttack()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (flamethrowerEngine.IsColliding(enemies[i]) == Direction.Left || watergunEngine.IsColliding(enemies[i]) == Direction.Left)
                {
                    if (enemies[i].texture.Name == "vulpix")
                    {
                        dialogueOutput = "Breed on this!";
                    }
                    else if (enemies[i].texture.Name == "pikachu")
                    {
                        dialogueOutput = "Forgive me Brock...!";
                    }
                    else if (enemies[i].texture.Name == "pichu")
                    {
                        dialogueOutput = "Piiiiichuuuuuuuu!";
                    }
                    else if (enemies[i].texture.Name == "jigglypuff")
                    {
                        dialogueOutput = "Doodle on my face again, yea.";
                    }
                    else if (enemies[i].texture.Name == "aguman")
                    {
                        dialogueOutput = "What the #$%";
                    }

                    enemies[i].Location.Y = 0;
                    enemies.Remove(enemies[i]);
                    i--;
                }
            }
        }

        protected void CheckCollision()
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                player.checkCollision(platforms[i]);
                foreach (Enemy e in enemies)
                {
                    e.checkCollision(platforms[i]);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                player.checkCollision(enemies[i]);
            }
        }

        private void spawnNewEnemies(int num)
        {
            Random r = new Random();
            for (int i = 0; i < num; i++)
            {
                int x = r.Next(0, 800);
                while (x > 300 && x < 500)
                {
                    x = r.Next(0, 800);
                }
               // Enemy e = new Enemy(Content.Load<Texture2D>("enemy"), 1, 1, Content, new Vector2(x, -100), 100);
                Enemy e;
                switch (r.Next(5))
                {
                    case 0:
                        e = new Enemy(Content.Load<Texture2D>("vulpix"), 1, 1,
                        Content, new Vector2(x, -370), 100);
                        e.texture.Name = "vulpix";
                        break;
                    case 1:
                        e = new Enemy(Content.Load<Texture2D>("pikachu"), 1, 1,
                        Content, new Vector2(x, -370), 100);
                        e.texture.Name = "pikachu";
                        break;
                    case 2:
                        e = new Enemy(Content.Load<Texture2D>("jigglypuff"), 1, 1,
                        Content, new Vector2(x, -370), 100);
                        e.texture.Name = "jigglypuff";
                        break;
                    default:
                        e = new Enemy(Content.Load<Texture2D>("aguman"), 1, 1,
                        Content, new Vector2(x, -370), 100);
                        e.texture.Name = "aguman";
                        break;
                }
                enemies.Add(e);
                spriteList.Add(e);
            }
        }
    }
}
