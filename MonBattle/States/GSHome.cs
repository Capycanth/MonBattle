using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonBattle.Config.Internal;
using MonBattle.Engine;
using MonBattle.Entity;
using System.Collections.Generic;

namespace MonBattle.States
{
    public class GSHome : IGameState
    {
        private GameStateEnum gsEnum;
        private InputManager inputManager;
        private List<DrawableBase> drawables;

        public GameStateEnum GSEnum { get => gsEnum; set => gsEnum = value; }
        public InputManager InputManager { get => inputManager; set => inputManager = value; }
        public List<DrawableBase> Drawables { get => drawables; set => drawables = value; }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (DrawableBase drawable in drawables)
            {
                _spriteBatch.Draw(drawable.Texture, drawable.Destination, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            inputManager.Update();
            if (inputManager.Pressed(InputEnum.ENTER))
            {
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Stop();
                Game._cache.SoundEffectCache["Home_StartEffect"].Play();
                Game._gameStateManager.PushGSTransitions(LoaderPackage.GSTHomeOut(new GSBattle()));
            }
        }

        public void Initialize()
        {
            gsEnum = GameStateEnum.HOME;
            inputManager = new InputManager();
            inputManager.KeyBindings = new Dictionary<Keys, InputEnum>()
            {
                { Keys.Enter, InputEnum.ENTER },
            };
            // TODO: Screen Size Height/Width
            drawables = new List<DrawableBase>(1);
            drawables.Add(new StaticSprite(Game._cache.TextureCache["Home_MainBackground"], new Rectangle(0, 0, 1080, 720), new Rectangle(0, 0, 1080, 720)));
        }

        public void LoadContent(ContentManager _contentManager)
        {
            Game._cache.addTextures(new Dictionary<string, Texture2D>()
            {
                { "Home_MainBackground", _contentManager.Load<Texture2D>("Graphics/Home_MainBackground") }
            });
            Game._cache.addSongs(new Dictionary<string, Song>()
            {
                { "Home_MainThemeMusic", _contentManager.Load<Song>("Music/MartianCowboy") }
            });
            Game._cache.addSoundEffects(new Dictionary<string, SoundEffect>()
            {
                { "Home_StartEffect", _contentManager.Load<SoundEffect>("SoundEffects/OilDrumSoftImpact") }
            });
        }

        public void Begin()
        {
            MediaPlayer.Play(Game._cache.SongCache["Home_MainThemeMusic"]);
            MediaPlayer.IsRepeating = true;
        }

        public void UnloadContent()
        {
            Game._cache.disposeTexture("Home_MainBackground");
            Game._cache.disposeSong("Home_MainThemeMusic");
            Game._cache.disposeSoundEffect("Home_StartEffect");
        }
    }
}
