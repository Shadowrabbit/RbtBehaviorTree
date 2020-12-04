// ******************************************************************
//       /\ /|       @file       BNodeBase.cs
//       \ V/        @brief      行为树节点基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:05:11
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using _3rd.LitJson;
using UnityEngine;

namespace SR.RbtBehaviorTree
{
    public class BNodeBase : IJson
    {
        /// <summary>
        /// 节点执行结果
        /// </summary>
        public enum ActionResult
        {
            Idle = 0, //待机中
            Success = 1, //成功    
            Running = 2, //运行中
            Failure = 3, //失败
        }

        private ActionResult _actionResult; //节点执行结果
        protected BNodeBase _parent; //父节点
        protected List<BNodeBase> _listChildNodes = new List<BNodeBase>(); //子节点列表
        public virtual BNodeType NodeType => BNodeType.None; //节点类型

        public BNodeBase Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public List<BNodeBase> ListChildNodes
        {
            get => _listChildNodes;
            set => _listChildNodes = value;
        }

        /// <summary>
        /// 进入时回调
        /// </summary>
        /// <param name="bData"></param>
        protected virtual void OnEnter(BDataBase bData)
        {
            _actionResult = ActionResult.Idle;
        }

        /// <summary>
        /// 运行中回调
        /// </summary>
        /// <param name="bData"></param>
        protected virtual ActionResult OnRunning(BDataBase bData)
        {
            return ActionResult.Success;
        }

        /// <summary>
        /// 离开时回调
        /// </summary>
        /// <param name="bData"></param>
        protected virtual void OnExit(BDataBase bData)
        {
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateNode(BDataBase bData)
        {
            //待机状态 进入当前节点
            if (_actionResult == ActionResult.Idle)
            {
                OnEnter(bData);
                _actionResult = ActionResult.Running;
            }

            //当前帧动作执行结果
            var actionResult = OnRunning(bData);
            //执行中
            if (actionResult == ActionResult.Running) return _actionResult;
            //执行结束
            OnExit(bData);
            _actionResult = ActionResult.Idle;

            return _actionResult;
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="bNode"></param>
        public void AddNode(BNodeBase bNode)
        {
            _listChildNodes.Add(bNode);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="bNode"></param>
        public void RemoveNode(BNodeBase bNode)
        {
            _listChildNodes.Remove(bNode);
        }

        /// <summary>
        /// 插入节点 将新节点插入到目标节点前方
        /// </summary>
        /// <param name="targetNode">目标节点</param>
        /// <param name="bNode"></param>
        public void InsertNode(BNodeBase targetNode, BNodeBase bNode)
        {
            var index = _listChildNodes.FindIndex((node) => node == targetNode);
            _listChildNodes.Insert(index, bNode);
        }

        /// <summary>
        /// 替换节点 将目标节点替换成新节点
        /// </summary>
        /// <param name="targetNode"></param>
        /// <param name="bNode"></param>
        public void ReplaceNode(BNodeBase targetNode, BNodeBase bNode)
        {
            var index = _listChildNodes.FindIndex((node) => node == targetNode);
            _listChildNodes[index] = bNode;
        }

        /// <summary>
        /// node是否为this的子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsChildNode(BNodeBase node)
        {
            for (var i = 0; i < _listChildNodes.Count; i++)
            {
                var isChildNode = _listChildNodes[i].IsChildNode(node);
                if (isChildNode)
                {
                    return true;
                }
            }

            return this == node;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        public JsonData WriteJson()
        {
            var jsonData = new JsonData
            {
                ["nodeType"] = NodeType.ToString(), //节点类型
                ["typeName"] = GetType().FullName, //命名空间+class类型
                ["args"] = new JsonData() //额外参数(可能不存在)
            };
            jsonData["args"].SetJsonType(JsonType.Object);
            //反射获取当前类的公共参数
            var fieldInfos = GetType().GetFields();
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var info = fieldInfos[i];
                jsonData["args"][info.Name] = info.GetValue(this).ToString();
            }

            //子节点
            jsonData["listChildNodes"] = new JsonData();
            jsonData["listChildNodes"].SetJsonType(JsonType.Array);
            for (var i = 0; i < _listChildNodes.Count; i++)
            {
                jsonData["listChildNodes"].Add(_listChildNodes[i].WriteJson());
            }

            return jsonData;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="jsonData"></param>
        public void ReadJson(JsonData jsonData)
        {
            //当前节点
            var args = jsonData["args"];
            var fieldInfos = GetType().GetFields();
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var info = fieldInfos[i];
                if (!args.Keys.Contains(info.Name)) continue;
                var strValue = args[info.Name].ToString();
                object val = null;
                if (info.FieldType == typeof(int)) val = int.Parse(strValue);
                else if (info.FieldType == typeof(float)) val = float.Parse(strValue);
                else if (info.FieldType == typeof(bool)) val = bool.Parse(strValue);
                else if (info.FieldType == typeof(string)) val = strValue;
                else return;
                info.SetValue(this, val);
            }

            //子节点
            for (var i = 0; i < jsonData["listChildNodes"].Count; i++)
            {
                var typeName = jsonData["listChildNodes"][i]["typeName"].ToString();
                var type = Type.GetType(typeName);
                if (type == null)
                {
                    Debug.LogError("class类型错误 type=" + typeName);
                    return;
                }

                if (!(Activator.CreateInstance(type) is BNodeBase childNode))
                {
                    Debug.LogError("class类型不匹配 期望:" + nameof(BNodeBase));
                    return;
                }

                childNode.ReadJson(jsonData["listChildNodes"][i]);
                childNode.Parent = this;
                AddNode(childNode);
            }
        }
    }
}