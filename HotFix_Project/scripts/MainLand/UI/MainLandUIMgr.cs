using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFix_Project
{
    class MainLandUIMgr : BaseUIMgr
    {
        public override void InitGameObjParam()
        {
            InitBtns();

        }
        //添加监听事件
        public override void AddDataListener()
        {

        }
        //移除监听事件
        public override void RemoveDataListener()
        {

        }

        void InitBtns()
        {
            BaseButton Bag = m_GameObj.transform.Find("Bag").GetComponent<BaseButton>();
            BaseButton Sign = m_GameObj.transform.Find("Sign").GetComponent<BaseButton>();
            BaseButton Mail = m_GameObj.transform.Find("Mail").GetComponent<BaseButton>();
            BaseButton Shop = m_GameObj.transform.Find("Shop").GetComponent<BaseButton>();

            Bag.SetClickCallback(OnBtnsClicked , (int)MainLandConst.BtnType.Bag);
            Sign.SetClickCallback(OnBtnsClicked , (int)MainLandConst.BtnType.Sign);
            Mail.SetClickCallback(OnBtnsClicked , (int)MainLandConst.BtnType.Mail);
            Shop.SetClickCallback(OnBtnsClicked , (int)MainLandConst.BtnType.Shop);
        }

        void OnBtnsClicked(int _Data)
        {
            Debug.LogWarning("_Data===" + _Data);
            MainLandConst.BtnType ClickedData = (MainLandConst.BtnType)_Data; 
            switch(ClickedData)
            {
                case MainLandConst.BtnType.Bag:
                    {
                        
                    }
                    break;
                case MainLandConst.BtnType.Sign:
                    {

                    }
                    break;
                case MainLandConst.BtnType.Mail:
                    {

                    }
                    break;
                case MainLandConst.BtnType.Shop:
                    {

                    }
                    break;
            }
        }
    }
}
