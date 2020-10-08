using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace konoha
{
    class Projectile
    {
        private Vector2 position;
        private int speed = 800;
        private static int projectileSpriteWidth = 30;
        private Dir direction;
        private bool collision = false;

        public static List<Projectile> projectiles = new List<Projectile>();

        public Projectile(Vector2 newPos, Dir newDir)
        {
            position = newPos;
            direction = newDir;
        }

        public bool Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        public Vector2 Position { get { return position; } }

        public int Radius { get { return projectileSpriteWidth / 2; } }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
    }
}
