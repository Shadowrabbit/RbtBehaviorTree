// ******************************************************************
//       /\ /|       @file       BNodeAlwaysTrue.cs
//       \ V/        @brief      永真节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 10:56:38
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeAlwaysTrue : BNodeDecorator
    {
        protected override void OnRunning(BDataBase bData)
        {
            base.OnRunning(bData);
            var node = _listChildNodes[0];
            var actionResult = node.UpdateNode(bData);
            _actionResult = actionResult == ActionResult.Running ? ActionResult.Running : ActionResult.Success;
        }
    }
}