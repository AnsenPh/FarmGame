using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class LoginDataNotify : BaseDataNotify
    {
        public BaseData<bool> Data_TestWindowShow = new BaseData<bool>();












        private LoginDataNotify()
        {

        }

        private static LoginDataNotify Instance = null;
        public static LoginDataNotify GetInstance()
        {
            if (LoginDataNotify.Instance == null)
            {
                LoginDataNotify.Instance = new LoginDataNotify();
            }

            return LoginDataNotify.Instance;
        }

        public static void ReleaseInstance()
        {
            LoginDataNotify.Instance = null;
        }


        public void Init()
        {

        }


    }

}
