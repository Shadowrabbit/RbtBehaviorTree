// ******************************************************************
//       /\ /|       @file       TexLineFactory.cs
//       \ V/        @brief      Tex工厂
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 13:30:43
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using Base;
using UnityEngine;

namespace Editor.EditorRbtBehaviorTree
{
    public class TexLineFactory : SingletonBase<TexLineFactory>
    {
        public Texture2D Create(Color color)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }
    }
}