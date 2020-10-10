using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace konoha
{
    enum Dir
    {
        Up, Down, Left, Right,
        UpWithAxe, DownWithAxe, LeftWithAxe, RightWithAxe
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerDown;
        Texture2D playerUp;
        Texture2D playerLeft;
        Texture2D playerRight;

        Texture2D playerDownWithAxe;
        Texture2D playerUpWithAxe;
        Texture2D playerLeftWithAxe;
        Texture2D playerRightWithAxe;

        Texture2D playerDownWithAxeSwing;

        Texture2D eyeEnemy_Sprite;
        Texture2D snakeEnemy_Sprite;
        Texture2D bush_Sprite;
        Texture2D tree_Sprite;

        Texture2D heart_Sprite;
        Texture2D bullet_Sprite;

        TiledMapRenderer mapRenderer;
        TiledMap myMap;

        OrthographicCamera cam;

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

            mapRenderer = new TiledMapRenderer(GraphicsDevice);
            mapRenderer.LoadMap(myMap);

            cam = new OrthographicCamera(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerUp = Content.Load<Texture2D>("player/playerUp");
            playerDown = Content.Load<Texture2D>("player/playerDown");
            playerRight = Content.Load<Texture2D>("player/playerRight");
            playerLeft = Content.Load<Texture2D>("player/playerLeft");

            playerUpWithAxe = Content.Load<Texture2D>("player/withAxe/playerUpWithAxe");
            playerRightWithAxe = Content.Load<Texture2D>("player/withAxe/playerRightWithAxe");
            playerDownWithAxe = Content.Load<Texture2D>("player/withAxe/playerDownWithAxe");
            playerLeftWithAxe = Content.Load<Texture2D>("player/withAxe/playerLeftWithAxe");

            // TODO: consider adding more frames? analyze the twitching issue and then decide
            playerDownWithAxeSwing = Content.Load<Texture2D>("player/withAxeSwing/playerDownWithAxeSwing");

            eyeEnemy_Sprite = Content.Load<Texture2D>("enemies/eyeEnemy");
            snakeEnemy_Sprite = Content.Load<Texture2D>("enemies/snakeEnemy");

            bush_Sprite = Content.Load<Texture2D>("obstacles/bush");
            tree_Sprite = Content.Load<Texture2D>("obstacles/tree");

            bullet_Sprite = Content.Load<Texture2D>("misc/bullet");
            heart_Sprite = Content.Load<Texture2D>("misc/heart");

            // NO EQUIP
            player.animations[0] = new AnimatedSprite(playerUp, 1, 4, 0.15D);
            player.animations[1] = new AnimatedSprite(playerDown, 1, 4, 0.15D);
            player.animations[2] = new AnimatedSprite(playerLeft, 1, 4, 0.15D);
            player.animations[3] = new AnimatedSprite(playerRight, 1, 4, 0.15D);

            // WITH AXE
            player.animations[4] = new AnimatedSprite(playerUpWithAxe, 1, 4, 0.15D);
            player.animations[5] = new AnimatedSprite(playerDownWithAxe, 1, 4, 0.15D);
            player.animations[6] = new AnimatedSprite(playerLeftWithAxe, 1, 4, 0.15D);
            player.animations[7] = new AnimatedSprite(playerRightWithAxe, 1, 4, 0.15D);

            // WITH AXE + SWING
            player.animations[8] = new AnimatedSprite(playerDownWithAxeSwing, 1, 4, 0.1D);
            player.animations[9] = new AnimatedSprite(playerDownWithAxeSwing, 1, 4, 0.1D);
            player.animations[10] = new AnimatedSprite(playerDownWithAxeSwing, 1, 4, 0.1D);
            player.animations[11] = new AnimatedSprite(playerDownWithAxeSwing, 1, 4, 0.1D);


            myMap = Content.Load<TiledMap>("misc/rpgTilesMap");

            //
            // COMMENTING OUT ENEMIES FOR NOW WHILE DEVELOPING THE SWING ANIMATIONS
            //
            //TiledMapObject[] allEnemies = myMap.GetLayer<TiledMapObjectLayer>("enemies").Objects;
            //foreach (TiledMapObject enemy in allEnemies)
            //{
            //    string type;
            //    enemy.Properties.TryGetValue("Type", out type);

            //    if (type == "Snake")
            //    {
            //        Enemy.enemies.Add(new Snake(enemy.Position));
            //    } else if (type == "Eye")
            //    {
            //        Enemy.enemies.Add(new EyeOfChalupa(enemy.Position));
            //    }

            //}

            TiledMapObject[] allObstacles = myMap.GetLayer<TiledMapObjectLayer>("obstacles").Objects;
            foreach (TiledMapObject obstacle in allObstacles)
            {
                string type;
                obstacle.Properties.TryGetValue("Type", out type);

                if (type == "Tree")
                {
                    Obstacle.obstacles.Add(new Tree(obstacle.Position));
                } else if (type == "Bush")
                {
                    Obstacle.obstacles.Add(new Bush(obstacle.Position));
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (player.Health > 0)
              player.Update(gameTime);

            cam.LookAt(player.Position);

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

                if (Obstacle.DidCollide(projectile.Position, projectile.Radius))
                {
                    projectile.Collision = true;
                }
            }

            foreach (Enemy enemy in Enemy.enemies)
            {
                int sum = player.HitBoxRadius + enemy.HitBoxRadius;
                if (Vector2.Distance(player.Position, enemy.Position) < sum && player.HealthTimer <= 0)
                {
                    player.Health--;
                    player.HealthTimer = 1.5f;
                }
            }

            Projectile.projectiles.RemoveAll(projectile => projectile.Collision);
            Enemy.enemies.RemoveAll(enemy => enemy.Heath == 0);

            mapRenderer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            mapRenderer.Draw(cam.GetViewMatrix());

            _spriteBatch.Begin(transformMatrix: cam.GetViewMatrix());

            if (player.Health > 0)
                player.anim.Draw(_spriteBatch, new Vector2(player.Position.X - (player.playerSpriteWidth / 2), player.Position.Y - (player.playerSpriteHeight / 2)));

            foreach (Projectile projectile in Projectile.projectiles)
            {
                _spriteBatch.Draw(bullet_Sprite, new Vector2(projectile.Position.X - projectile.Radius, projectile.Position.Y - projectile.Radius), Color.White);
            }

            foreach (Obstacle obstacle in Obstacle.obstacles)
            {
                Texture2D obstacleSprite;

                if (obstacle.GetType() == typeof(Tree))
                    obstacleSprite = tree_Sprite;
                else
                    obstacleSprite = bush_Sprite;

                // note: notice how it's being drawn at the top left corner with no offset
                _spriteBatch.Draw(obstacleSprite, obstacle.Position, Color.White);
            }

            foreach (Enemy enemy in Enemy.enemies)
            {
                Texture2D enemySprite;

                // for centering the image on draw
                int enemyImageRadiusForDrawOffset;

                if (enemy.GetType() == typeof(Snake))
                {
                    enemySprite = snakeEnemy_Sprite;
                    enemyImageRadiusForDrawOffset = Snake.SnakeSpriteWidth / 2;
                } else
                {
                    enemySprite = eyeEnemy_Sprite;
                    enemyImageRadiusForDrawOffset = EyeOfChalupa.EyeSpritewidth / 2;
                }

                _spriteBatch.Draw(enemySprite, new Vector2(enemy.Position.X - enemyImageRadiusForDrawOffset, enemy.Position.Y - enemyImageRadiusForDrawOffset), Color.White);
            }

            _spriteBatch.End();


            _spriteBatch.Begin();

            for (int i = 0; i < player.Health; i++)
            {
                _spriteBatch.Draw(heart_Sprite, new Vector2(5 + i * Heart.HeartSpriteWidth, 5), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
