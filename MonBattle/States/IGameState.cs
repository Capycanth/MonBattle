using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonBattle.Engine;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonBattle.Entity;
using Microsoft.Xna.Framework.Content;
using System.Threading.Tasks;

namespace MonBattle.States
{
    public enum GameStateEnum
    {
        NONE = 0,
        HOME = 1,
        BATTLE = 2,
        FREEROAM = 3,
        CUTSCENE = 4,
        SETTINGS = 5,
    }

    public interface IGameState
    {
        GameStateEnum GSEnum { get; set; }
        InputManager InputManager { get; set; }
        List<DrawableBase> Drawables { get; set; }

        void Initialize();
        void LoadContent(ContentManager _contentManager);
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch _spriteBatch);
        void Begin(); // Begin music, intro camera animations, intro transitions, etc...
        void HandleInput()
        {
            InputManager.HandleInput(Keyboard.GetState(), Mouse.GetState());
        }
    }
}
