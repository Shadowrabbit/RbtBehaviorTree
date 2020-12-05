// ******************************************************************
//       /\ /|       @file       BNodeIsHpMoreThan.cs
//       \ V/        @brief      生命大于节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 21:19:35
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace SR.RbtBehaviorTree
{
    public class BNodeIsHpMoreThan : BNodeCondition
    {
        public float hpPercent; //生命百分比

        protected override void OnRunning(BDataBase bData)
        {
            var currentHpPercent = (float) bData.currentHpValue / bData.maxHpValue;
            _actionResult = currentHpPercent > hpPercent ? ActionResult.Success : ActionResult.Failure;
        }
    }
}