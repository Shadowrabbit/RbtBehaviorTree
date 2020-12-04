// ******************************************************************
//       /\ /|       @file       SingletonBase.cs
//       \ V/        @brief      单例基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 09:44:54
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

namespace SR
{
    public class SingletonBase<T> where T : class, new()
    {
        public static T Instance => SingletonInner.Instance;

        private class SingletonInner
        {
            internal static readonly T Instance = new T();
        }
    }
}