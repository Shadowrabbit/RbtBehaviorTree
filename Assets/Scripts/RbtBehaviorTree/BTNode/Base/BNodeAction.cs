// ******************************************************************
//       /\ /|       @file       BNodeAction.cs
//       \ V/        @brief      行为树动作节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:05:56
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace SR.RbtBehaviorTree
{
    public class BNodeAction : BNodeBase
    {
        public override BNodeType NodeType => BNodeType.Action;
    }
}