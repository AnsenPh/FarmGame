using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotFix_Project
{
    public abstract class BaseWindow : BaseUIMgr
    {
        public GameObject m_WindowRootObj;
        bool m_IsAnimating = false;

        public override void SetGameObj(GameObject _Prefab, Transform _Parent)
        {
            InitWindowBG(_Parent);
            m_GameObj = GameObject.Instantiate(_Prefab);
            m_GameObj.transform.SetParent(m_WindowRootObj.transform);
            m_GameObj.transform.localPosition = _Prefab.transform.position;
            m_GameObj.transform.localRotation = _Prefab.transform.rotation;
            m_GameObj.transform.localScale = _Prefab.transform.localScale;

            //给弹窗内容设置点击事件，避免点击事件穿透到黑背景上，因为黑背景会响应点击事件并会关闭弹窗
            EventTrigger trigger = m_GameObj.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = m_GameObj.AddComponent<EventTrigger>();
            }

            List<EventTrigger.Entry> entrys = trigger.triggers;
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(OnWindowContentClicked);
            entrys.Add(entry);


            InitGameObjParam();
            AddDataListener();
            Show(true);
        }

        void InitWindowBG(Transform _Parent)
        {
            GameObject Prefab = ABManager.LoadAssetFromAB("src/common/window", "WindowBG") as GameObject;
            m_WindowRootObj = GameObject.Instantiate(Prefab);
            m_WindowRootObj.transform.SetParent(_Parent);
            m_WindowRootObj.transform.localPosition = Prefab.transform.position;
            m_WindowRootObj.transform.localRotation = Prefab.transform.rotation;
            m_WindowRootObj.transform.localScale = Prefab.transform.localScale;
            AddDarkBGClickEvent();
        }


        //点击黑背景自动关闭弹窗 
        void AddDarkBGClickEvent()
        {
            EventTrigger trigger = m_WindowRootObj.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = m_WindowRootObj.AddComponent<EventTrigger>();
            }
                
            List<EventTrigger.Entry> entrys = trigger.triggers;
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(OnDarkBGClicked);
            entrys.Add(entry);
        }

  
        void OnDarkBGClicked(BaseEventData arg0)
        {
            ExcutiveHide();
        }

        //给弹窗内容设置点击事件，避免点击事件穿透到黑背景上，因为黑背景会响应点击事件并会关闭弹窗
        void OnWindowContentClicked(BaseEventData arg0)
        {

        }

        public override void Show(bool _Show)
        {
            if(m_WindowRootObj.activeSelf == _Show)
            {
                return;
            }

            if (m_IsAnimating)
            {
                return;
            }

            m_IsAnimating = true;

            if (_Show)
            {
                ExcutiveShow();
            }
            else
            {
                ExcutiveHide();
            }
        }

        void ExcutiveShow()
        {
            m_WindowRootObj.SetActive(true);
            m_GameObj.transform.DOKill(true);
            m_GameObj.transform.DOScale(0.01f, 0);
            Sequence Seq = DOTween.Sequence();
            Seq.Append(m_GameObj.transform.DOScale(1.1f, 0.15f));
            Seq.Append(m_GameObj.transform.DOScale(1.0f, 0.075f));
            Seq.AppendCallback(ShowEnd);
        }

        void ShowEnd()
        {
            m_IsAnimating = false;
        }

        void ExcutiveHide()
        {
            m_GameObj.transform.DOKill(true);
            Sequence Seq = DOTween.Sequence();
            Seq.Append(m_GameObj.transform.DOScale(1.1f, 0.1f));
            Seq.Append(m_GameObj.transform.DOScale(0.01f, 0.15f));
            Seq.AppendCallback(HideEnd);
        }

        void HideEnd()
        {
            m_IsAnimating = false;
            m_WindowRootObj.SetActive(false);
        }

        public override void Delete()
        {
            m_GameObj.transform.DOKill(true);
            base.Delete();
            GameObject.Destroy(m_WindowRootObj);
            m_WindowRootObj = null;
        }

    }

}
