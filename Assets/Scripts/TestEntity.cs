// ******************************************************************
//       /\ /|       @file       TestEntity.cs
//       \ V/        @brief      测试单位
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-05 12:36:22
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using _3rd.LitJson;
using SR.RbtBehaviorTree;
using UnityEngine;

public class TestEntity : MonoBehaviour
{
    private BDataBase _bDataBase;
    private readonly BTree _bTree = new BTree();
    public TextAsset textAsset;

    //因为没有战斗和地图模块 不方便配合展示 故采用log表现行为树的流程
    //这个测试用例编写了一个单位
    //这个单位从被创建后就一直带有中毒效果 每次-20生命
    //当这个单位的生命值大于等于50%时 会进入空闲状态
    //当这个单位的生命值低于50%时 会尝试使用药剂恢复生命 每次+10生命
    //当这个单位的生命值低于30%时 会尝试逃跑
    private void Start()
    {
        _bDataBase = new BDataBase() {maxHpValue = 100, currentHpValue = 100};
        var jsonData = JsonMapper.ToObject(textAsset.text);
        _bTree.ReadJson(jsonData);
    }

    private void Update()
    {
        _bTree?.Update(_bDataBase);
    }
}