// ******************************************************************
//       /\ /|       @file       BNodeDecorator.cs
//       \ V/        @brief      行为树装饰节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:10:25
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeDecorator : BNodeBase
    {
        public override BNodeType NodeType => BNodeType.Decorator;

        protected override void OnEnter(BDataBase bData)
        {
            base.OnEnter(bData);
            if (_listChildNodes.Count != 1)
            {
                Debug.LogError("装饰节点必须拥有并且只有1个子节点");
            }
        }
    }
}