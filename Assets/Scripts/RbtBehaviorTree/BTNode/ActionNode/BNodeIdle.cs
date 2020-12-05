// ******************************************************************
//       /\ /|       @file       BNodeIdle.cs
//       \ V/        @brief      空闲状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 11:48:00
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeIdle : BNodeAction
    {
        protected override void OnEnter(BDataBase bData)
        {
            base.OnEnter(bData);
            Debug.Log("这个单位处于空闲状态");
            _actionResult = ActionResult.Success;
        }
    }
}