// ******************************************************************
//       /\ /|       @file       BNodeEscape.cs
//       \ V/        @brief      逃跑节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 12:01:31
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeEscape : BNodeAction
    {
        protected override void OnEnter(BDataBase bData)
        {
            base.OnEnter(bData);
            Debug.Log("这个单位逃跑了");
            _actionResult = ActionResult.Success;
        }
    }
}