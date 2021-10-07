using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace HotFix_Project
{
    public class LoginMgr : BaseUIMgr
    {
        public BaseButton m_LoginInBtn;
        public LoginMgr()
        {

        }
        public override void InitGameObjParam()
        {
            m_LoginInBtn = m_GameObj.transform.Find("LoginBtn").GetComponent<BaseButton>();
            m_LoginInBtn.SetClickCallback(OnLoginInBtn);

            ////////////////////////////////////////////////////////////////////////////
            /////                   如何接管Mono的方法
            ////////////////////////////////////////////////////////////////////////////
            //ILMonoBehaviour Mono = m_GameObj.AddComponent<ILMonoBehaviour>();
            //Mono.OnUpdate = Update;
            ////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////////
            /////                   如何使用携程
            ////////////////////////////////////////////////////////////////////////////
            //ILRunTimeStart.GetInstance().DoCoroutine(CoroutineTest());
            ////////////////////////////////////////////////////////////////////////////



        }

        public IEnumerator CoroutineTest()
        {
            Debug.Log("开始协程,t=" + Time.time);
            yield return new WaitForSeconds(3);
            Debug.Log("等待了3秒,t=" + Time.time);
        }

        void Update()
        {
            Debug.Log("Update");
        }

        public override void AddDataListener()
        {
            LoginDataNotify.GetInstance().Data_TestWindowShow.AddListner(this, TestWindowShow_CB);
            LoginDataNotify.GetInstance().Data_TestObject.AddListner(this, TestObject_CB);
            
        }

        public override void RemoveDataListener()
        {
            LoginDataNotify.GetInstance().RemoveAllListenerByTarget(this);
        }

        void OnLoginInBtn(int _Data)
        {
            LoginDataNotify.GetInstance().Data_TestWindowShow.SetData(true);


            ReceiveStruct gg = new ReceiveStruct();
            LoginDataNotify.GetInstance().Data_TestObject.SetData(gg);
        }

        void TestWindowShow_CB(bool _Result)
        {
            UIMgr.GetInstance().ShowWindowUI("TestWindow", _Result);
        }

        void TestObject_CB(object _Result)
        {
            ReceiveStruct GG = _Result as ReceiveStruct;
            Debug.Log("GG.m_StatusCode====" + GG.m_StatusCode);
            //UIMgr.GetInstance().ShowWindowUI("TestWindow", _Result);
        }
    }
}

