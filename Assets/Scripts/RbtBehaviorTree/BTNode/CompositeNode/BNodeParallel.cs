// ******************************************************************
//       /\ /|       @file       BNodeParallel.cs
//       \ V/        @brief      并行节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 19:40:09
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;

namespace SR.RbtBehaviorTree
{
    public class BNodeParallel : BNodeComposite
    {
        private int _finishedNodeCount = 0; //已完成的子节点数量
        private readonly Dictionary<int, bool> _mapNodeIndexToComplete = new Dictionary<int, bool>(); //index对应的子节点是否执行完毕

        protected override void OnEnter(ref BDataBase bData)
        {
            base.OnEnter(ref bData);
            _finishedNodeCount = 0;
            //将所有子节点标记为未完成
            _mapNodeIndexToComplete.Clear();
            for (var i = 0; i < listChildNodes.Count; i++)
            {
                _mapNodeIndexToComplete.Add(i, false);
            }
        }

        protected override ActionResult OnRunning(ref BDataBase bData)
        {
            //全部节点执行完毕
            if (_finishedNodeCount >= listChildNodes.Count)
            {
                return ActionResult.Success;
            }

            //尝试执行每一个子节点
            for (var i = 0; i < listChildNodes.Count; i++)
            {
                //这个节点已经执行完毕 跳过
                if (_mapNodeIndexToComplete[i])
                {
                    continue;
                }

                //当前子节点的执行结果
                var actionResult = listChildNodes[i].UpdateNode(ref bData);
                //当前节点执行中 跳过
                if (actionResult == ActionResult.Running) continue;
                //当前子节点执行完毕
                _finishedNodeCount++;
                _mapNodeIndexToComplete[i] = true;
            }

            return ActionResult.Running;
        }
    }
}