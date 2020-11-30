// ******************************************************************
//       /\ /|       @file       BTreeMainWindowController.cs
//       \ V/        @brief      主页面控制器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 14:07:38
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using RbtBehaviorTree;
using RbtBehaviorTree.BTNode.Base;

namespace Editor.EditorRbtBehaviorTree
{
    public class BTreeMainWindowController
    {
        private BTreeMainWindow _view; //视图
        private BTree _model; //数据模型
        public BTree Model => _model;
        public int rootNodeTypeIndex = -1; //根节点类型索引
        public BNodeBase selectedNode; //选中的节点
        public BNodeBase targetNode; //目标节点

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="view"></param>
        public BTreeMainWindowController(BTreeMainWindow view)
        {
            //视图绑定
            _view = view;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="model"></param>
        public void SetModel(BTree model)
        {
            _model = model;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
        }

        /// <summary>
        /// 加载
        /// </summary>
        public void Load()
        {
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void Create()
        {
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="arg"></param>
        public void AddNode(object arg)
        {
            if (selectedNode == null)
            {
                return;
            }

            var type = arg as Type;
            var node = BNodeFactory.Instance.Create(type);
            selectedNode.AddNode(ref node);
            node.parent = selectedNode;
            _view.Repaint();
        }

        /// <summary>
        /// 替换节点
        /// </summary>
        /// <param name="arg"></param>
        public void ReplaceNode(object arg)
        {
            if (selectedNode == null)
            {
                return;
            }

            var type = arg as Type;
            var node = BNodeFactory.Instance.Create(type);
            node.parent = selectedNode.parent;
            node.listChildNodes = selectedNode.listChildNodes;
            //更新父节点信息
            if (node.parent != null)
            {
                node.parent.ReplaceNode(selectedNode, ref node);
            }
            else
            {
                _model.rootNode = node;
            }

            selectedNode = node;
            _view.Repaint();
        }

        /// <summary>
        /// 撤销节点
        /// </summary>
        /// <param name="arg"></param>
        public void RemoveNode(object arg)
        {
            if (selectedNode == null)
            {
                return;
            }

            selectedNode.parent?.RemoveNode(ref selectedNode);
            selectedNode.parent = null;
            selectedNode = null;
            targetNode = null;
            _view.Repaint();
        }
    }
}