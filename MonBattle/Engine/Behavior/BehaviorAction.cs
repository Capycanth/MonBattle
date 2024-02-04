using MonBattle.Entity;
using System;
using Microsoft.Xna.Framework;

namespace MonBattle.Engine.Behavior
{
    public static class BehaviorAction
    {
        private static Random random = new Random();
        public static void Attack(MonCreature mon, MonCreature targetMon)
        {
            if (Vector2.Distance(mon.Location, targetMon.Location) <= mon.Reach)
            {
                mon.Attack(targetMon);
            }
            else
            {
                mon.Location += BehaviorHelper.MoveTo(mon.Location, targetMon.Location, mon.Speed);
            }
        }

        public static void Defend(MonCreature mon)
        {
            if (Vector2.Distance(mon.Location, mon.Target.Location) <= mon.Reach)
            {
                int focus = random.Next(mon.Target.TargetedBy.Count);
                Attack(mon, mon.Target.TargetedBy[focus]);
            }
            else
            {
                mon.Location += BehaviorHelper.MoveTo(mon.Location, mon.Target.Location, mon.Speed);
            }
        }

        public static void Heal(MonCreature mon)
        {
            if (null == mon.Target)
            {
                mon.HealSelf();
                return;
            }

            if (Vector2.Distance(mon.Location, mon.Target.Location) <= mon.Reach)
            {
                mon.HealMon(mon.Target);
            }
            else
            {
                mon.Location += BehaviorHelper.MoveTo(mon.Location, mon.Target.Location, mon.Speed);
            }
        }

        public static void Wander(MonCreature mon)
        {
            if (Vector2.Zero == mon.TargetLocation || Vector2.Distance(mon.Location, mon.TargetLocation) <= 10)
            {
                float newX = random.Next(200) - 100;
                float newY = random.Next(200) - 100;
                Vector2 newTarget = new Vector2(mon.Location.X - newX, mon.Location.Y - newY);
                mon.TargetLocation = newTarget;
                mon.CoolDownTimer.restartTimer(); // Simulate stopping for a few seconds
            }
            else
            {
                mon.Location += BehaviorHelper.MoveTo(mon.Location, mon.TargetLocation, mon.Speed);
            }
        }

        private static class BehaviorHelper
        {
            public static Vector2 MoveTo(Vector2 from, Vector2 to, float speed)
            {
                Vector2 movement = to - from;
                movement.X = Math.Clamp(movement.X, (-1) * speed, speed);
                movement.Y = Math.Clamp(movement.Y, (-1) * speed, speed);
                return movement;
            }
        }
    }
}
