using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class LoginNetwork : BaseNetwork<LoginNetwork>
    {
        enum MsgID
        {
            LoginMsg = 1001,

        }

        public override void RegisterMsgEvent()
        {
            NetworkCtr.Instance.RegisterMsg((int)MsgID.LoginMsg, OnLoginMsg);
        }

        public override void UnRegisterMsgEvent()
        {
            NetworkCtr.Instance.UnRegisterMsg((int)MsgID.LoginMsg);
        }

        /// <summary>
        /// 消息回调
        /// </summary>
        void OnLoginMsg(ReceiveStruct _Struct)
        {
            LoginDataNotify.Instance.Data_TestWindowShow.Data = true;
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

