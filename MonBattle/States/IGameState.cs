using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Capybara_1.Engine;
using Microsoft.Xna.Framework.Input;

namespace Capybara_1.States
{
    public enum GameStateEnum
    {
        // Add Game State Enums here for each Game State
    }

    public interface IGameState
    {
        GameStateEnum GameState { get; set; }
        InputManager InputManager { get; set; }

        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void UnloadContent();
        void HandleInput()
        {
            InputManager.HandleInput(Keyboard.GetState(), Mouse.GetState());
        }
    }
}
