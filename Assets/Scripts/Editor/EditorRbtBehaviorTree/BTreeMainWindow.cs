// ******************************************************************
//       /\ /|       @file       BTreeMainWindow.cs
//       \ V/        @brief      行为树编辑器主窗口
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 13:04:39
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using System.Reflection;
using Editor.EditorRbtBehaviorTree.Def;
using RbtBehaviorTree;
using RbtBehaviorTree.BTNode.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorRbtBehaviorTree
{
    public class BTreeMainWindow : EditorWindow
    {
        private const string NULL = "Null";
        private static BTreeMainWindow _instance; //视图
        private static BTreeMainWindowController _controller; //控制器
        private Vector2 _scrollPos = new Vector2(0, 0); //树的绘制区域

        [MenuItem("Window/BehaviorTree")]
        private static void ShowWindow()
        {
            var window = GetWindow<BTreeMainWindow>();
            window.titleContent = DefEditorBTreeUI.CONTENT_TITLE_MAIN;
            window.Show();
            _instance = window;
            _controller = new BTreeMainWindowController(_instance);
            BNodeFactory.Instance.Test();
        }

        private void OnGUI()
        {
            TryDrawTree();
            DrawMainMenu();
        }

        /// <summary>
        /// 尝试绘制行为树
        /// </summary>
        private void TryDrawTree()
        {
            //左边定一片可滑动区域作为行为树节点绘制区域
            _scrollPos = GUI.BeginScrollView(
                new Rect(0, 0, position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH, position.height),
                _scrollPos,
                new Rect(0, 0, maxSize.x, maxSize.y));
            //绘制背景
            DrawTreeBg();
            //数据存在
            if (_controller.Model?.rootNode != null)
            {
                var zeroX = 0;
                var zeroY = 0;
                //绘制树
                DrawTree(ref zeroX, ref zeroY, _controller.Model);
            }

            GUI.EndScrollView();
        }

        /// <summary>
        /// 绘制树的背景
        /// </summary>
        private void DrawTreeBg()
        {
            var texBlackLine = TexLineFactory.Instance.Create(Color.black);
            var texWhiteLine = TexLineFactory.Instance.Create(Color.gray);
            for (var i = 0; i < 1000; i++)
            {
                GUI.DrawTexture(
                    new Rect(0, i * DefEditorBTreeUI.NODE_HEIGHT, _instance.position.width,
                        DefEditorBTreeUI.NODE_HEIGHT),
                    i % 2 == 0 ? texBlackLine : texWhiteLine);
            }
        }

        /// <summary>
        /// 绘制行为树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tree"></param>
        private void DrawTree(ref int x, ref int y, BTree tree)
        {
        }

        /// <summary>
        /// 绘制节点
        /// </summary>
        private void DrawNode()
        {
        }

        /// <summary>
        /// 绘制主菜单
        /// </summary>
        private void DrawMainMenu()
        {
            const int x = 0;
            var y = 0;
            GUI.BeginGroup(new Rect(position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH, y,
                DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH, 1000));
            //加载
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT), DefEditorBTreeUI.LOAD))
            {
                _controller.Load();
            }

            y += DefEditorBTreeUI.BUTTON_HEIGHT;
            //保存
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),  DefEditorBTreeUI.SAVE))
            {
                _controller.Save();
            }

            y += DefEditorBTreeUI.BUTTON_HEIGHT;
            GUI.Label(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                "=======================");
            y += DefEditorBTreeUI.NODE_HEIGHT;
            //名称
            GUI.Label(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                "TreeName: " + (_controller.Model == null
                    ? NULL
                    : _controller.Model.name));
            y += DefEditorBTreeUI.NODE_HEIGHT;
            //创建
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),  DefEditorBTreeUI.CREATE))
            {
                _controller.Create();
            }

            y += DefEditorBTreeUI.BUTTON_HEIGHT;
            //清除
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),  DefEditorBTreeUI.CLEAR))
            {
                _controller.Clear();
            }

            y += DefEditorBTreeUI.BUTTON_HEIGHT;
            GUI.Label(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                "=======================");
            y += DefEditorBTreeUI.NODE_HEIGHT;
            //树存在的话
            if (_controller.Model != null)
            {
                //允许修改树的名称
                _controller.Model.name =
                    GUI.TextField(new Rect(x, y, DefEditorBTreeUI.POPUP_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                        _controller.Model.name);
                y += DefEditorBTreeUI.NODE_HEIGHT;
                //修改树的根节点类型
                var cacheRootNodeTypeIndex = _controller.rootNodeTypeIndex;
                _controller.rootNodeTypeIndex = EditorGUI.Popup(
                    new Rect(x, y, DefEditorBTreeUI.POPUP_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),
                    _controller.rootNodeTypeIndex,
                    BNodeFactory.Instance.GetCompositeNodeNameList().ToArray());
                //树的根节点类型更变
                if (cacheRootNodeTypeIndex != _controller.rootNodeTypeIndex)
                {
                    var newRootNode =
                        BNodeFactory.Instance.CreateRootNode(_controller.rootNodeTypeIndex);
                    _controller.Model.ReplaceRootNode(newRootNode);
                }

                y += DefEditorBTreeUI.BUTTON_HEIGHT;
            }

            GUI.Label(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                "=======================");
            y += DefEditorBTreeUI.NODE_HEIGHT;
            //存在选中节点
            if (_controller.selectedNode != null)
            {
                GUI.Label(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                    "Node Type: " + _controller.selectedNode.NodeType);
                y += DefEditorBTreeUI.NODE_HEIGHT;
                GUI.Label(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                    "Node Name: " + _controller.selectedNode.GetType().FullName);
                y += DefEditorBTreeUI.NODE_HEIGHT;
                //子类附加属性
                DrawNodeAdditionalProperty(_controller.selectedNode, x, ref y);
            }

            GUI.Label(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                "=======================");
            GUI.EndGroup();
        }

        /// <summary>
        /// 绘制节点额外属性
        /// </summary>
        /// <param name="node"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DrawNodeAdditionalProperty(BNodeBase node, int x, ref int y)
        {
            try
            {
                //当前节点类型
                var type = node.GetType();
                //当前节点参数信息列表
                var fieldInfos = type.GetFields();
                for (var i = 0; i < fieldInfos.Length; i++)
                {
                    //参数信息
                    var info = fieldInfos[i];
                    object vl;
                    //整形的情况
                    if (info.FieldType == typeof(int))
                    {
                        var value = info.GetValue(node).ToString();
                        GUI.Label(new Rect(x, y, DefEditorBTreeUI.POPUP_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                            info.Name);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        value = GUI.TextField(
                            new Rect(x + DefEditorBTreeUI.POPUP_WIDTH, y, DefEditorBTreeUI.POPUP_WIDTH,
                                DefEditorBTreeUI.NODE_HEIGHT), value);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        vl = int.Parse(value);
                    }
                    //浮点的情况
                    else if (info.FieldType == typeof(float))
                    {
                        var value = info.GetValue(node).ToString();
                        GUI.Label(new Rect(x, y, DefEditorBTreeUI.POPUP_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                            info.Name);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        value = GUI.TextField(
                            new Rect(x + DefEditorBTreeUI.POPUP_WIDTH, y, DefEditorBTreeUI.POPUP_WIDTH,
                                DefEditorBTreeUI.NODE_HEIGHT), value);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        vl = float.Parse(value);
                    }
                    //布尔的情况
                    else if (info.FieldType == typeof(bool))
                    {
                        var value = (bool) info.GetValue(node);
                        GUI.Label(new Rect(x, y, DefEditorBTreeUI.POPUP_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                            info.Name);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        value = GUI.Toggle(
                            new Rect(x + DefEditorBTreeUI.POPUP_WIDTH, y, DefEditorBTreeUI.POPUP_WIDTH,
                                DefEditorBTreeUI.NODE_HEIGHT), value, "");
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        vl = value;
                    }
                    //字符串的情况
                    else if (info.FieldType == typeof(string))
                    {
                        var value = info.GetValue(node).ToString();
                        GUI.Label(new Rect(x, y, DefEditorBTreeUI.POPUP_WIDTH, DefEditorBTreeUI.POPUP_WIDTH),
                            info.Name);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        value = GUI.TextField(
                            new Rect(x + DefEditorBTreeUI.POPUP_WIDTH, y, DefEditorBTreeUI.POPUP_WIDTH,
                                DefEditorBTreeUI.POPUP_WIDTH), value);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        vl = value;
                    }
                    else
                    {
                        Debug.LogWarning("预期之外的数据类型");
                        return;
                    }

                    info.SetValue(node, vl);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        private void Update()
        {
            if (_controller.selectedNode == null)
            {
                return;
            }

            Repaint();
        }
    }
}