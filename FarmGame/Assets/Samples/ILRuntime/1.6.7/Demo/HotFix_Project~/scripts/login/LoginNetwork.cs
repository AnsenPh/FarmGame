using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class LoginNetwork : BaseNetwork
    {
        public static LoginNetwork GetInstance()
        {
            if(Instance == null)
            {
                Instance = new LoginNetwork();
            }
            return Instance;
        }
        LoginNetwork()
        {

        }
        static LoginNetwork Instance = null;

        enum MsgID
        {
            LoginMsg = 1001,

        }

        public override void RegisterMsgEvent()
        {
            NetworkCtr.GetInstance().RegisterMsg((int)MsgID.LoginMsg, OnLoginMsg);
        }

        public override void UnRegisterMsgEvent()
        {
            NetworkCtr.GetInstance().UnRegisterMsg((int)MsgID.LoginMsg);
        }


        /// <summary>
        /// 消息回调
        /// </summary>

        void OnLoginMsg(ReceiveStruct _Struct)
        {

        }

        /// <summary>
        /// 消息发送
        /// </summary>
        public void SendLoginMsg(string _Account , string Password)
        {
            //NetworkCtr.GetInstance().SendMsg((int)MsgID.LoginMsg ,  "这里填入protobuff的结构体" );
        }
    }
}

