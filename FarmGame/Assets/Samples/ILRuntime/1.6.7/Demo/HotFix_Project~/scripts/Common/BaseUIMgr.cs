using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public abstract class BaseUIMgr
    {
        public GameObject m_GameObj;

        public BaseUIMgr()
        {

        }

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
        //��prefab�е���Ҫ�����Ƶĸ���UI���������������
        public abstract void InitGameObjParam();
        //��Ӽ����¼�
        public abstract void AddDataListener();
        //�Ƴ������¼�
        public abstract void RemoveDataListener();
    }
}

