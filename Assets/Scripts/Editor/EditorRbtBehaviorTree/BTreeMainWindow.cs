// ******************************************************************
//       /\ /|       @file       BTreeMainWindow.cs
//       \ V/        @brief      行为树编辑器主窗口
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 13:04:39
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using UnityEditor;
using UnityEngine;

namespace SR.RbtBehaviorTree
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
        }

        private void OnGUI()
        {
            TryDrawTree();
            DrawMainMenu();
        }

        private void Update()
        {
            //拖拽中每帧刷新
            if (_controller.isNodeInDragging)
            {
                Repaint();
            }
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
            //绘制根节点
            DrawNode(x, ref y, tree.rootNode);
        }

        /// <summary>
        /// 绘制节点
        /// </summary>
        private void DrawNode(int x, ref int y, BNodeBase node)
        {
            DrawNodeBg(ref node, ref y); //节点bg
            GUI.Label(new Rect(x, y, position.width, DefEditorBTreeUI.NODE_HEIGHT), node.GetType().Name);
            DrawSubMenu(ref x, ref y, ref node); //绘制节点处子菜单
            _controller.CheckNodeMouseDown(ref x, ref y, ref node);
            _controller.CheckNodeMouseUp(ref x, ref y, ref node);
            var pos1 = new Vector3(x + DefEditorBTreeUI.NODE_POLY_LINE_WIDTH / 2, y + DefEditorBTreeUI.NODE_HEIGHT, 0);
            Handles.color = Color.magenta;
            //绘制子节点和线
            for (var i = 0; i < node.listChildNodes.Count; i++)
            {
                y += DefEditorBTreeUI.NODE_HEIGHT;
                //绘制子节点前方线
                var pos2 = new Vector3(x + DefEditorBTreeUI.NODE_POLY_LINE_WIDTH / 2,
                    y + DefEditorBTreeUI.NODE_HEIGHT / 2, 0);
                var pos3 = new Vector3(x + DefEditorBTreeUI.NODE_POLY_LINE_WIDTH,
                    y + DefEditorBTreeUI.NODE_HEIGHT / 2, 0);
                DrawNode(x + DefEditorBTreeUI.NODE_POLY_LINE_WIDTH, ref y, node.listChildNodes[i]);
                Handles.DrawPolyLine(pos1, pos2, pos3);
            }
        }

        /// <summary>
        /// 绘制子菜单
        /// </summary>
        private void DrawSubMenu(ref int x, ref int y, ref BNodeBase node)
        {
            //正在移动节点
            if (_controller.isNodeInDragging)
            {
                return;
            }

            //节点选择范围
            var nodeSelectableRect = new Rect(0, y, position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH,
                DefEditorBTreeUI.NODE_HEIGHT);
            var evt = Event.current;
            //不在当前节点可选范围内
            if (!nodeSelectableRect.Contains(evt.mousePosition))
            {
                return;
            }

            //没有右键点击
            if (evt.type != EventType.ContextClick) return;
            _controller.selectedNode = node;
            var menu = new GenericMenu();
            foreach (var item in BNodeFactory.Instance.listCompositeTypes)
            {
                menu.AddItem(new GUIContent("Create/Composite/" + item.Name), false, _controller.AddNode, item);
                menu.AddItem(new GUIContent("Replace/Composite/" + item.Name), false, _controller.ReplaceNode, item);
            }

            foreach (var item in BNodeFactory.Instance.listActionTypes)
            {
                menu.AddItem(new GUIContent("Create/Action/" + item.Name), false, _controller.AddNode, item);
                menu.AddItem(new GUIContent("Replace/Action/" + item.Name), false, _controller.ReplaceNode, item);
            }

            foreach (var item in BNodeFactory.Instance.listConditionTypes)
            {
                menu.AddItem(new GUIContent("Create/Condition/" + item.Name), false, _controller.AddNode, item);
                menu.AddItem(new GUIContent("Replace/Condition/" + item.Name), false, _controller.ReplaceNode, item);
            }

            foreach (var item in BNodeFactory.Instance.listDecoratorTypes)
            {
                menu.AddItem(new GUIContent("Create/Decorator/" + item.Name), false, _controller.AddNode, item);
                menu.AddItem(new GUIContent("Replace/Decorator/" + item.Name), false, _controller.ReplaceNode, item);
            }

            menu.AddItem(new GUIContent("Delete"), false, _controller.RemoveNode, "");
            menu.ShowAsContext();
        }

        /// <summary>
        /// 绘制节点背景
        /// </summary>
        private void DrawNodeBg(ref BNodeBase node, ref int y)
        {
            //无论是否处于拖拽状态 当前节点被选中的话变蓝色
            if (_controller.selectedNode == node)
            {
                //选中的节点
                var texLineBlue = TexLineFactory.Instance.Create(Color.blue);
                GUI.DrawTexture(new Rect(0, y, position.width, DefEditorBTreeUI.NODE_HEIGHT), texLineBlue);
            }

            //节点拖拽中
            if (!_controller.isNodeInDragging) return;
            var texLineGreen = TexLineFactory.Instance.Create(Color.green);
            var texLineRed = TexLineFactory.Instance.Create(Color.red);
            var evt = Event.current;
            //节点插入识别范围
            var insertLineRect = new Rect(0, y, position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH, 5);
            //节点挂载识别范围
            var nodeSelectableRect = new Rect(0, y, position.width - DefEditorBTreeUI.MAIN_MENU_LAYOUT_WIDTH,
                DefEditorBTreeUI.NODE_HEIGHT - 5);
            //在当前节点的插入识别范围    
            if (insertLineRect.Contains(evt.mousePosition))
            {
                //当前拖拽中的是根节点 红色警告
                if (_controller.selectedNode?.parent == null)
                {
                    GUI.DrawTexture(new Rect(0, y, position.width, 2),
                        texLineRed);
                }
                //无法插入在根节点前方
                else if (node.parent == null)
                {
                    GUI.DrawTexture(new Rect(0, y, position.width, 2),
                        texLineRed);
                }
                //选中节点不是目标节点的根节点时允许操作
                else
                {
                    GUI.DrawTexture(new Rect(0, y, position.width, 2),
                        _controller.selectedNode?.IsChildNode(ref node) != true ? texLineGreen : texLineRed);
                }
            }
            //在当前节点的挂载范围
            else if (nodeSelectableRect.Contains(evt.mousePosition))
            {
                //动作节点与条件节点不能作为挂载根节点
                if (node.GetType().IsSubclassOf(typeof(BNodeAction)) ||
                    node.GetType().IsSubclassOf(typeof(BNodeCondition)))
                {
                    GUI.DrawTexture(new Rect(0, y, position.width, DefEditorBTreeUI.NODE_HEIGHT), texLineRed);
                }
                //当前拖拽中的是根节点 红色警告
                else if (_controller.selectedNode?.parent == null)
                {
                    GUI.DrawTexture(new Rect(0, y, position.width, DefEditorBTreeUI.NODE_HEIGHT), texLineRed);
                }
                //选中节点不是目标节点的根节点时允许操作
                else
                {
                    GUI.DrawTexture(new Rect(0, y, position.width, DefEditorBTreeUI.NODE_HEIGHT),
                        _controller.selectedNode?.IsChildNode(ref node) != true ? texLineGreen : texLineRed);
                }
            }
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
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),
                DefEditorBTreeUI.LOAD))
            {
                _controller.Load();
            }

            y += DefEditorBTreeUI.BUTTON_HEIGHT;
            //保存
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),
                DefEditorBTreeUI.SAVE))
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
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),
                DefEditorBTreeUI.CREATE))
            {
                _controller.Create();
            }

            y += DefEditorBTreeUI.BUTTON_HEIGHT;
            //清除
            if (GUI.Button(new Rect(x, y, DefEditorBTreeUI.BUTTON_WIDTH, DefEditorBTreeUI.BUTTON_HEIGHT),
                DefEditorBTreeUI.CLEAR))
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
                // //允许修改树的名称
                GUI.Label(new Rect(x, y, DefEditorBTreeUI.TITLE_WIDTH, DefEditorBTreeUI.NODE_HEIGHT),
                    "TreeName: ");
                _controller.Model.name =
                    GUI.TextField(
                        new Rect(x + DefEditorBTreeUI.TITLE_WIDTH, y, DefEditorBTreeUI.POPUP_WIDTH,
                            DefEditorBTreeUI.NODE_HEIGHT),
                        _controller.Model.name);
                y += DefEditorBTreeUI.NODE_HEIGHT;
                //修改树的根节点类型
                GUI.Label(
                    new Rect(x, y, DefEditorBTreeUI.TITLE_WIDTH,
                        DefEditorBTreeUI.NODE_HEIGHT),
                    "RootType: ");
                var cacheRootNodeTypeIndex = _controller.rootNodeTypeIndex;
                _controller.rootNodeTypeIndex = EditorGUI.Popup(
                    new Rect(x + DefEditorBTreeUI.TITLE_WIDTH, y, DefEditorBTreeUI.POPUP_WIDTH,
                        DefEditorBTreeUI.BUTTON_HEIGHT),
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
                    "Node Name: " + _controller.selectedNode.GetType().Name);
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
                            info.Name + ":");
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
                            info.Name + ":");
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
                            info.Name + ":");
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
                            info.Name + ":");
                        value = GUI.TextField(
                            new Rect(x + DefEditorBTreeUI.POPUP_WIDTH, y, DefEditorBTreeUI.POPUP_WIDTH,
                                DefEditorBTreeUI.POPUP_WIDTH), value);
                        y += DefEditorBTreeUI.NODE_HEIGHT;
                        vl = value;
                    }
                    else
                    {
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
    }
}