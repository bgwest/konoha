using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace konoha
{
    class Enemy
    {
        private Vector2 position;
        protected int health;
        protected int speed;
        protected int radius;

        public static List<Enemy> enemies = new List<Enemy>();

        public int Heath {
            get { return health;  }
            set { health = value;  }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public int Radius
        {
            get { return radius; }
        }

        public Enemy(Vector2 newPos)
        {
            position = newPos;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 moveDir = playerPos - position;
            // reduces magnitude. gets the dir we want, but with a length of 1
            moveDir.Normalize();
            position += moveDir * speed * deltaTime;
        }
    }

    class Snake : Enemy
    {
        public int snakeSpriteWidth = 100;

        public Snake(Vector2 newPos) : base(newPos) {
            speed = 160;
        }

    }

    class EyeOfChalupa : Enemy
    {
        public int eyeSpriteWidth = 100;

        public EyeOfChalupa(Vector2 newPos) : base(newPos) {
            speed = 80;
        }

    }
}
