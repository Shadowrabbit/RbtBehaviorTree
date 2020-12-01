// ******************************************************************
//       /\ /|       @file       BNodeBase.cs
//       \ V/        @brief      行为树节点基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-11-29 18:05:11
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;

namespace SR.RbtBehaviorTree
{
    public class BNodeBase
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

        /// <summary>
        /// 节点类型
        /// </summary>
        public virtual BNodeType NodeType => BNodeType.None; //节点类型

        public BNodeBase parent = null; //父节点
        public List<BNodeBase> listChildNodes = new List<BNodeBase>(); //子节点列表
        private ActionResult _actionResult; //节点执行结果

        /// <summary>
        /// 进入时回调
        /// </summary>
        /// <param name="bData"></param>
        protected virtual void OnEnter(ref BDataBase bData)
        {
            _actionResult = ActionResult.Idle;
        }

        /// <summary>
        /// 运行中回调
        /// </summary>
        /// <param name="bData"></param>
        protected virtual ActionResult OnRunning(ref BDataBase bData)
        {
            return ActionResult.Success;
        }

        /// <summary>
        /// 离开时回调
        /// </summary>
        /// <param name="bData"></param>
        protected virtual void OnExit(ref BDataBase bData)
        {
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateNode(ref BDataBase bData)
        {
            //待机状态 进入当前节点
            if (_actionResult == ActionResult.Idle)
            {
                OnEnter(ref bData);
                _actionResult = ActionResult.Running;
            }

            //当前帧动作执行结果
            var actionResult = OnRunning(ref bData);
            //执行中
            if (actionResult == ActionResult.Running) return _actionResult;
            //执行结束
            OnExit(ref bData);
            _actionResult = ActionResult.Idle;

            return _actionResult;
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="bNode"></param>
        public void AddNode(ref BNodeBase bNode)
        {
            listChildNodes.Add(bNode);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="bNode"></param>
        public void RemoveNode(ref BNodeBase bNode)
        {
            listChildNodes.Remove(bNode);
        }

        /// <summary>
        /// 插入节点 将新节点插入到目标节点前方
        /// </summary>
        /// <param name="targetNode">目标节点</param>
        /// <param name="bNode"></param>
        public void InsertNode(BNodeBase targetNode, ref BNodeBase bNode)
        {
            var index = listChildNodes.FindIndex((node) => node == targetNode);
            listChildNodes.Insert(index, bNode);
        }

        /// <summary>
        /// 替换节点 将目标节点替换成新节点
        /// </summary>
        /// <param name="targetNode"></param>
        /// <param name="bNode"></param>
        public void ReplaceNode(BNodeBase targetNode, ref BNodeBase bNode)
        {
            var index = listChildNodes.FindIndex((node) => node == targetNode);
            listChildNodes[index] = bNode;
        }
    }
}