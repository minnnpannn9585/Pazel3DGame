using System;

namespace BTFrame
{
    /*
     * Condition node of behaviour tree
     */
    public class ConditionNode : BTNode
    {
        private readonly Func<bool> _condition;

        public ConditionNode(Func<bool> condition)
        {
            this._condition = condition;
        }
        
        /*
         * Evaluate condition
         */
        public override bool Evaluate()
        {
            return _condition();
        }
    }
}