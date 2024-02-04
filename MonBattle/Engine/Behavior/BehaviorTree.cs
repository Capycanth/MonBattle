namespace MonBattle.Engine.Behavior
{
    public class BehaviorTree
    {
        private IBehaviorNode root;

        public BehaviorTree(IBehaviorNode root)
        {
            this.root = root;
        }

        public void Execute()
        {
            root.Execute()();
        }
    }
}
