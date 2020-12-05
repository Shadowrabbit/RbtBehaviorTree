// ******************************************************************
//       /\ /|       @file       BNodeRestore.cs
//       \ V/        @brief      生命恢复节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 11:57:52
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeRestore : BNodeAction
    {
        protected override void OnEnter(BDataBase bData)
        {
            base.OnEnter(bData);
            Debug.Log("这个单位尝试使用治疗药剂恢复生命");
        }

        protected override void OnRunning(BDataBase bData)
        {
            bData.currentHpValue += 10;
            Debug.Log("生命值恢复了10点 当前生命值:" + bData.currentHpValue);
            _actionResult = ActionResult.Success;
        }
    }
}