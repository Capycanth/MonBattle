using MonBattle.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonBattle.States;

namespace MonBattle.Config.Internal
{
    public static class LoaderPackage
    {
        public static List<CameraAnimation> BattleOpening = new List<CameraAnimation>()
        {
            new CameraAnimation(new Vector2(50, 0), 1.2f, float.NaN, 60),
            new CameraAnimation(new Vector2(300, -200), 3f, float.NaN, 60),
            new CameraAnimation(new Vector2(0, 400), 3f, float.NaN, 120),
            new CameraAnimation(new Vector2(-300, -200), 1.15f, float.NaN, 60),
            new CameraAnimation(new Vector2(-100, 0), 1.15f, float.NaN, 90),
            new CameraAnimation(new Vector2(-300, -200), 3f, float.NaN, 60),
            new CameraAnimation(new Vector2(0, 400), 3f, float.NaN, 120),
            new CameraAnimation(new Vector2(350, -200), 1.15f, float.NaN, 60),
            new CameraAnimation(Vector2.Zero, 1.1f, float.NaN, 120),
            new CameraAnimation(Vector2.Zero, 1.0f, float.NaN, 30),
        };

        public static List<GameStateTransition> DefaultGST(GameStateEnum gsEnum)
        {
            return new List<GameStateTransition>()
            {
                new GSTDelay(500),
                new GSTBlackFade(60, true),
                new GSTLoad(0, gsEnum),
                new GSTBlackFade(500, false),
            };
        }

    }
}
