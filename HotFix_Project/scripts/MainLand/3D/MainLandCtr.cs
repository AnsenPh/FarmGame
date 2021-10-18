using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
namespace HotFix_Project
{
    class MainLandCtr : BaseUIMgr
    {
        List<GameObject> m_Btns;
        GameObject m_FunctionObj;
        public override void InitGameObjParam()
        {
            m_FunctionObj = m_GameObj.transform.Find("Function").gameObject;
            InitBtnAnm();
        }
        //添加监听事件
        public override void AddDataListener()
        {

        }
        //移除监听事件
        public override void RemoveDataListener()
        {

        }


        void InitBtnAnm()
        {
            m_Btns = new List<GameObject>();
            GameObject HomeObj = m_FunctionObj.transform.Find("Home").gameObject;
            GameObject temp = HomeObj.transform.Find("Btn").gameObject;
            m_Btns.Add(temp);
            for (int i = 0; i < m_Btns.Count; i++)
            {
                GameObject Current = m_Btns[i];
                float CurrentY = Current.transform.localPosition.y;
                float TopY = CurrentY + 50;
                float BottomY = CurrentY - 50;
                Sequence Seq = DOTween.Sequence();
                Seq.Append(Current.transform.DOLocalMoveY(TopY, 1));
                Seq.Append(Current.transform.DOLocalMoveY(CurrentY, 1));
                Seq.Append(Current.transform.DOLocalMoveY(BottomY, 1));
                Seq.Append(Current.transform.DOLocalMoveY(CurrentY, 1));
                Seq.SetLoops(-1, LoopType.Restart);
            }
        }
    }
}
