// ******************************************************************
//       /\ /|       @file       IJson.cs.cs
//       \ V/        @brief      json序列化接口
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2020-12-04 12:08:18
//    *(__\_\        @Copyright  Copyright (c) 2020, Shadowrabbit
// ******************************************************************

using _3rd.LitJson;

namespace SR
{
    public interface IJson
    {
        /// <summary>
        /// 序列化
        /// </summary>
        JsonData WriteJson();

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="jsonData"></param>
        void ReadJson(JsonData jsonData);
    }
}