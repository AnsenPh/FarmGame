using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public static class MainStart
    {
        // Start is called before the first frame update
        public static void Start()
        {
            try
            {
                Debug.Log("Dll的代码开始执行");
                UIMgr.GetInstance().ShowRootUI("LoginMgr", true);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }


    }

}
