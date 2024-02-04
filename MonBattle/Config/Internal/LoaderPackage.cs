using MonBattle.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonBattle.States;
using System.Reflection.Metadata.Ecma335;

namespace MonBattle.Config.Internal
{
    public static class LoaderPackage
    {
        public static List<CameraAnimation> BattleOpening = new List<CameraAnimation>()
        {
            new CameraAnimation(Vector2.Zero, float.NaN, float.NaN, 1000),
            new CameraAnimation(new Vector2(50, 0), 1.2f, float.NaN, 1000),
            new CameraAnimation(new Vector2(300, -200), 3f, float.NaN, 1000),
            new CameraAnimation(new Vector2(0, 400), 3f, float.NaN, 2000),
            new CameraAnimation(new Vector2(-300, -200), 1.15f, float.NaN, 1000),
            new CameraAnimation(new Vector2(-100, 0), 1.15f, float.NaN, 1000),
            new CameraAnimation(new Vector2(-300, -200), 3f, float.NaN, 1000),
            new CameraAnimation(new Vector2(0, 400), 3f, float.NaN, 2000),
            new CameraAnimation(new Vector2(350, -200), 1.15f, float.NaN, 1000),
            new CameraAnimation(Vector2.Zero, 1.1f, float.NaN, 2000),
            new CameraAnimation(Vector2.Zero, 1.0f, float.NaN, 200),
        };

        public static List<GameStateTransition> GSTHomeOut(IGameState gameState)
        {
            return new List<GameStateTransition>()
            {
                new GSTDelay(500),
                new GSTBlackFade(1000, true),
                new GSTLoad(0, gameState),
            };
        }

        public static List<GameStateTransition> GSTBattleIn = new List<GameStateTransition>()
        {
            new GSTBlackFade(1000, false),
        };

    }
}
