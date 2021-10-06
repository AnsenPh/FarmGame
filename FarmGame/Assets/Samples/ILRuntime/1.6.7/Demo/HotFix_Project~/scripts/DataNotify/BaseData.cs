using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HotFix_Project
{
    public class BaseData<T>
    {
        public BaseData()
        {
            m_HasEverSetValue = false;
        }

        private T m_Data;
        public T Data
        {
            get
            {
                return m_Data;
            }
            set
            {
                m_Data = value;
                m_HasEverSetValue = true;
                TriggerCallback();
            }
        }

        public void SetData(T _Data)
        {
            m_Data = _Data;
            m_HasEverSetValue = true;
            TriggerCallback();
        }

        private bool m_HasEverSetValue;
        private Dictionary<object, Action<T>> m_CallbackDic = new Dictionary<object, Action<T>>();

        public void AddListner(object _Target, Action<T> _Callback)
        {
            if (m_CallbackDic.ContainsKey(_Target))
            {
                Debug.Log("当前Target已经监听过这个数据了======" + _Target);
            }
            else
            {
                m_CallbackDic.Add(_Target, _Callback);
                if (m_HasEverSetValue)
                {
                    _Callback(m_Data);
                }
            }
        }

        public void RemoveListenerByTarget(object _Target)
        {
            if (!m_CallbackDic.ContainsKey(_Target))
            {
                return;
            }
            m_CallbackDic[_Target] = null;
            m_CallbackDic.Remove(_Target);
        }


        private void TriggerCallback()
        {
            foreach (KeyValuePair<object, Action<T>> _Pair in m_CallbackDic)
            {
                _Pair.Value(m_Data);
            }
        }

        public void RemoveAllListenner()
        {
            m_CallbackDic.Clear();
        }

        public void Reset()
        {
            m_HasEverSetValue = false;
            RemoveAllListenner();
        }
    }
}
    


