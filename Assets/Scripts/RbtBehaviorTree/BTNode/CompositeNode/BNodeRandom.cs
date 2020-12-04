// ******************************************************************
//       /\ /|       @file       BNodeRandom.cs
//       \ V/        @brief      随机节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 19:40:28
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeRandom : BNodeComposite
    {
        private int _runningIndex = 0; //正在运行中的子节点索引

        protected override void OnEnter(BDataBase bData)
        {
            base.OnEnter(bData);
            //随机选个子节点
            _runningIndex = Random.Range(0, _listChildNodes.Count);
        }

        protected override ActionResult OnRunning(BDataBase bData)
        {
            return _listChildNodes[_runningIndex].UpdateNode(bData);
        }
    }
}