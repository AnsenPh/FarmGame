using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HotFix_Project
{
    public class LocalPlayerData : BaseDataNotify<LocalPlayerData>
    {

        public BaseData<string> Data_Session = new BaseData<string>();
        public BaseData<string> Data_Token = new BaseData<string>();
        public BaseData<int> Data_UserID = new BaseData<int>();

        //玩家拥有得物品  第一个参数是物品ID  , 第二个是数量
        public List<KeyValuePair<int , int>> m_Items = new List<KeyValuePair<int, int>>();

        public LocalPlayerData()
        {
            m_Items.Add(new KeyValuePair<int, int>(1, 1));
        }
    }
}

    

