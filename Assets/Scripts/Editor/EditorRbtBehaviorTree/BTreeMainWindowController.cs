// ******************************************************************
//       /\ /|       @file       BTreeMainWindowController.cs
//       \ V/        @brief      主页面控制器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 14:07:38
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BTreeMainWindowController
    {
        public BTree Model => _model;
        public int rootNodeTypeIndex = -1; //根节点类型索引
        public BNodeBase selectedNode; //选中的节点
        public bool isNodeInDragging; //正在移动节点
        private readonly BTreeMainWindow _view; //视图
        private BTree _model; //数据模型

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
            if (_model == null)
            {
                Debug.LogError("行为树不存在!");
                return;
            }

            _model.rootNode.listChildNodes = null;
        }

        /// <summary>
        /// 创建行为树
        /// </summary>
        public void Create()
        {
            var rootNode = BNodeFactory.Instance.Create(typeof(BNodeSequence));
            _model = new BTree {name = "New Tree", rootNode = rootNode};
            rootNodeTypeIndex = BNodeFactory.Instance.GetCompositeNodeIndex(typeof(BNodeSequence));
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
            selectedNode = node;
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

            //根节点
            if (selectedNode.parent == null)
            {
                _model.rootNode = null;
            }
            //子节点
            else
            {
                selectedNode.parent.RemoveNode(ref selectedNode);
                selectedNode.parent = null;
            }

            selectedNode = null;
            _view.Repaint();
        }

        /// <summary>
        /// 拖动检测
        /// </summary>
        public void CheckNodeMouseUp(ref int x, ref int y, ref BNodeBase node)
        {
            var evt = Event.current;
            if (evt.type != EventType.MouseUp) return;
            //节点插入识别范围
            var insertLineRect = new Rect(0, y, _view.position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH, 5);
            //节点挂载识别范围
            var nodeSelectableRect = new Rect(0, y, _view.position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH,
                DefEditorBTreeUI.NODE_HEIGHT - 5);
            //在插入识别范围内抬起
            if (insertLineRect.Contains(evt.mousePosition))
            {
                OnMouseUpInsertLine(ref node);
            }
            //在节点挂载识别范围内抬起
            else if (nodeSelectableRect.Contains(evt.mousePosition))
            {
                OnMouseUpNode(ref node);
            }

            isNodeInDragging = false;
            _view.Repaint();
        }

        /// <summary>
        /// 点击检测
        /// </summary>
        /// <param name="y"></param>
        /// <param name="node"></param>
        /// <param name="x"></param>
        public void CheckNodeMouseDown(ref int x, ref int y, ref BNodeBase node)
        {
            var evt = Event.current;
            if (evt.button != 0 || evt.type != EventType.MouseDown) return;
            //节点选择范围
            var nodeSelectableRect = new Rect(x, y, _view.position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH,
                DefEditorBTreeUI.NODE_HEIGHT);
            //不在当前节点范围内
            if (!nodeSelectableRect.Contains(evt.mousePosition))
            {
                return;
            }

            OnMouseDownNode(ref node);
            isNodeInDragging = true;
            _view.Repaint();
        }

        /// <summary>
        /// 按下节点回调
        /// </summary>
        /// <param name="node"></param>
        private void OnMouseDownNode(ref BNodeBase node)
        {
            if (isNodeInDragging)
            {
                return;
            }

            selectedNode = node;
        }

        /// <summary>
        /// 鼠标在插入线抬起回调
        /// </summary>
        private void OnMouseUpInsertLine(ref BNodeBase node)
        {
            //选中节点与目标节点相同
            if (selectedNode == node)
            {
                return;
            }

            //禁止拖拽移动根节点
            if (selectedNode?.parent == null)
            {
                return;
            }

            //无法插入到根节点前方
            if (node.parent == null)
            {
                return;
            }

            //目标节点是选中节点的子节点 无法挂载
            if (selectedNode.IsChildNode(ref node))
            {
                return;
            }

            selectedNode.parent.RemoveNode(ref selectedNode);
            selectedNode.parent = node.parent;
            node.parent.InsertNode(node, ref selectedNode);
            _view.Repaint();
        }

        /// <summary>
        /// 鼠标在节点处抬起回调
        /// </summary>
        private void OnMouseUpNode(ref BNodeBase node)
        {
            if (selectedNode == node)
            {
                return;
            }

            //禁止拖拽移动根节点
            if (selectedNode?.parent == null)
            {
                return;
            }

            //目标节点是选中节点的子节点 无法挂载
            if (selectedNode.IsChildNode(ref node))
            {
                return;
            }

            selectedNode.parent.RemoveNode(ref selectedNode);
            selectedNode.parent = node;
            node.AddNode(ref selectedNode);
            _view.Repaint();
        }
    }
}