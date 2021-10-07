using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    public class TestWindow: BaseWindow
    {
        public BaseButton m_CloseWindowBtn;
        public override void InitGameObjParam()
        {
            m_CloseWindowBtn = m_GameObj.transform.Find("CloseBtn").GetComponent<BaseButton>();
            m_CloseWindowBtn.SetClickCallback(OnCloseWindow);
        }
        //���Ӽ����¼�
        public override void AddDataListener()
        {

        }
        //�Ƴ������¼�
        public override void RemoveDataListener()
        {

        }

        public void OnCloseWindow(int _Data)
        {
            Debug.Log("OnCloseWindow"); ;
            LoginDataNotify.GetInstance().Data_TestWindowShow.SetData(false);
        }
    }

}