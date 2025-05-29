namespace BTFrame
{
    /*
     * Behaviour tree
     */
    public class BehaviorTree
    {
        public BTNode RootNode;

        /*
         * Update behaviour tree
         */
        public void BTUpdate()
        {
            RootNode.Evaluate();
        }
    }
}