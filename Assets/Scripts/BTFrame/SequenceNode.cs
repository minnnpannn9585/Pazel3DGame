using System.Collections.Generic;

namespace BTFrame
{
    /*
     * Sequence node of behaviour tree
     */
    public class SequenceNode : BTNode
    {
        public readonly List<BTNode> Children = new();
        
        /*
         * Evaluate each child nodes in list
         */
        public override bool Evaluate()
        {
            foreach (var child in Children)
            {
                if (!child.Evaluate())
                {
                    return false;
                }
            }
            return true;
        }
    }
}