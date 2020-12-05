// ******************************************************************
//       /\ /|       @file       BTree.cs
//       \ V/        @brief      行为树
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:03:06
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using _3rd.LitJson;
using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BTree : IJson
    {
        public string name;
        public BNodeBase rootNode;

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            rootNode = null;
        }

        /// <summary>
        /// 更变根节点
        /// </summary>
        public void ReplaceRootNode(BNodeBase node)
        {
            if (node == null)
            {
                return;
            }

            if (!node.GetType().IsSubclassOf(typeof(BNodeComposite)))
            {
                Debug.LogError("根节点必须为组合节点！");
                return;
            }

            node.ListChildNodes = rootNode.ListChildNodes;
            rootNode = null;
            for (var i = 0; i < node.ListChildNodes.Count; i++)
            {
                node.ListChildNodes[i].Parent = node;
            }

            rootNode = node;
        }

        /// <summary>
        /// 从树中移除节点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(BNodeBase node)
        {
            if (node == null)
            {
                return;
            }

            //根节点不存在
            if (rootNode == null)
            {
                return;
            }

            //不是这棵树上的节点
            if (!rootNode.IsChildNode(node))
            {
                Debug.LogError("移除节点失败 树:" + name + "中不存在节点:" + node.GetType().Name);
                return;
            }

            //被删除的是根节点
            if (node.Parent == null)
            {
                rootNode = null;
                return;
            }

            //子节点
            node.Parent.RemoveNode(node);
            node.Parent = null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="bData"></param>
        public void Update(BDataBase bData)
        {
            rootNode?.UpdateNode(bData);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public JsonData WriteJson()
        {
            var jsonData = new JsonData {["name"] = name, ["rootNode"] = rootNode?.WriteJson()};
            jsonData["rootNode"].SetJsonType(JsonType.Object);
            return jsonData;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="jsonData"></param>
        public void ReadJson(JsonData jsonData)
        {
            name = jsonData["name"].ToString();
            //根节点不为空
            if (!jsonData.Keys.Contains("rootNode")) return;
            var typeName = jsonData["rootNode"]["typeName"].ToString();
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Debug.LogError("class类型错误 type=" + typeName);
                return;
            }

            rootNode = Activator.CreateInstance(type) as BNodeBase;
            rootNode?.ReadJson(jsonData["rootNode"]);
        }
    }
}