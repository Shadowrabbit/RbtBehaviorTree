// ******************************************************************
//       /\ /|       @file       BNodeDie.cs
//       \ V/        @brief      死亡节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 11:43:34
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeDie : BNodeAction
    {
        protected override void OnEnter(BDataBase bData)
        {
            base.OnEnter(bData);
            Debug.Log("这个单位已经挂了 等待系统回收尸体");
        }
    }
}