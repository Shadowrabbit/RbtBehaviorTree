// ******************************************************************
//       /\ /|       @file       DefEditorBTreeUI.cs
//       \ V/        @brief      行为树编辑器UI相关定义
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-30 13:11:04
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class DefEditorBTreeUI
    {
        public static int MAIN_MENU_LAYOUT_WIDTH = 200; //主菜单布局宽度
        public static int NODE_HEIGHT = 20; //节点高度
        public static int BUTTON_HEIGHT = 40; //按钮高度
        public static int BUTTON_WIDTH = 200; //按钮宽度
        public static int POPUP_WIDTH = 100; //选择框宽度
        public static int TITLE_WIDTH = 70; //标题框宽度
        public static int NODE_POLY_LINE_WIDTH = 20; //节点前方标记线的宽度
        public static string SAVE = "Save Tree";
        public static string LOAD = "Load Tree";
        public static string CREATE = "Create Tree";
        public static string CLEAR = "Clear All Nodes";
        public static string SAVE_TREE = "Save Tree";
        public static string LOAD_TREE = "Load Tree";
        public static string DEFAULT_TREE_NAME = "defaultTree";

        public static GUIContent CONTENT_TITLE_MAIN = new GUIContent("BTreeMainWindow"); //主页面标题
    }
}