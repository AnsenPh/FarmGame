using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;
using System.Text;


public class NativeSocket
{
    string m_Ip;
    int m_Port;

    public Action OnConnect = null;
    public Action OnConnectFailed = null;
    public Action OnClosed = null;
    Action<byte[]> OnMsgRecieved = null;
    Queue<byte[]> m_RecieveQueue;
    Queue<byte[]> m_SendingQueue;

    public delegate void ThredCallback_Delegate();
    ThredCallback_Delegate m_ThredCallback;

    TcpClient m_TcpClient = null;
    NetworkStream m_Stream = null;

    Thread RecieveMsg_Thread;
    Thread SendMsg_Thread;
    public NativeSocket()
    {
        InitThread();
    }

    public void SetRevieveCallback(Action<Byte[]> _Callback)
    {
        OnMsgRecieved = _Callback;
    }

    public void ConnectSocket(string _Ip, int _Port)
    {
        m_Ip = _Ip;
        m_Port = _Port;
        m_TcpClient = new TcpClient();
        m_TcpClient.BeginConnect(m_Ip, m_Port, new AsyncCallback(Connect_Callback), m_TcpClient);
    }

    public void CloseSocket()
    {
        if (m_TcpClient != null)
        {
            if(m_TcpClient.GetStream() != null)
            {
                m_TcpClient.GetStream().Close();
                m_TcpClient.GetStream().Dispose();
            }
            m_TcpClient.Close();
            m_TcpClient.Dispose();
        }

        lock (m_RecieveQueue)
        {
            m_RecieveQueue.Clear();
        }

        lock (m_SendingQueue)
        {
            m_SendingQueue.Clear();
        }

        m_TcpClient = null;

        if (OnClosed != null)
        {
            OnClosed();
        }
    }

    public void TryToSendMsg(byte[] _MsgBuffer)
    {
        lock (m_SendingQueue)
        {
            m_SendingQueue.Enqueue(_MsgBuffer);
        }
    }

    public void DealMsg()
    {
        byte[] CurrentMsg = GetRencentlyRecieveData();

        if(CurrentMsg.Length == 0)
        {
            Debug.LogWarning("不可能到这里，有问题，请排查");
            return;
        }

        if (OnMsgRecieved != null)
        {
            OnMsgRecieved(CurrentMsg);
        }
    }


    byte[] GetRencentlyRecieveData()
    {
        lock (m_RecieveQueue)
        {
            byte[] MsgBuffer = new byte[0];
            if (m_RecieveQueue.Count > 0)
            {
                MsgBuffer = m_RecieveQueue.Dequeue();
            }
            return MsgBuffer;
        }
    }




    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////// 以下为多线程的收发实现 基本不会用到
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void Connect_Callback(IAsyncResult _Result)
    {
        TcpClient CurrentTcp = (TcpClient)_Result.AsyncState;
        try
        {
            CurrentTcp.EndConnect(_Result);
            if (CurrentTcp.Connected)
            {
                if (OnConnect != null)
                {
                    OnConnect();
                }

            }
            else
            {
                if (OnConnectFailed != null)
                {
                    OnConnectFailed();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Connect_Callback error====" + e);
        }
    }


    public void InitThread()
    {
        m_ThredCallback = DealMsg;
        RecieveMsg_Thread = new Thread(RecieveMsg_MethodOfThread);
        RecieveMsg_Thread.IsBackground = true;
        RecieveMsg_Thread.Start(m_ThredCallback);

        SendMsg_Thread = new Thread(SendMsg_MethodOfThread);
        SendMsg_Thread.IsBackground = true;
        SendMsg_Thread.Start();
    }

    public void SendMsg_MethodOfThread()
    {
        try
        {
            while (true)
            {
                if (m_TcpClient != null && m_TcpClient.Connected)
                {
                    if(m_TcpClient.GetStream().CanWrite)
                    {
                        if (m_SendingQueue.Count > 0)
                        {
                            byte[] CurrentMsg = m_SendingQueue.Dequeue();
                            m_TcpClient.GetStream().Write(CurrentMsg, 0, CurrentMsg.Length);
                        }
                    }
                }
                else
                {

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("SendMsg_MethodOfThread error====" + e);
        }
    }

    public void RecieveMsg_MethodOfThread(object obj)
    {
        try
        {
            while (true)
            {
                if (m_TcpClient != null && m_TcpClient.Connected)
                {
                    //int alen = m_TcpClient.Available;

                    if (m_TcpClient.GetStream().CanRead)
                    {
                        int PreffixLength = 4;
                        byte[] preffixBytes = new byte[PreffixLength];
                        int RecievePreffix = m_Stream.Read(preffixBytes, 0, PreffixLength);
                        if(RecievePreffix == PreffixLength)
                        {
                            int DataTotalLength = BitConverter.ToInt32(preffixBytes,0);
                            byte[] fullData = new byte[DataTotalLength];
                            int StartIndex = 0;
                            int CurrentRecieveLength = 0;

                            do
                            {
                                int CurrentRecieve = m_Stream.Read(fullData,StartIndex, DataTotalLength - CurrentRecieveLength);
                                CurrentRecieveLength += CurrentRecieve;
                                StartIndex += CurrentRecieve;
                            }
                            while (CurrentRecieveLength != DataTotalLength);

                            lock (m_RecieveQueue)
                            {
                                m_RecieveQueue.Enqueue(fullData);
                            }

                            ThredCallback_Delegate TempCallback = obj as ThredCallback_Delegate;
                            TempCallback();

                        }
                        //byte[] readBuffer = new byte[1024];
                        //int NumOfReadBytes = 0;
                        //
                        //do
                        //{
                        //    NumOfReadBytes = m_Stream.Read(readBuffer, 0, readBuffer.Length);
                        //    byte[] FinalBytes = new byte[NumOfReadBytes];
                        //    System.Array.Copy(readBuffer, 0, FinalBytes, 0, NumOfReadBytes);
                        //    lock (m_RecieveQueue)
                        //    {
                        //        m_RecieveQueue.Enqueue(FinalBytes);
                        //    }
                        //}
                        //while (m_TcpClient.GetStream().DataAvailable);

                    }
                }
                else
                {

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("RecieveMsg_MethodOfThread error====" + e);
        }
    }
}
