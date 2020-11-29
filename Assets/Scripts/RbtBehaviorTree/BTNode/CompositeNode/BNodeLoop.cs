// ******************************************************************
//       /\ /|       @file       BNodeLoop.cs
//       \ V/        @brief      循环节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 20:19:07
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using RbtBehaviorTree.BTNode.Base;

namespace RbtBehaviorTree.BTNode.CompositeNode
{
    public class BNodeLoop : BNodeComposite
    {
        //无限次
        private static readonly int INFINITE = -1;
        private int _currentNodeIndex; //当前执行的节点索引
        private int _finishedNodeCount = 0; //已执行的节点数
        public int maxLoopTimes; //最大循环次数
    }
}