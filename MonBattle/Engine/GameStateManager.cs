using MonBattle.Config.Internal;
using MonBattle.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace MonBattle.Engine
{
    public class GameStateManager
    {
        #region Singleton
        private static GameStateManager _singleton;

        public static GameStateManager GetGameStateManager(ContentManager _contentManager)
        {
            if (_singleton == null)
            {
                _singleton = new GameStateManager();
                _singleton._contentManager = _contentManager;
            }
            return _singleton;
        }
        #endregion

        private IGameState currentState;
        private List<GameStateTransition> GSTs = new List<GameStateTransition>();
        private List<GSTEnum> PauseUpdateStates = new List<GSTEnum>() { GSTEnum.DELAY };
        private ContentManager _contentManager;

        public void ChangeState(IGameState newState)
        {
            currentState?.UnloadContent(); // Unload content of the current state if it exists
            currentState = newState;
            currentState.LoadContent(_contentManager);
            currentState.Initialize();
            DetermineGSTs();
        }

        public void Update(GameTime gameTime)
        {
            if (Game.transitionTo != GameStateEnum.NONE)
            {
                TransitionByGSEnum();
                Game.transitionTo = GameStateEnum.NONE;
            }

            if (GSTs.Count == 0)
            {
                currentState?.Update(gameTime);
                return;
            }

            if (GSTs[0].IsCompleted)
            {
                GSTs.RemoveAt(0);
                return;
            }

            GSTs[0].Update(gameTime);

            if (PauseUpdateStates.Contains(GSTs[0].GSTEnum))
            {
                return;
            }

            currentState?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentState?.Draw(spriteBatch);
        }

        private void TransitionByGSEnum()
        {
            switch (Game.transitionTo)
            {
                case GameStateEnum.HOME:
                    ChangeState(new GSHome());
                    break;
                case GameStateEnum.BATTLE:
                    break;
                default:
                    throw new Exception();
            }
        }

        private void DetermineGSTs()
        {
            switch (Game.transitionTo)
            {
                case GameStateEnum.HOME:
                    // No Transition
                    break;
                case GameStateEnum.BATTLE:
                default:
                    GSTs.AddRange(LoaderPackage.DefaultGST);
                    break;
            }
        }
    }

    public enum GSTEnum
    {
        DELAY = 0,
        BLACK_FADE = 1,
    }

    public abstract class GameStateTransition
    {
        protected Timer timer = new();
        protected GSTEnum gstEnum;
        protected int runtime;
        protected bool completed = false;

        public GameStateTransition(int runtime)
        {
            this.runtime = runtime;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public GSTEnum GSTEnum { get { return gstEnum; } set { this.gstEnum = value; } }
        public bool IsCompleted { get { return completed; } }
    }

    public class GSTBlackFade : GameStateTransition
    {
        private Rectangle rect;
        private Texture2D texture;
        private float frameFade;
        private float currentFade;

        public GSTBlackFade(int runtime, bool fadeOut) : base(runtime)
        {
            this.GSTEnum = GSTEnum.BLACK_FADE;
            rect = new Rectangle(0, 0, 1080, 720);
            texture = Game._cache.TextureCache["BlackPixel"];

            if (fadeOut)
            {
                frameFade = 1f / runtime;
                currentFade = 0f;
            }
            else
            {
                frameFade = -1f / runtime;
                currentFade = 1f;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, rect, new Color(Color.White, currentFade));
        }

        public override void Update(GameTime gameTime)
        {
            if (currentFade > 1f || currentFade < 0)
            {
                this.completed = true;
                return;
            }
            currentFade += frameFade;
        }
    }

    public class GSTDelay : GameStateTransition
    {
        public GSTDelay(int runtime) : base(runtime)
        {
            this.GSTEnum = GSTEnum.DELAY;
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            timer.updateTime(gameTime.ElapsedGameTime.Milliseconds);
            if (timer.isTimeMet(1000))
            {
                this.completed = true;
            }
        }
    }
}
