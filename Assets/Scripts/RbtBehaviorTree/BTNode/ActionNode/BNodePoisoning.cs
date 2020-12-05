// ******************************************************************
//       /\ /|       @file       BNodePoisoning.cs
//       \ V/        @brief      中毒节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 11:51:51
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodePoisoning : BNodeAction
    {
        protected override void OnEnter(BDataBase bData)
        {
            base.OnEnter(bData);
            Debug.Log("这个单位进入中毒状态");
        }

        protected override void OnRunning(BDataBase bData)
        {
            bData.currentHpValue -= 20;
            Debug.Log("减少20点生命 当前生命值:" + bData.currentHpValue);
            _actionResult = ActionResult.Success;
        }
    }
}