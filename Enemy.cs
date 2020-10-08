using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace konoha
{
    class Enemy
    {
        private Vector2 currentEnemyPosition;
        protected int health;
        // when "enemySpeed" was named just "speed" (which matched the Player class)
        // the debugger was using the Player class value for "speed"
        // even though "speed" is a private int
        protected int enemySpeed;
        protected int radius;
        protected int hitboxRadius;

        public static List<Enemy> enemies = new List<Enemy>();

        public int Heath {
            get { return health;  }
            set { health = value;  }
        }

        public Vector2 Position
        {
            get { return currentEnemyPosition; }
        }

        public int Radius
        {
            get { return radius; }
        }

        public int HitBoxRadius
        {
            get { return hitboxRadius; }
        }

        public Enemy(Vector2 newPos)
        {
            currentEnemyPosition = newPos;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            // used to guard against frame rate drops or increases 
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //  this vector points FROM the enemy TO the direction of the player
            Vector2 pointEnemyToPlayer = playerPos - currentEnemyPosition;

            // keeps vector direction but reduces magnitude (to length of 1 - e.g. (5,0) -> (1,0))
            pointEnemyToPlayer.Normalize();

            Vector2 positionContainer = currentEnemyPosition;

            // finally, enemy position is increased by the normailized length
            // delta time in debug on average is showing up as .0166667
            // making the movent relatively small
            positionContainer += pointEnemyToPlayer * enemySpeed * deltaTime;

            if (!Obstacle.DidCollide(positionContainer, HitBoxRadius))
            {
                currentEnemyPosition += pointEnemyToPlayer * enemySpeed * deltaTime;
            }
        }
    }

    class Snake : Enemy
    {
        public Snake(Vector2 newPos) : base(newPos) {
            enemySpeed = 80;
            hitboxRadius = 42;
            health = 3;
        }

        public static int SnakeSpriteWidth { get; } = 100;

    }

    class EyeOfChalupa : Enemy
    {
        public EyeOfChalupa(Vector2 newPos) : base(newPos) {
            enemySpeed = 120;
            hitboxRadius = 45;
            health = 5;
        }

        public static int EyeSpritewidth { get; } = 146;

    }
}
