using System;
using System.Collections.Generic;

namespace MonBattle.Engine.Behavior
{
    public enum ConditionReturnEnum
    {
        FALSE = 0,
        TRUE = 1,
        ATTACK = 2,
        DEFEND = 3,
        HEAL = 4,
        WANDER = 5,
    }

    public interface IBehaviorNode
    {
        Action Execute();
    }

    public class ConditionNode : IBehaviorNode
    {
        private Func<ConditionReturnEnum> _condition;
        private Dictionary<ConditionReturnEnum, IBehaviorNode> children;

        public ConditionNode(Func<ConditionReturnEnum> condition, Dictionary<ConditionReturnEnum, IBehaviorNode> children)
        {
            _condition = condition;
            this.children = children;
        }

        public Action Execute()
        {
            return children[_condition()].Execute();
        }
    }

    public class ActionNode : IBehaviorNode
    {
        private Action action;

        public ActionNode(Action action)
        {
            this.action = action;
        }

        public Action Execute()
        {
            return action;
        }
    }
}
