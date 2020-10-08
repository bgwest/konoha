using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace konoha
{
    class Obstacle
    {
        protected Vector2 position;
        protected int spriteWidth;
        protected int spriteHeight;
        protected Vector2 hitPosition;
        protected int hitPositionRadiusOffset;
        protected int hitPositionHeightOffset;

        public static List<Obstacle> obstacles = new List<Obstacle>();

        public Vector2 HitPosition
        {
            get { return hitPosition; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public int Radius
        {
            get { return spriteWidth / 2; }
        }

        public int HitPositionRadiusOffset
        {
            get { return Radius - hitPositionRadiusOffset; }
        }

        public Obstacle(Vector2 newPos)
        {
            position = newPos;
        }

        public static bool DidCollide(Vector2 positionToTest, int radiusToTest)
        {
            foreach (Obstacle obstacle in Obstacle.obstacles)
            {
                int sum = obstacle.HitPositionRadiusOffset + radiusToTest;

                if (Vector2.Distance(obstacle.HitPosition, positionToTest) < sum)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Tree : Obstacle
    {
        public Tree(Vector2 newPos) : base(newPos)
        {
            spriteWidth = 128;
            spriteHeight = 192;
            hitPositionRadiusOffset = 18;
            hitPositionHeightOffset = 34;

            hitPosition = new Vector2(position.X + Radius, position.Y + (spriteHeight / 2 + hitPositionHeightOffset));
        }
    }

    class Bush : Obstacle
    {
        public Bush(Vector2 newPos) : base(newPos)
        {
            spriteWidth = 112;
            spriteHeight = 114;
            hitPositionRadiusOffset = 24;

            hitPosition = new Vector2(position.X + Radius, position.Y + spriteHeight / 2);
        }
    }
}
