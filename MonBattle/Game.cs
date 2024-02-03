using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonBattle.Engine;
using MonBattle.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonBattle
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Cache _cache;
        public static Camera _camera;
        public static GameStateManager _gameStateManager;
        public static GameStateEnum transitionTo;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.SynchronizeWithVerticalRetrace = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
        }

        protected override void Initialize()
        {
            _cache = Cache.GetCache();
            _cache.addTextures(new Dictionary<string, Texture2D>()
            {
                { "BlackPixel", Content.Load<Texture2D>("Graphics/BlackPixel") }
            });
            _camera = Camera.GetCamera();
            _gameStateManager = GameStateManager.GetGameStateManager(Content);
            _gameStateManager.SetGSTransitions(new List<GameStateTransition>() { new GSTLoad(0, GameStateEnum.HOME) });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _camera.Update(gameTime);
            _gameStateManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: _camera.TranslationMatrix);
            _gameStateManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}