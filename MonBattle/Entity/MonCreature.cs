using MonBattle.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonBattle.Engine.Behavior;
using MonBattle.Config.Internal;

namespace MonBattle.Entity
{
    public class MonCreature : AnimatedSprite
    {
        private BehaviorTree behaviorTree;
        private MonCreature target;
        private Vector2 location;
        private Vector2 targetLocation;
        private float reach;
        private float speed;
        private List<MonCreature> targetedBy;
        private Timer coolDownTimer = new Timer();

        // Temporary test variables
        public bool shouldAttack = false;
        public bool shouldHeal = false;
        public bool shouldDefend = false;
        public bool shouldWander = true;

        public Vector2 Location { get { return location; } set { location = value; } }
        public MonCreature Target { get { return target; } set { target = value; } }
        public float Reach { get { return reach; } set { reach = value; } }
        public float Speed { get { return speed; } set { speed = value; } }
        public List<MonCreature> TargetedBy { get { return targetedBy; } set { this.targetedBy = value; } }
        public Vector2 TargetLocation { get { return targetLocation; } set { targetLocation = value; } }
        public Timer CoolDownTimer { get { return coolDownTimer; } set { coolDownTimer = value; } }

        public MonCreature()
        {
            behaviorTree = new BehaviorTree(LoaderPackage.BasicBehavior(this));
        }

        public new void Update(GameTime gameTime)
        {
            coolDownTimer.updateTime(gameTime.ElapsedGameTime.Milliseconds);
            if (coolDownTimer.isTimeMet(1000))
            {
                behaviorTree.Execute();
            }
        }

        public void Attack(MonCreature targetMon)
        {
            coolDownTimer.restartTimer();
        }

        public void HealSelf()
        {
            coolDownTimer.restartTimer();
        }

        public void HealMon(MonCreature targetMon)
        {
            coolDownTimer.restartTimer();
        }
    }
}
