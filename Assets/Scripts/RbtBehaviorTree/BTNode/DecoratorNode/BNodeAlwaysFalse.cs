// ******************************************************************
//       /\ /|       @file       BNodeAlwaysFalse.cs
//       \ V/        @brief      永假节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                        
//      /  \\        @Modified   2020-12-05 10:59:23
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace SR.RbtBehaviorTree
{
    public class BNodeAlwaysFalse : BNodeDecorator
    {
        protected override void OnEnter(BDataBase bData)
        {
            _actionResult = ActionResult.Failure;
            var node = _listChildNodes[0];
            var actionResult = node.UpdateNode(bData);
            _actionResult = actionResult == ActionResult.Running ? ActionResult.Running : ActionResult.Failure;
        }
    }
}