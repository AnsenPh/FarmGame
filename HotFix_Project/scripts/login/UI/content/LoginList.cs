using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace HotFix_Project
{
    class LoginList : BaseUIMgr
    {
        ScrollRect m_ScrollView;
        public LoginList()
        {

        }
        public override void InitGameObjParam()
        {
            m_ScrollView = m_GameObj.transform.Find("ScrollView").GetComponent<ScrollRect>();
            //在这里找到所有的按钮，并且绑定点击事件，然后在事件回调中处理业务逻辑

        }


        public override void AddDataListener()
        {

        }

        public override void RemoveDataListener()
        {
         
        }
    }
}
