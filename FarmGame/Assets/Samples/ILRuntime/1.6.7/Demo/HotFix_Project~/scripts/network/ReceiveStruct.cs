using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;

namespace HotFix_Project
{
    public class ReceiveStruct
    {
        public ReceiveStruct(int _Status , int _MsgId , string _Description , int _ActionId , string _Time , ByteString _MsgBody)
        {
            m_StatusCode = _Status;
            m_MsgId = _MsgId;
            m_Description = _Description;
            m_ActionID = _ActionId;
            m_Time = _Time;
            m_MsgBody = _MsgBody;
        }

        public int m_StatusCode;
        public int m_MsgId;
        public string m_Description;
        public int m_ActionID;
        public string m_Time;
        public ByteString m_MsgBody;

    }
}
