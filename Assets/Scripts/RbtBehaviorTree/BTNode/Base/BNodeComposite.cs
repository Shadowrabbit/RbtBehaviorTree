// ******************************************************************
//       /\ /|       @file       BNodeComposite.cs
//       \ V/        @brief      行为树组合节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:09:11
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace SR.RbtBehaviorTree
{
    public class BNodeComposite : BNodeBase
    {
        public override BNodeType NodeType => BNodeType.Composite;
    }
}