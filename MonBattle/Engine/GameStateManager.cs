using MonBattle.Config.Internal;
using MonBattle.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Threading.Tasks;

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
        private ContentManager _contentManager;

        public async Task ChangeState(IGameState newState)
        {
            currentState?.UnloadContent(); // Unload content of the current state if it exists
            currentState = newState;
            await currentState.LoadContent(_contentManager);
            currentState.Initialize();
            GSTs[0].IsCompleted = true;
        }

        public void Update(GameTime gameTime)
        {
            currentState?.HandleInput();

            if (Game.transitionTo != GameStateEnum.NONE)
            {
                TransitionByGSEnum();
                return;
            }

            if (GSTs.Count == 0)
            {
                currentState.Update(gameTime);
            }
            else
            {
                if (GSTs[0].IsCompleted)
                {
                    GSTs.RemoveAt(0);
                    return;
                }
                switch (GSTs[0].GSTEnum)
                {
                    case GSTEnum.DELAY:
                        GSTs[0].Update(gameTime);
                        break;
                    case GSTEnum.BLACK_FADE:
                        GSTs[0].Update(gameTime);
                        break;
                    case GSTEnum.LOAD:
                        GSTs[0].Update(gameTime);
                        break;
                    default:
                        break;
                }
                return;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GSTs.Count == 0)
            {
                currentState.Draw(spriteBatch);
            }
            else
            {
                switch (GSTs[0].GSTEnum)
                {
                    case GSTEnum.DELAY:
                        currentState.Draw(spriteBatch);
                        break;
                    case GSTEnum.BLACK_FADE:
                        currentState.Draw(spriteBatch);
                        GSTs[0].Draw(spriteBatch);
                        break;
                    case GSTEnum.LOAD:
                        GSTs[0].Draw(spriteBatch);
                        break;
                    default:
                        break;
                }
            }
        }

        private void TransitionByGSEnum()
        {
            switch (Game.transitionTo)
            {
                case GameStateEnum.HOME:
                    ChangeState(new GSHome());
                    break;
                case GameStateEnum.BATTLE:
                    ChangeState(new GSBattle());
                    break;
                default:
                    throw new Exception();
            }
            Game.transitionTo = GameStateEnum.NONE;
        }

        public void SetGSTransitions(List<GameStateTransition> GSTs)
        {
            this.GSTs = GSTs;
        }
    }

    public enum GSTEnum
    {
        DELAY = 0,
        BLACK_FADE = 1,
        LOAD = 2,
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
        public bool IsCompleted { get { return completed; } set { this.completed = value; } }
    }

    public class GSTBlackFade : GameStateTransition
    {
        private Rectangle rect;
        private Texture2D texture;
        private float frameFade;
        private float currentFade;
        private bool fadeOut;

        public GSTBlackFade(int runtime, bool fadeOut) : base(runtime)
        {
            this.GSTEnum = GSTEnum.BLACK_FADE;
            this.rect = new Rectangle(0, 0, 1080, 720);
            this.texture = Game._cache.TextureCache["BlackPixel"];
            this.fadeOut = fadeOut;

            this.frameFade = 1f / runtime;
            this.currentFade = this.fadeOut ? 0f : 1f;
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.texture, this.rect, new Color(Color.White, this.currentFade));
        }

        public override void Update(GameTime gameTime)
        {
            if (this.fadeOut)
            {
                this.currentFade += frameFade;
            } 
            else
            {
                this.currentFade -= frameFade;
            }
            this.currentFade = Math.Clamp(this.currentFade, 0f, 1f);
            if (this.currentFade == 1f || this.currentFade == 0f)
            {
                this.completed = true;
                return;
            }
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
            // there is none, its a delay, duh...
        }

        public override void Update(GameTime gameTime)
        {
            timer.updateTime(gameTime.ElapsedGameTime.Milliseconds);
            if (timer.isTimeMet(runtime))
            {
                this.completed = true;
            }
        }
    }

    public class GSTLoad : GameStateTransition
    {
        private GameStateEnum gsEnum;
        private bool ran;
        private Rectangle rect;
        private Texture2D texture;
        public GSTLoad(int runtime, GameStateEnum gsEnum) : base(runtime)
        {
            this.GSTEnum = GSTEnum.LOAD;
            this.gsEnum = gsEnum;
            this.ran = false;
            this.rect = new Rectangle(0, 0, 1080, 720);
            this.texture = Game._cache.TextureCache["BlackPixel"];
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.ran)
            {
                Game.transitionTo = this.gsEnum;
                this.ran = true;
            }
        }
    }
}
