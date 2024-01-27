using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonBattle.Entity
{
    public class AnimatedSprite : DrawableBase
    {
        private Rectangle destination;
        private Rectangle source;
        private Texture2D texture;
        private Texture2D spriteSheet;

        public Rectangle Destination { get { return this.destination; } set { this.destination = value; } }
        public Rectangle Source { get { return this.source; } set { this.source = value; } }
        public Texture2D Texture { get { return this.texture; } set { this.texture = value; } }

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
