// ******************************************************************
//       /\ /|       @file       ActionResult.cs.cs
//       \ V/        @brief      节点执行结果
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 15:55:24
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace SR.RbtBehaviorTree
{
    public enum ActionResult
    {
        Idle = 0, //空闲
        Success = 1, //成功    
        Running = 2, //运行中
        Failure = 3, //失败
    }
}