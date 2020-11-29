// ******************************************************************
//       /\ /|       @file       BNodeDecorator.cs
//       \ V/        @brief      行为树装饰节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:10:25
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace RbtBehaviorTree.BTNode.Base
{
    public class BNodeDecorator : BNodeBase
    {
        public override BNodeType NodeType => BNodeType.Decorator;
    }
}