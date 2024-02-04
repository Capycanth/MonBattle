using MonBattle.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Threading.Tasks;
using System.Diagnostics;

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

        private Queue<IGameState> stateStack = new Queue<IGameState>();
        private Queue<GameStateTransition> GSTs = new Queue<GameStateTransition>();
        private ContentManager _contentManager;
        private bool popState = false;

        public void Update(GameTime gameTime)
        {
            if (popState)
            {
                if (stateStack.Count > 1)
                {
                    DestroyState(stateStack.Dequeue());
                }
                popState = false;
                stateStack.Peek().Begin();
            }

            if (GSTs.Count == 0 && stateStack.Count > 0)
            {
                stateStack.Peek().HandleInput();
                stateStack.Peek().Update(gameTime);
            }
            else if (GSTs.Count > 0)
            {
                if (GSTs.Peek().IsCompleted)
                {
                    GSTs.Dequeue();
                    return;
                }
                switch (GSTs.Peek().GSTEnum)
                {
                    case GSTEnum.DELAY:
                        GSTs.Peek().Update(gameTime);
                        break;
                    case GSTEnum.BLACK_FADE:
                        GSTs.Peek().Update(gameTime);
                        break;
                    case GSTEnum.LOAD:
                        GSTs.Peek().Update(gameTime);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GSTs.Count == 0 && stateStack.Count > 0)
            {
                stateStack.Peek().Draw(spriteBatch);
            }
            else if (GSTs.Count > 0)
            {
                switch (GSTs.Peek().GSTEnum)
                {
                    case GSTEnum.DELAY:
                        stateStack.Peek().Draw(spriteBatch);
                        break;
                    case GSTEnum.BLACK_FADE:
                        stateStack.Peek().Draw(spriteBatch);
                        GSTs.Peek().Draw(spriteBatch);
                        break;
                    case GSTEnum.LOAD:
                        GSTs.Peek().Draw(spriteBatch);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public async Task PushToStackAndInitialize(IGameState newGameState, Action completeAction)
        {
            await Task.Run(() =>
            {
                newGameState.LoadContent(_contentManager);
                newGameState.Initialize();
                stateStack.Enqueue(newGameState);
                this.popState = true;
                completeAction();
            });
        }

        public async Task DestroyState(IGameState state)
        {
            await Task.Run(() =>
            {
                state.UnloadContent();
            });
        }

        public void PushGSTransitions(List<GameStateTransition> GSTs)
        {
            foreach(GameStateTransition gst in GSTs)
            {
                this.GSTs.Enqueue(gst);
            }
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
        public bool IsCompleted { get { return completed; } }
    }

    public class GSTBlackFade : GameStateTransition
    {
        private Rectangle rect;
        private Texture2D texture;
        private Color color;
        private float frameFade;
        private float currentFade;

        public GSTBlackFade(int runtime, bool fadeOut) : base(runtime)
        {
            this.gstEnum = GSTEnum.BLACK_FADE;
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
            this.color = new Color(Color.White, this.currentFade);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.texture, this.rect, this.color);
        }

        public override void Update(GameTime gameTime)
        {
            this.currentFade += (this.frameFade * gameTime.ElapsedGameTime.Milliseconds);
            this.currentFade = Math.Clamp(currentFade, 0f, 1f);
            if (this.currentFade == 1f || this.currentFade == 0f)
            {
                this.completed = true;
                return;
            }
            this.color = new Color(Color.White, this.currentFade);
        }
    }

    public class GSTDelay : GameStateTransition
    {
        public GSTDelay(int runtime) : base(runtime)
        {
            this.gstEnum = GSTEnum.DELAY;
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

    public class GSTLoad : GameStateTransition
    {
        private IGameState toState;
        private Rectangle rect;
        private Texture2D texture;
        private bool waiting = false;
        public GSTLoad(int runtime, IGameState toState) : base(runtime)
        {
            this.toState = toState;
            this.gstEnum = GSTEnum.LOAD;
            rect = new Rectangle(0, 0, 1080, 720);
            texture = Game._cache.TextureCache["BlackPixel"];
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.waiting)
            {
                return;
            }
            Game._gameStateManager.PushToStackAndInitialize(this.toState, () => { this.completed = true; });
            this.waiting = true;
        }
    }
}
