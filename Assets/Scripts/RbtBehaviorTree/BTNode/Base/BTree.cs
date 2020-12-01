// ******************************************************************
//       /\ /|       @file       BTree.cs
//       \ V/        @brief      行为树
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:03:06
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BTree
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
            if (node.NodeType != BNodeType.Composite)
            {
                Debug.LogError("根节点必须为组合节点！");
                return;
            }

            node.listChildNodes = rootNode.listChildNodes;
            rootNode = null;
            for (var i = 0; i < node.listChildNodes.Count; i++)
            {
                node.listChildNodes[i].parent = node;
            }

            rootNode = node;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="bData"></param>
        public void Update(ref BDataBase bData)
        {
            rootNode.UpdateNode(ref bData);
        }
    }
}