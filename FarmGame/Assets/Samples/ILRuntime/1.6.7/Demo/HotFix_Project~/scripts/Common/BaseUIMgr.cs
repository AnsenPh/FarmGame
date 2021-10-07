using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public abstract class BaseUIMgr
    {
        public GameObject m_GameObj;

        public virtual void SetGameObj(GameObject _Prefab , Transform _Parent)
        {
            m_GameObj = GameObject.Instantiate(_Prefab);
            m_GameObj.transform.SetParent(_Parent);
            m_GameObj.transform.localPosition = _Prefab.transform.position;
            m_GameObj.transform.localRotation = _Prefab.transform.rotation;
            m_GameObj.transform.localScale = _Prefab.transform.localScale;

            InitGameObjParam();
            AddDataListener();
        }
        public virtual void Show(bool _Show)
        {
            m_GameObj.SetActive(_Show);
        }
        //把prefab中的需要被控制的各种UI组件与代码关联起来
        public abstract void InitGameObjParam();
        //添加监听事件
        public abstract void AddDataListener();
        //移除监听事件
        public abstract void RemoveDataListener();
    }
}

