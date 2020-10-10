using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace konoha
{
    enum DirectionTranslationFromBaseDirectionSets
    {
      playerWithAxe = 4,
      playerUsingAxe = 4,
    }

    class Player
    {
        private Vector2 position = new Vector2(100, 100);
        private int health = 3;
        private int speed = 200;
        private Dir direction = Dir.Down;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        private float healthTimer = 0f;
        private float weaponTimer = 0f;
        private Keys hotEquipWeaponKey = Keys.A;
        private Keys useEquippedWeaponKey = Keys.S;

        public int playerSpriteWidth = 96;
        public AnimatedSprite anim;
        public AnimatedSprite[] animations = new AnimatedSprite[12];

        public Player()
        {
        }

        public float HealthTimer
        {
            get { return healthTimer; }
            set { healthTimer = value; }
        }

        public int HitBoxRadius
        {
            get { return (playerSpriteWidth / 2) + 8; }
        }

        public int Radius
        {
            get { return playerSpriteWidth / 2; }
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

            Keys[] currentKeysPress = kState.GetPressedKeys();

            bool isEquippingWeapon = false;
            bool isUsingWeapon = false;

            foreach (Keys key in currentKeysPress)
            {
                if (key == hotEquipWeaponKey)
                {
                    isEquippingWeapon = true;
                }

                if (key == useEquippedWeaponKey)
                {
                    isUsingWeapon = true;
                }
            }

            if (healthTimer > 0)
            {
                healthTimer -= deltaTime;
            }

            int setDirection = (int)direction;

            if (isEquippingWeapon)
                setDirection += (int)DirectionTranslationFromBaseDirectionSets.playerWithAxe;

            if (weaponTimer > 0)
            {
                weaponTimer -= deltaTime;
                setDirection += (int)DirectionTranslationFromBaseDirectionSets.playerUsingAxe;
            }

            bool isSwingingWeapon = (isEquippingWeapon && isUsingWeapon) || weaponTimer > 0;


            if (isSwingingWeapon)
            {
                if (weaponTimer <= 0)
                {
                  weaponTimer = .3f;
                  setDirection += (int)DirectionTranslationFromBaseDirectionSets.playerUsingAxe;
                }
            }

            // TODO: stacking swings causes this to crash - try button mashing swings or holding down swing to repro
            anim = animations[setDirection];

            if (isMoving || isSwingingWeapon)
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
                Vector2 positionContainer = position;

                switch (direction)
                {
                    case Dir.Right:
                        positionContainer.X += speed * deltaTime;

                        if (!Obstacle.DidCollide(positionContainer, Radius))
                        {
                            position.X += speed * deltaTime;
                        }
                        break;

                    case Dir.Left:
                        positionContainer.X -= speed * deltaTime;

                        if (!Obstacle.DidCollide(positionContainer, Radius))
                        {
                            position.X -= speed * deltaTime;
                        }

                        break;

                    case Dir.Down:
                        positionContainer.Y += speed * deltaTime;

                        if (!Obstacle.DidCollide(positionContainer, Radius))
                        {
                            position.Y += speed * deltaTime;
                        }

                        break;

                    case Dir.Up:
                        positionContainer.Y -= speed * deltaTime;

                        if (!Obstacle.DidCollide(positionContainer, Radius))
                        {
                            position.Y -= speed * deltaTime;
                        }

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
