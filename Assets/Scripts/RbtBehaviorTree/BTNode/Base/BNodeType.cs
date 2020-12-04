// ******************************************************************
//       /\ /|       @file       BNodeType.cs
//       \ V/        @brief      节点类型
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-04 13:20:37
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace SR.RbtBehaviorTree
{
    public enum BNodeType
    {
        None = 0,
        Composite = 1, //组合
        Decorator = 2, //装饰
        Condition = 3, //条件
        Action = 4 //动作
    }
}