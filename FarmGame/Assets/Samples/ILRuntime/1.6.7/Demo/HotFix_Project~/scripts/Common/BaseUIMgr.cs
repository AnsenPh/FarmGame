using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public abstract class BaseUIMgr
    {
        public GameObject m_GameObj;
        List<BaseUIMgr> m_SubUIScripts = new List<BaseUIMgr>();
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

        //向当前节点添加子节点UI
        void AddSubUI(string _ClassName , bool _ShowOrHide)
        {
            BaseUIMgr TempScripts = UIMgr.Instance.ShowSubUI(_ClassName, _ShowOrHide, m_GameObj.transform);
            m_SubUIScripts.Add(TempScripts);
        }

        public virtual void Delete()
        {
            for(int i = 0; i < m_SubUIScripts.Count; i++)
            {
                m_SubUIScripts[i].Delete();
            }
            m_SubUIScripts.Clear();

            GameObject.Destroy(m_GameObj);
        }


        //把prefab中的需要被控制的各种UI组件与代码关联起来
        public abstract void InitGameObjParam();
        //添加监听事件
        public abstract void AddDataListener();
        //移除监听事件
        public abstract void RemoveDataListener();
    }
}

