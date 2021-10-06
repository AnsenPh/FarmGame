using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HotFix_Project
{
    public abstract class BaseWindow : BaseUIMgr
    {
        public GameObject m_WindowRootObj;
        bool m_IsAnimating = false;
        public BaseWindow()
        {

        }

        public override void SetGameObj(GameObject _Prefab, Transform _Parent)
        {
            InitWindowBG(_Parent);
            m_GameObj = GameObject.Instantiate(_Prefab);
            m_GameObj.transform.SetParent(m_WindowRootObj.transform);
            m_GameObj.transform.localPosition = _Prefab.transform.position;
            m_GameObj.transform.localRotation = _Prefab.transform.rotation;
            m_GameObj.transform.localScale = _Prefab.transform.localScale;

            InitGameObjParam();
            AddDataListener();
            Show(true);
        }

        void InitWindowBG(Transform _Parent)
        {
            GameObject Prefab = ABManager.LoadAssetFromAB_GameObject("src/common/window", "WindowBG");
            m_WindowRootObj = GameObject.Instantiate(Prefab);
            m_WindowRootObj.transform.SetParent(_Parent);
            m_WindowRootObj.transform.localPosition = Prefab.transform.position;
            m_WindowRootObj.transform.localRotation = Prefab.transform.rotation;
            m_WindowRootObj.transform.localScale = Prefab.transform.localScale;
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
    }

}
