using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
namespace HotFix_Project
{
    class MainLand3DMgr : BaseUIMgr
    {
        List<GameObject> m_Btns;
        GameObject m_FunctionObj;
        Camera m_Camera;

        string m_ClickDownObjName = "";
        public override void InitGameObjParam()
        {
            m_FunctionObj = m_GameObj.transform.Find("Function").gameObject;
            m_Camera = m_GameObj.transform.Find("Camera").GetComponent<Camera>();
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
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit, float.MaxValue, LayerMask.GetMask(CommonConst.CameraTouchLayer)))
                {
                    m_ClickDownObjName = Hit.collider.gameObject.name;
                    PlayScaleAnm(Hit.collider.gameObject);
                }
            }


            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit, float.MaxValue, LayerMask.GetMask(CommonConst.CameraTouchLayer)))
                {
                    if (m_ClickDownObjName == Hit.collider.gameObject.name)
                    {
                        OnFunctionObjClicked(Hit.collider.gameObject);
                    }
                }

                m_ClickDownObjName = "";
            }
        }

        void OnFunctionObjClicked(GameObject _FunctionObj)
        {
            if (_FunctionObj.name == "Home")
            {
                UIMgr.Instance.ChangeScene(null, "RoomUIMgr" , "RoomHallMgr");
            }
        }

        void PlayScaleAnm(GameObject _FunctionObj)
        {
            Vector3 CurrentScale = _FunctionObj.transform.localScale;
            Vector3 ScaleTo = CurrentScale * 1.1f;
            _FunctionObj.transform.DOKill(true);
            Sequence Seq = DOTween.Sequence();
            Seq.Append(_FunctionObj.transform.DOScale(ScaleTo, 0.1f));
            Seq.Append(_FunctionObj.transform.DOScale(CurrentScale, 0.1f));
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
