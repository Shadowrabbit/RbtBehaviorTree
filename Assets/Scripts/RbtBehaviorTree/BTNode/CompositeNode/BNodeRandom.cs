﻿// ******************************************************************
//       /\ /|       @file       BNodeRandom.cs
//       \ V/        @brief      随机节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 19:40:28
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using RbtBehaviorTree.BTData;
using RbtBehaviorTree.BTNode.Base;
using UnityEngine;

namespace RbtBehaviorTree.BTNode.CompositeNode
{
    public class BNodeRandom : BNodeComposite
    {
        private int _runningIndex = 0; //正在运行中的子节点索引

        protected override void OnEnter(ref BDataBase bData)
        {
            base.OnEnter(ref bData);
            //随机选个子节点
            _runningIndex = Random.Range(0, this._listChilds.Count);
        }

        protected override ActionResult OnRunning(ref BDataBase bData)
        {
            return _listChilds[_runningIndex].UpdateNode(ref bData);
        }
    }
}