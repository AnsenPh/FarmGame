using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HotFix_Project
{
    public class LocalPlayerData : BaseDataNotify
    {
        private LocalPlayerData()
        {

        }

        private static LocalPlayerData Instance = null;
        public static LocalPlayerData GetInstance()
        {
            if (LocalPlayerData.Instance == null)
            {
                LocalPlayerData.Instance = new LocalPlayerData();
            }

            return LocalPlayerData.Instance;
        }

        public static void ReleaseInstance()
        {
            LocalPlayerData.Instance = null;
        }


        public void Init()
        {

        }

        public BaseData<string> Data_Session = new BaseData<string>();
        public BaseData<string> Data_Token = new BaseData<string>();
        public BaseData<int> Data_UserID = new BaseData<int>();
    }
}

    

