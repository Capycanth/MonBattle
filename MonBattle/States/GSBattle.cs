using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonBattle.Config.Internal;
using MonBattle.Engine;
using MonBattle.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonBattle.States
{
    public class GSBattle : IGameState
    {
        private GameStateEnum gsEnum;
        private InputManager inputManager;
        private List<DrawableBase> drawables;

        public GameStateEnum GSEnum { get => gsEnum; set => gsEnum = value; }
        public InputManager InputManager { get => inputManager; set => inputManager = value; }
        public List<DrawableBase> Drawables { get => drawables; set => drawables = value; }

        public void Update(GameTime gameTime)
        {
            foreach (DrawableBase drawable in drawables)
            {
                drawable.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (DrawableBase drawable in drawables)
            {
                _spriteBatch.Draw(drawable.Texture, drawable.Destination, Color.White);
            }
        }

        public void Initialize()
        {
            gsEnum = GameStateEnum.BATTLE;
            inputManager = new InputManager();
            inputManager.KeyBindings = new Dictionary<Keys, InputEnum>()
            {

            };
            drawables = new List<DrawableBase>(17);
            drawables.Add(new StaticSprite(Game._cache.TextureCache["Battle_MainBackground"], new Rectangle(0, 0, 1080, 720), new Rectangle(0, 0, 1080, 720)));
            for (int i = 0; i < 17; i++)
            {
                //drawables.Add()
            }
            
            //MediaPlayer.Play(Game._cache.SongCache["Battle_MainThemeMusic"]);
            //Game._camera.AddAnimations(LoaderPackage.BattleOpening);
        }

        public async Task LoadContent(ContentManager _contentManager)
        {
            await Task.Run(() =>
            {
                Game._cache.addTextures(new Dictionary<string, Texture2D>()
                {
                    { "BlackPixel", _contentManager.Load<Texture2D>("Graphics/BlackPixel") },
                    { "RedTriangle", _contentManager.Load<Texture2D>("Graphics/RedTriangle") },
                    { "BlueTriangle", _contentManager.Load<Texture2D>("Graphics/BlueTriangle") },
                    { "Battle_MainBackground", _contentManager.Load<Texture2D>("Graphics/Battle_MainBackground") },
                });
                    Game._cache.addSongs(new Dictionary<string, Song>()
                {
                    { "Battle_MainThemeMusic", _contentManager.Load<Song>("Music/DeathAndAxes") }
                });
            });
        }
    }
}
