// ******************************************************************
//       /\ /|       @file       BNodeSelector.cs
//       \ V/        @brief      选择节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 19:40:42
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using RbtBehaviorTree.BTData;
using RbtBehaviorTree.BTNode.Base;

namespace RbtBehaviorTree.BTNode.CompositeNode
{
    public class BNodeSelector : BNodeComposite
    {
        private int _failedNodeCount = 0; //已失败的节点数量

        /// <summary>
        /// 进入时回调
        /// </summary>
        /// <param name="bData"></param>
        protected override void OnEnter(ref BDataBase bData)
        {
            base.OnEnter(ref bData);
            _failedNodeCount = 0;
        }

        /// <summary>
        /// 运行中回调
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        protected override ActionResult OnRunning(ref BDataBase bData)
        {
            //全部子节点失败 
            if (_failedNodeCount >= listChildNodes.Count)
            {
                return ActionResult.Failure;
            }

            //当前队列中正在运行的节点
            var node = listChildNodes[_failedNodeCount];
            //执行该节点动作
            var actionResult = node.UpdateNode(ref bData);
            //某个节点成功则选择节点成功
            if (actionResult == ActionResult.Success)
            {
                return actionResult;
            }

            //某个节点失败 index++
            if (actionResult == ActionResult.Failure)
            {
                _failedNodeCount++;
            }

            return ActionResult.Running;
        }
    }
}