using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HotFix_Project
{
    public abstract class BaseUIMgr
    {
        public GameObject m_GameObj;
        List<BaseUIMgr> m_AddtionalScripts = new List<BaseUIMgr>();
        List<GameObject> m_AddtionalObj = new List<GameObject>();
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
            BaseUIMgr TempScripts = NewPrefab(_ClassName, m_GameObj.transform);
            TempScripts.Show(_ShowOrHide);
            return TempScripts;
        }

        public BaseUIMgr NewPrefab(string _ClassName , Transform _Parent = null)
        {
            BaseUIMgr TempScripts = UIMgr.Instance.NewPrefab(_ClassName, _Parent);
            m_AddtionalScripts.Add(TempScripts);
            return TempScripts;
        }

        public void NewPrefabAsync(string _ClassName ,Transform _Parent , System.Action<BaseUIMgr> _FinishCallback, System.Action<float> _UpdateCallBack = null)
        {
            UIMgr.Instance.NewPrefabAsync(_ClassName, _Parent, (Script) =>
            {
                m_AddtionalScripts.Add(Script);
                if(_FinishCallback != null)
                {
                    _FinishCallback(Script);
                }
            } , _UpdateCallBack);
        }

        public GameObject NewPrefabWithoutCode(string _Path , string _FileName)
        {
            GameObject TempObj = ABManager.LoadAssetFromAB(_Path , _FileName) as GameObject;
            m_AddtionalObj.Add(TempObj);
            return TempObj;
        }

        public virtual void Delete()
        {
            RemoveDataListener();
            while(m_AddtionalScripts.Count>0)
            {
                m_AddtionalScripts[0].Delete();
                m_AddtionalScripts[0] = null;
                m_AddtionalScripts.RemoveAt(0);
            }
            m_AddtionalScripts.Clear();
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

