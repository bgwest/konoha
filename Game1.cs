using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace konoha
{
    enum Dir
    {
        Up, Down, Left, Right
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D player_Sprite;
        Texture2D playerDown;
        Texture2D playerUp;
        Texture2D playerLeft;
        Texture2D playerRight;

        Texture2D eyeEnemy_Sprite;
        Texture2D snakeEnemy_Sprite;
        Texture2D bush_Sprite;
        Texture2D tree_Sprite;

        Texture2D heart_Sprite;
        Texture2D bullet_Sprite;

        Player player = new Player();

        static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
        }

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics = new GraphicsDeviceManager(this);

            // TODO: Solve why this isn't setting
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            // TODO: this was a debug step and should not be needed, but also is not working
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player_Sprite = Content.Load<Texture2D>("player/player");
            playerUp = Content.Load<Texture2D>("player/playerUp");
            playerRight = Content.Load<Texture2D>("player/playerRight");
            playerDown = Content.Load<Texture2D>("player/playerDown");
            playerLeft = Content.Load<Texture2D>("player/playerLeft");

            eyeEnemy_Sprite = Content.Load<Texture2D>("enemies/eyeEnemy");
            snakeEnemy_Sprite = Content.Load<Texture2D>("enemies/snakeEnemy");

            bush_Sprite = Content.Load<Texture2D>("obstacles/bush");
            tree_Sprite = Content.Load<Texture2D>("obstacles/tree");

            bullet_Sprite = Content.Load<Texture2D>("misc/bullet");
            heart_Sprite = Content.Load<Texture2D>("misc/heart");

            player.animations[0] = new AnimatedSprite(playerUp, 1, 4);
            player.animations[1] = new AnimatedSprite(playerDown, 1, 4);
            player.animations[2] = new AnimatedSprite(playerLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(playerRight, 1, 4);

            // FOR TESTING -- NOT OK/LONG-TERM
            Enemy.enemies.Add(new Snake(new Vector2(100, 400)));
            Enemy.enemies.Add(new EyeOfChalupa(new Vector2(300, 450)));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            foreach (Projectile projectile in Projectile.projectiles)
            {
                projectile.Update(gameTime);
            }

            foreach (Enemy enemy in Enemy.enemies)
            {
                enemy.Update(gameTime, player.Position);
            }

            foreach (Projectile projectile in Projectile.projectiles)
            {
                foreach (Enemy enemy in Enemy.enemies)
                {
                    int sum = projectile.Radius + enemy.HitBoxRadius;

                    if (Vector2.Distance(projectile.Position, enemy.Position) < sum)
                    {
                        projectile.Collision = true;
                        enemy.Heath--;
                    }
                }
            }

            Projectile.projectiles.RemoveAll(projectile => projectile.Collision);
            Enemy.enemies.RemoveAll(enemy => enemy.Heath == 0);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            player.anim.Draw(_spriteBatch, new Vector2(player.Position.X - (player.playerSpriteWidth / 2), player.Position.Y - (player.playerSpriteWidth / 2)));

            _spriteBatch.Begin();

            foreach (Projectile projectile in Projectile.projectiles)
            {
                _spriteBatch.Draw(bullet_Sprite, new Vector2(projectile.Position.X - projectile.Radius, projectile.Position.Y - projectile.Radius), Color.White);
            }

            foreach (Enemy enemy in Enemy.enemies)
            {
                Texture2D spriteToDraw;
                // for centering the image on draw
                int enemyImageRadiusForDrawOffset;

                if (enemy.GetType() == typeof(Snake))
                {
                    spriteToDraw = snakeEnemy_Sprite;
                    enemyImageRadiusForDrawOffset = Snake.SnakeSpriteWidth / 2;
                } else
                {
                    spriteToDraw = eyeEnemy_Sprite;
                    enemyImageRadiusForDrawOffset = EyeOfChalupa.EyeSpritewidth / 2;
                }

                _spriteBatch.Draw(spriteToDraw, new Vector2(enemy.Position.X - enemyImageRadiusForDrawOffset, enemy.Position.Y - enemyImageRadiusForDrawOffset), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
