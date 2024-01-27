using Capybara_1.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Capybara_1.Config.Internal
{
    public static class LoaderPackage
    {
        public static List<CameraAnimation> BattleOpening = new List<CameraAnimation>()
        {
            new CameraAnimation(new Vector2(50, 0), 1.2f, float.NaN, 30),
            new CameraAnimation(new Vector2(300, -200), 3f, float.NaN, 30),
            new CameraAnimation(new Vector2(0, 400), 3f, float.NaN, 60),
            new CameraAnimation(new Vector2(-300, -200), 1.15f, float.NaN, 30),
            new CameraAnimation(new Vector2(-100, 0), 1.15f, float.NaN, 45),
            new CameraAnimation(new Vector2(-300, -200), 3f, float.NaN, 30),
            new CameraAnimation(new Vector2(0, 400), 3f, float.NaN, 60),
            new CameraAnimation(new Vector2(350, -200), 1.15f, float.NaN, 30),
            new CameraAnimation(Vector2.Zero, 1.1f, float.NaN, 60),
            new CameraAnimation(Vector2.Zero, 1.0f, float.NaN, 15),
        };

        public static List<GameStateTransition> DefaultGST = new List<GameStateTransition>()
        {
                new GSTDelay(20),
                new GSTBlackFade(15, true),
                new GSTBlackFade(15, false),
        };
    }
}
