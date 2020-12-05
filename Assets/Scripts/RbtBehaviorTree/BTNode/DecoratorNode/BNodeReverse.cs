// ******************************************************************
//       /\ /|       @file       BNodeReverse.cs
//       \ V/        @brief      反转节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 18:17:49
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeReverse : BNodeDecorator
    {
        protected override void OnRunning(BDataBase bData)
        {
            base.OnRunning(bData);
            var node = _listChildNodes[0];
            var actionResult = node.UpdateNode(bData);

            if (actionResult == ActionResult.Success)
            {
                _actionResult = ActionResult.Failure;
                return;
            }

            if (actionResult == ActionResult.Failure)
            {
                _actionResult = ActionResult.Success;
                return;
            }

            _actionResult = ActionResult.Running;
        }
    }
}