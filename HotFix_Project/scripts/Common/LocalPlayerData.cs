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
    }
}

    

