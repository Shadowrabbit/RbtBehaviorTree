// ******************************************************************
//       /\ /|       @file       BNodeCondition.cs
//       \ V/        @brief      行为树条件节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:09:47
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace RbtBehaviorTree.BTNode.Base
{
    public class BNodeCondition : BNodeBase
    {
        public override BNodeType NodeType => BNodeType.Condition;
    }
}