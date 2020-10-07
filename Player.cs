using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace konoha
{
    class Player
    {
        private Vector2 position = new Vector2(100, 100);
        private int health = 3;
        private int speed = 200;
        private Dir direction = Dir.Down;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        private float healthTimer = 0f;

        public int playerSpriteWidth = 96;
        public AnimatedSprite anim;
        public AnimatedSprite[] animations = new AnimatedSprite[4];

        public float HealthTimer
        {
            get { return healthTimer; }
            set { healthTimer = value; }
        }

        public int HitBoxRadius
        {
            get { return (playerSpriteWidth / 2) + 8; }
        }

        public Player()
        {
        }

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }

        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public void setX(float newX)
        {
            position.X = newX;
        }

        public void setY(float newY)
        {
            position.Y = newY;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (healthTimer > 0)
            {
                healthTimer -= deltaTime;
            }


            anim = animations[(int)direction];

            if (isMoving)
                anim.Update(gameTime);
            else
                anim.setFrame(1);

            isMoving = false;

            if (kState.IsKeyDown(Keys.Up))
            {
                isMoving = true;
                direction = Dir.Up;
            }

            if (kState.IsKeyDown(Keys.Down))
            {
                isMoving = true;
                direction = Dir.Down;
            }

            if (kState.IsKeyDown(Keys.Left))
            {
                isMoving = true;
                direction = Dir.Left;
            }

            if (kState.IsKeyDown(Keys.Right))
            {
                isMoving = true;
                direction = Dir.Right;
            }

            if (isMoving)
            {
                switch (direction)
                {
                    case Dir.Right:
                        position.X += speed * deltaTime;
                        break;
                    case Dir.Left:
                        position.X -= speed * deltaTime;
                        break;
                    case Dir.Down:
                        position.Y += speed * deltaTime;
                        break;
                    case Dir.Up:
                        position.Y -= speed * deltaTime;
                        break;
                    default:
                        // TODO: Add event error handling
                        break;
                }
            }

            if (kState.IsKeyDown(Keys.Space) && kStateOld.IsKeyUp(Keys.Space))
            {
                Projectile.projectiles.Add(new Projectile(position, direction));
            }

            kStateOld = kState;
        }
    }
}
