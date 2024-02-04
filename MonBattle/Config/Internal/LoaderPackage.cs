using MonBattle.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonBattle.States;
using System.Reflection.Metadata.Ecma335;
using MonBattle.Engine.Behavior;
using MonBattle.Entity;

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

        public static IBehaviorNode BasicBehavior(MonCreature mon)
        {
            return new ConditionNode(
                () =>
                {
                    if (mon.shouldAttack)
                    {
                        return ConditionReturnEnum.ATTACK;
                    } 
                    else if (mon.shouldDefend)
                    {
                        return ConditionReturnEnum.DEFEND;
                    } 
                    else if (mon.shouldWander)
                    {
                        return ConditionReturnEnum.WANDER;
                    }
                    else if (mon.shouldHeal)
                    {
                        return ConditionReturnEnum.HEAL;
                    }
                    else
                    {
                        return ConditionReturnEnum.FALSE;
                    }
                }, 
                new Dictionary<ConditionReturnEnum, IBehaviorNode>() 
                {
                    { 
                        ConditionReturnEnum.FALSE, 
                        new ActionNode(() => BehaviorAction.Wander(mon)) 
                    },
                    {
                        ConditionReturnEnum.DEFEND,
                        new ActionNode(() => BehaviorAction.Defend(mon))
                    },
                    {
                        ConditionReturnEnum.HEAL,
                        new ActionNode(() => BehaviorAction.Heal(mon))
                    },
                    {
                        ConditionReturnEnum.ATTACK,
                        new ActionNode(() => BehaviorAction.Attack(mon, mon.Target))
                    },
                    {
                        ConditionReturnEnum.WANDER,
                        new ActionNode(() => BehaviorAction.Wander(mon))
                    },
                });
        }

    }
}
