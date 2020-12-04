// ******************************************************************
//       /\ /|       @file       BNodeFactory.cs
//       \ V/        @brief      节点工厂
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 18:54:26
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeFactory : SingletonBase<BNodeFactory>
    {
        public readonly List<Type> listCompositeTypes; //组合节点类型列表
        public readonly List<Type> listActionTypes; //动作节点类型列表
        public readonly List<Type> listConditionTypes; //条件节点类型列表
        public readonly List<Type> listDecoratorTypes; //装饰节点类型列表

        public BNodeFactory()
        {
            listCompositeTypes = GetSubClassList(typeof(BNodeComposite));
            listActionTypes = GetSubClassList(typeof(BNodeAction));
            listConditionTypes = GetSubClassList(typeof(BNodeCondition));
            listDecoratorTypes = GetSubClassList(typeof(BNodeDecorator));
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public BNodeBase Create(Type type)
        {
            return Activator.CreateInstance(type) as BNodeBase;
        }

        /// <summary>
        /// 创建根节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BNodeBase CreateRootNode(int index)
        {
            if (listCompositeTypes.Count <= index)
            {
                Debug.LogError("没有对应的组合节点 index=" + index);
                return null;
            }

            return Create(listCompositeTypes[index]);
        }

        /// <summary>
        /// 获取组合节点名称列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetCompositeNodeNameList()
        {
            return listCompositeTypes.Select(compositeType => compositeType.Name).ToList();
        }

        /// <summary>
        /// 获取组合节点的索引
        /// </summary>
        /// <returns></returns>
        public int GetCompositeNodeIndex(Type type)
        {
            for (var i = 0; i < listCompositeTypes.Count; i++)
            {
                if (type == listCompositeTypes[i])
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 反射获取子类的类型列表
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public List<Type> GetSubClassList(Type nodeType)
        {
            var listType = new List<Type>();
            var rootPath = Application.dataPath + "/";
            var listCsFileName = GetCSharpFileNameList(rootPath);
            foreach (var name in listCsFileName)
            {
                var className = name.Split('.')[0];
                var classType = Type.GetType("SR.RbtBehaviorTree." + className);
                //类型不存在 或者不是node的子类
                if (classType == null || !classType.IsSubclassOf(nodeType)) continue;
                listType.Add(classType);
                //Debug.Log(classType.Name);
            }

            return listType;
        }

        /// <summary>
        /// 获取CS文件名称列表
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns></returns>
        private List<string> GetCSharpFileNameList(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);
            //获取当前目录下的cs文件名称列表
            var listFileName = dirInfo.GetFiles("*.cs").Select(file => file.Name).ToList();
            var subDirs = Directory.GetDirectories(dir); //子目录名称
            foreach (var subDir in subDirs)
            {
                //把子目录下的文件列表添加到父目录列表中
                listFileName.AddRange(GetCSharpFileNameList(subDir));
            }

            return listFileName;
        }
    }
}