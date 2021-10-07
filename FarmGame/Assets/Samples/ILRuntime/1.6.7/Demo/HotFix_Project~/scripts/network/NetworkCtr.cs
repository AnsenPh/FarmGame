using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.Windows;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
namespace HotFix_Project
{
    public class NetworkCtr
    {
        public static NetworkCtr GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NetworkCtr();
            }
            return Instance;
        }
        public void InitSocket(string _Ip, int _Port, Action _ConnectSuccess, Action _ConnectFailed, Action _SocketClosed)
        {
            SetSocketCallback(_ConnectSuccess, _ConnectFailed, _SocketClosed);
            m_NativeSocket.ConnectSocket(_Ip, _Port);
        }

        public void SetSocketCallback(Action _ConnectSuccess, Action _ConnectFailed, Action _SocketClosed)
        {
            m_NativeSocket.OnConnect = _ConnectSuccess;
            m_NativeSocket.OnConnectFailed = _ConnectFailed;
            m_NativeSocket.OnClosed = _SocketClosed;
        }

        public void SendMsg(int _MsgID, Google.Protobuf.IMessage _ProtoData)
        {
            ResetData();
            CombineCoreData(_MsgID, _ProtoData);
        }

        Dictionary<int, Action<ReceiveStruct>> m_EventDic = new Dictionary<int, Action<ReceiveStruct>>();
        public void RegisterMsg(int _ActionId , Action<ReceiveStruct> _Callback)
        {
            if(m_EventDic.ContainsKey(_ActionId))
            {
                Debug.LogWarning("_ActionId===" + _ActionId + "===已经被注册过了，此次注册不会生效");
                return;
            }

            m_EventDic.Add(_ActionId, _Callback);

        }

        public void UnRegisterMsg(int _ActionId)
        {
            if (m_EventDic.ContainsKey(_ActionId) == false)
            {
                Debug.LogWarning("_ActionId===" + _ActionId + "===还没有被注册，无法注销");
                return;
            }
            m_EventDic.Remove(_ActionId);
        }

        public void ClearAllMsgEvent()
        {
            m_EventDic.Clear();
        }


        /////////////////////////////////////////////////////////////////////////////////
        ///          以下为内部实现，不会用到
        /////////////////////////////////////////////////////////////////////////////////


        string m_EncodeKey = "yiqu@2018";
        NetworkCtr()
        {
            m_NativeSocket = new NativeSocket();
            m_NativeSocket.SetRevieveCallback(OnMsg);
        }

        static NetworkCtr Instance = null;

        NativeSocket m_NativeSocket = null;
        int m_SendCounter = 1;
        string m_UserData = "";
        string m_PostData = "";
       

        void OnMsg(Byte[] _Data)
        {
            if(_Data[0] == 0x1f && _Data[1] == 0x8b && _Data[2] == 0x08 && _Data[3] == 0x00)
            {
                _Data = DecompressionData(_Data);
            }

            ReceiveStruct DataAfterParse = TryParseData(_Data);

            int CurrentActionId = DataAfterParse.m_ActionID;
            if(m_EventDic.ContainsKey(CurrentActionId)==false)
            {
                Debug.LogWarning("收到了ActionID===" + CurrentActionId + "===的消息，但是并没有事先注册，所以没有回调可以调用");
                return;
            }

            m_EventDic[CurrentActionId](DataAfterParse);

        }
        
        byte[] DecompressionData(byte[] _Data)
        {
            MemoryStream ms = new MemoryStream();
            int count = 0;
            GZipInputStream zip = new GZipInputStream(new MemoryStream(_Data));
            byte[] TempData = new byte[256];

            while((count = zip.Read(TempData,0, TempData.Length))!=0)
            {
                ms.Write(TempData,0,count);
            }
            byte[] result = ms.ToArray();
            ms.Close();
            return result;
        }

        ReceiveStruct TryParseData(byte[] _Data)
        {
            if(_Data == null || _Data.Length == 0)
            {
                Debug.LogError("出错啦，怎么可能等于0，快点排查");
                return null;
            }

            int CurrentPos = 0;
            int DataTotalLength = BitConverter.ToInt32(_Data, CurrentPos);
            CurrentPos += sizeof(int);

            if(DataTotalLength != _Data.Length)
            {
                Debug.LogError("出错啦，怎么可能不等于，快点排查");
                return null;
            }

            int StatusCode = BitConverter.ToInt32(_Data, CurrentPos);
            CurrentPos += sizeof(int);

            int MsgId = BitConverter.ToInt32(_Data, CurrentPos);
            CurrentPos += sizeof(int);


            int StringLength = BitConverter.ToInt32(_Data, CurrentPos);
            CurrentPos += sizeof(int);
            string Description = string.Empty;
            if (StringLength>0)
            {
                Description = Encoding.UTF8.GetString(_Data,CurrentPos , StringLength);
                CurrentPos += StringLength;
            }

            int ActionID = BitConverter.ToInt32(_Data, CurrentPos);
            CurrentPos += sizeof(int);

            int StringTimeLength = BitConverter.ToInt32(_Data, CurrentPos);
            CurrentPos += sizeof(int);
            string TimeStr = string.Empty;
            if (StringTimeLength > 0)
            {
                TimeStr = Encoding.UTF8.GetString(_Data, CurrentPos, StringTimeLength);
                CurrentPos += StringTimeLength;
            }

            
            int BodyLength = _Data.Length - CurrentPos;
            byte[] BodyBytes = new byte[BodyLength];
            Google.Protobuf.ByteString BodyByteString = Google.Protobuf.ByteString.Empty;
            if (BodyLength > 0)
            {
                Buffer.BlockCopy(_Data,CurrentPos, BodyBytes, 0 , BodyLength);

                int CurrentBodyPos = 0;
                int BodyHeadLength = sizeof(int);
                if(CurrentBodyPos + BodyHeadLength > BodyBytes.Length)
                {
                    Debug.LogError("出错啦，快点排查");
                    return null;
                }

                int RestLength = BitConverter.ToInt32(BodyBytes , CurrentBodyPos);
                CurrentBodyPos += BodyHeadLength;

                string BodyStr = Encoding.UTF8.GetString(BodyBytes , CurrentBodyPos , RestLength);
                BodyByteString = Google.Protobuf.ByteString.FromBase64(BodyStr);
            }

            ReceiveStruct Temp = new ReceiveStruct(StatusCode, MsgId, Description, ActionID, TimeStr, BodyByteString);
            return Temp;
        }

        void ResetData()
        {
            string Session = LocalPlayerData.GetInstance().Data_Session.Data;
            int UserID = LocalPlayerData.GetInstance().Data_UserID.Data;
            string Token = LocalPlayerData.GetInstance().Data_Token.Data;
            m_PostData = "";
            m_UserData = string.Format("MsgId={0}&Sid={1}&Uid={2}&St={3}&Token={4}", m_SendCounter, Session, UserID, "", Token);
            m_SendCounter++;
        }

        void CombineCoreData(int _MsgID, Google.Protobuf.IMessage _ProtoData)
        {
            m_UserData += string.Format("&{0}={1}", "actionId", _MsgID);
            m_UserData += string.Format("&{0}=", "data");
            string TempByteString = Google.Protobuf.MessageExtensions.ToByteString(_ProtoData).ToBase64();
            m_UserData += UnityWebRequest.EscapeURL(TempByteString);
            byte[] FinalData = ConvertFinalDataToByte();
            m_NativeSocket.TryToSendMsg(FinalData);
        }

        byte[] ConvertFinalDataToByte()
        {
            m_PostData = "?d=";
            string Temp = m_UserData + "&sign=" + Md5Sum(m_UserData + m_EncodeKey);
            m_PostData += UnityWebRequest.EscapeURL(Temp);
            byte[] TempData = Encoding.ASCII.GetBytes(m_PostData);
            byte[] len = BitConverter.GetBytes(TempData.Length);
            byte[] sendBytes = new byte[TempData.Length + len.Length];
            Buffer.BlockCopy(len,0,sendBytes,0,len.Length);
            Buffer.BlockCopy(TempData, 0, sendBytes, len.Length, TempData.Length);
            return sendBytes;
        }


        string Md5Sum(string strToEncrypt)
        {
            byte[] bs = UTF8Encoding.UTF8.GetBytes(strToEncrypt);
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();

            byte[] hashBytes = md5.ComputeHash(bs);

            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            return hashString.PadLeft(32, '0');
        }


    }

}
