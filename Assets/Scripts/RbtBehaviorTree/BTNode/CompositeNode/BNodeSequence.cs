﻿// ******************************************************************
//       /\ /|       @file       BNodeSequence.cs
//       \ V/        @brief      队列节点 从左到右执行直到某个节点返回false
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 19:40:58
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using RbtBehaviorTree.BTData;
using RbtBehaviorTree.BTNode.Base;

namespace RbtBehaviorTree.BTNode.CompositeNode
{
    public class BNodeSequence : BNodeComposite
    {
        private int _finishedNodeCount = 0; //已完成的节点数量

        /// <summary>
        /// 进入时回调
        /// </summary>
        /// <param name="bData"></param>
        protected override void OnEnter(ref BDataBase bData)
        {
            base.OnEnter(ref bData);
            _finishedNodeCount = 0;
        }

        /// <summary>
        /// 运行中回调
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        protected override ActionResult OnRunning(ref BDataBase bData)
        {
            //完成了队列中的全部节点
            if (_finishedNodeCount >= _listChilds.Count)
            {
                return ActionResult.Success;
            }

            //当前队列中正在运行的节点
            var node = _listChilds[_finishedNodeCount];
            //执行该节点动作
            var actionResult = node.UpdateNode(ref bData);
            //某个节点失败则本队列失败
            if (actionResult == ActionResult.Failure)
            {
                return actionResult;
            }

            //某个节点成功 index++
            if (actionResult == ActionResult.Success)
            {
                _finishedNodeCount++;
            }

            return ActionResult.Running;
        }
    }
}