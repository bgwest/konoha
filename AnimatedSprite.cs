using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace konoha
{
    // original AnimatedSprite class borrowed from: rbwhitaker.wikidot.com/monogame-texture-atlases-2
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private double timer;
        private double walkingSpeed;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            walkingSpeed = 0.15D;
            timer = walkingSpeed;
        }

        public void Update(GameTime gameTime)
        {
            // TODO: confirm understanding here and then remove comment
            // Currently, the only way this makes sense to me is if Update restarts it's ElapsedGameTime
            // since the last time it was called. Is that true?
            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
              currentFrame++;
              timer = walkingSpeed;
            }

            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            // this comment can probably be removed after I'm more of a monogame wizard
            // but right now this is a reminder that this Begin/End call is calling the same
            // Begin/End as in the main Game.cs Draw Method
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}