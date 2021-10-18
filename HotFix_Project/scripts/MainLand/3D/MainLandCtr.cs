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

            ILMonoBehaviour Mono = m_GameObj.AddComponent<ILMonoBehaviour>();
            Mono.OnUpdate = Update;

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

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit, float.MaxValue, LayerMask.GetMask("Touch3D")))
                {
                    OnFunctionObjClicked(Hit.collider.gameObject);
                }
            }
        }

        void OnFunctionObjClicked(GameObject _FunctionObj)
        {
            Vector3 CurrentScale = _FunctionObj.transform.localScale;
            Vector3 ScaleTo = CurrentScale * 1.1f;
            _FunctionObj.transform.DOKill(true);
            Sequence Seq = DOTween.Sequence();
            Seq.Append(_FunctionObj.transform.DOScale(ScaleTo, 0.1f));
            Seq.Append(_FunctionObj.transform.DOScale(CurrentScale, 0.1f));
            if (_FunctionObj.name == "Home")
            {
                
            }
        }

        void InitBtnAnm()
        {
            m_Btns = new List<GameObject>();
            GameObject HomeObj = m_FunctionObj.transform.Find("Home").gameObject;
            GameObject temp = HomeObj.transform.Find("Btn").gameObject;
            m_Btns.Add(temp);
            for (int i = 0; i < m_Btns.Count; i++)
            {

            }
        }
    }
}
