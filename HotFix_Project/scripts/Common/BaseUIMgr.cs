using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HotFix_Project
{
    public abstract class BaseUIMgr
    {
        public GameObject m_GameObj;
        Dictionary<string, BaseUIMgr> m_SubUIScripts = new Dictionary<string, BaseUIMgr>();
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
        public BaseUIMgr AddSubUI(string _ClassName , bool _ShowOrHide)
        {
            if(m_SubUIScripts.ContainsKey(_ClassName))
            {
                Debug.LogWarning("这个子prefab已经添加过了！！请别重复添加===" + _ClassName);
                return null;
            }
            BaseUIMgr TempScripts = UIMgr.Instance.NewPrefab(_ClassName, m_GameObj.transform);
            TempScripts.Show(_ShowOrHide);
            m_SubUIScripts.Add(_ClassName , TempScripts);
            return TempScripts;
        }

        public BaseUIMgr NewSubPrefab(string _ClassName)
        {
            BaseUIMgr TempScripts = UIMgr.Instance.NewPrefab(_ClassName,null);
            return TempScripts;
        }

        public virtual void Delete()
        {
            RemoveDataListener();
            for (int i = 0; i < m_SubUIScripts.Count; i++)
            {
                string Key = m_SubUIScripts.ElementAt(i).Key;
                BaseUIMgr Value = m_SubUIScripts.ElementAt(i).Value;
                Value.Delete();
                m_SubUIScripts[Key] = null;
            }
            m_SubUIScripts.Clear();
            GameObject.Destroy(m_GameObj);
            m_GameObj = null;
        }


        //把prefab中的需要被控制的各种UI组件与代码关联起来
        public abstract void InitGameObjParam();
        //添加监听事件
        public abstract void AddDataListener();
        //移除监听事件
        public abstract void RemoveDataListener();
    }
}

