using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Capybara_1.Entity
{
    public interface DrawableBase
    {
        Rectangle Destination { get; set; }
        Rectangle Source { get; set; }
        Texture2D Texture { get; set; }
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}
