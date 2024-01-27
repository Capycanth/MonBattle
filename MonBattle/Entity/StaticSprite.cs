using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Capybara_1.Entity
{
    public class StaticSprite : DrawableBase
    {
        private Rectangle destination;
        private Rectangle source;
        private Texture2D texture;

        public Rectangle Destination { get { return this.destination; } set { this.destination = value; } }
        public Rectangle Source { get { return this.source; } set { this.source = value; } }
        public Texture2D Texture { get { return this.texture; } set { this.texture = value; } }

        public StaticSprite(Texture2D texture, Rectangle destination, Rectangle source)
        {
            this.texture = texture;
            this.destination = destination;
            this.source = source;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, destination, source, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
