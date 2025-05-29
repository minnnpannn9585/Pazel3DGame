using System;

namespace BTFrame
{
    /*
     * Action node of behaviour tree
     */
    public class ActionNode : BTNode
    {
        private readonly Action _action;

        public ActionNode(Action action)
        {
            this._action = action;
        }
        
        /*
         * Execute action
         */
        public override bool Evaluate()
        {
            _action();
            return true;
        }
    }
}