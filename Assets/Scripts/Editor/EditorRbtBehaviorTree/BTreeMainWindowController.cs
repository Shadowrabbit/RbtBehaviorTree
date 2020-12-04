// ******************************************************************
//       /\ /|       @file       BTreeMainWindowController.cs
//       \ V/        @brief      主页面控制器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 14:07:38
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using System.IO;
using _3rd.LitJson;
using UnityEditor;
using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BTreeMainWindowController
    {
        private BTree _model; //数据模型
        private int _rootNodeTypeIndex = -1; //根节点类型索引
        private BNodeBase _selectedNode; //选中的节点
        private bool _isNodeInDragging; //正在移动节点
        private readonly BTreeMainWindow _view; //视图
        public BTree Model => _model;

        public int RootNodeTypeIndex
        {
            get => _rootNodeTypeIndex;
            set => _rootNodeTypeIndex = value;
        }

        public BNodeBase SelectedNode
        {
            get => _selectedNode;
            set => _selectedNode = value;
        }

        public bool IsNodeInDragging
        {
            get => _isNodeInDragging;
            set => _isNodeInDragging = value;
        }

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
            if (_model == null)
            {
                return;
            }

            //文件储存路径
            var savePath = EditorUtility.SaveFilePanel(DefEditorBTreeUI.SAVE_TREE,
                Application.dataPath, DefEditorBTreeUI.DEFAULT_TREE_NAME, "json");
            var strJson = _model.WriteJson().ToJson();
            File.WriteAllText(savePath, strJson);
            Debug.Log("保存成功" + savePath);
        }

        /// <summary>
        /// 加载
        /// </summary>
        public void Load()
        {
            var loadPath = EditorUtility.OpenFilePanel(DefEditorBTreeUI.LOAD_TREE,
                Application.dataPath, "json");
            if (loadPath == "") return;
            var strJson = File.ReadAllText(loadPath);
            var jsonData = JsonMapper.ToObject(strJson);
            if (_model == null)
            {
                _model = new BTree();
            }

            _model.ReadJson(jsonData);
            _view.Repaint();
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

            //根节点不存在
            if (_model.rootNode == null)
            {
                Debug.LogError("根节点不存在!");
                return;
            }

            //删除根节点下的所有子节点 注意list要倒着遍历删除
            for (var i = _model.rootNode.ListChildNodes.Count - 1; i >= 0; i--)
            {
                _model.RemoveNode(_model.rootNode.ListChildNodes[i]);
            }

            _selectedNode = null;
            _view.Repaint();
        }

        /// <summary>
        /// 创建行为树
        /// </summary>
        public void Create()
        {
            var rootNode = BNodeFactory.Instance.Create(typeof(BNodeSequence));
            _model = new BTree {name = "New Tree", rootNode = rootNode};
            _rootNodeTypeIndex = BNodeFactory.Instance.GetCompositeNodeIndex(typeof(BNodeSequence));
            _view.Repaint();
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="arg"></param>
        public void AddNode(object arg)
        {
            if (_selectedNode == null)
            {
                return;
            }

            var type = arg as Type;
            //根据类型创建新节点
            var node = BNodeFactory.Instance.Create(type);
            //将新节点挂载到选中的节点
            _selectedNode.AddNode(node);
            node.Parent = _selectedNode;
            //挂载结束 更换选中节点
            _selectedNode = node;
            _view.Repaint();
        }

        /// <summary>
        /// 替换节点
        /// </summary>
        /// <param name="arg"></param>
        public void ReplaceNode(object arg)
        {
            if (_selectedNode == null)
            {
                return;
            }

            var type = arg as Type;
            var node = BNodeFactory.Instance.Create(type);
            node.Parent = _selectedNode.Parent;
            node.ListChildNodes = _selectedNode.ListChildNodes;
            //更新父节点信息
            if (node.Parent != null)
            {
                node.Parent.ReplaceNode(_selectedNode, node);
            }
            else
            {
                _model.rootNode = node;
            }

            _selectedNode = node;
            _view.Repaint();
        }

        /// <summary>
        /// 撤销节点
        /// </summary>
        /// <param name="arg"></param>
        public void RemoveNode(object arg)
        {
            if (_selectedNode == null)
            {
                return;
            }

            //从书中移除选中节点
            _model.RemoveNode(_selectedNode);
            _selectedNode = null;
            _view.Repaint();
        }

        /// <summary>
        /// 拖动检测
        /// </summary>
        public void CheckNodeMouseUp(ref int x, ref int y, BNodeBase node)
        {
            var evt = Event.current;
            if (evt.button != 0 || evt.type != EventType.MouseUp) return;
            //节点插入识别范围
            var insertLineRect = new Rect(0, y, _view.position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH, 5);
            //节点挂载识别范围
            var nodeSelectableRect = new Rect(0, y, _view.position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH,
                DefEditorBTreeUI.NODE_HEIGHT - 5);
            //在插入识别范围内抬起
            if (insertLineRect.Contains(evt.mousePosition))
            {
                OnMouseUpInsertLine(node);
            }
            //在节点挂载识别范围内抬起
            else if (nodeSelectableRect.Contains(evt.mousePosition))
            {
                OnMouseUpNode(node);
            }

            _isNodeInDragging = false;
            _view.Repaint();
        }

        /// <summary>
        /// 点击检测
        /// </summary>
        /// <param name="y"></param>
        /// <param name="node"></param>
        /// <param name="x"></param>
        public void CheckNodeMouseDown(ref int x, ref int y, BNodeBase node)
        {
            var evt = Event.current;
            //button=0是左键点击
            if (evt.button != 0 || evt.type != EventType.MouseDown) return;
            //节点选择范围
            var nodeSelectableRect = new Rect(x, y, _view.position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH,
                DefEditorBTreeUI.NODE_HEIGHT);
            //不在当前节点范围内
            if (!nodeSelectableRect.Contains(evt.mousePosition))
            {
                return;
            }

            OnMouseDownNode(node);
            _isNodeInDragging = true;
            _view.Repaint();
        }

        /// <summary>
        /// 按下节点回调
        /// </summary>
        /// <param name="node"></param>
        private void OnMouseDownNode(BNodeBase node)
        {
            if (_isNodeInDragging)
            {
                return;
            }

            _selectedNode = node;
        }

        /// <summary>
        /// 鼠标在插入线抬起回调
        /// </summary>
        private void OnMouseUpInsertLine(BNodeBase node)
        {
            //选中节点与目标节点相同
            if (_selectedNode == node)
            {
                return;
            }

            //禁止拖拽移动根节点
            if (_selectedNode?.Parent == null)
            {
                return;
            }

            //无法插入到根节点前方
            if (node.Parent == null)
            {
                return;
            }

            //目标节点是选中节点的子节点 无法挂载
            if (_selectedNode.IsChildNode(node))
            {
                return;
            }

            _selectedNode.Parent.RemoveNode(_selectedNode);
            _selectedNode.Parent = node.Parent;
            node.Parent.InsertNode(node, _selectedNode);
            _view.Repaint();
        }

        /// <summary>
        /// 鼠标在节点处抬起回调
        /// </summary>
        private void OnMouseUpNode(BNodeBase node)
        {
            if (_selectedNode == node)
            {
                return;
            }

            //禁止拖拽移动根节点
            if (_selectedNode?.Parent == null)
            {
                return;
            }

            //目标节点是选中节点的子节点 无法挂载
            if (_selectedNode.IsChildNode(node))
            {
                return;
            }

            _selectedNode.Parent.RemoveNode(_selectedNode);
            _selectedNode.Parent = node;
            node.AddNode(_selectedNode);
            _view.Repaint();
        }
    }
}