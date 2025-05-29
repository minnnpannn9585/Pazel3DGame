using System.Collections.Generic;

namespace BTFrame
{
    /*
     * Selector node of behaviour tree
     */
    public class SelectorNode : BTNode
    {
        public readonly List<BTNode> Children = new();
        
        /*
         * Evaluate each child nodes in list
         */
        public override bool Evaluate()
        {
            foreach (var child in Children)
            {
                if (child.Evaluate())
                {
                    return true;
                }
            }

            return false;
        }
    }
}