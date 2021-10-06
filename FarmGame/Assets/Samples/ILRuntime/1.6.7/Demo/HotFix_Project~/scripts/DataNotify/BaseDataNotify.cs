using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;


namespace HotFix_Project
{
    public abstract class BaseDataNotify
    {

        public void RemoveAllListenerByTarget(object _Target)
        {
            FieldInfo[] array = GetType().GetFields();
            foreach (var item in array)
            {
                Type tempType = item.FieldType;
                if (tempType == typeof(BaseData<int>))
                {
                    ((BaseData<int>)item.GetValue(this)).RemoveListenerByTarget(_Target);
                }
                else if (tempType == typeof(BaseData<float>))
                {
                    ((BaseData<float>)item.GetValue(this)).RemoveListenerByTarget(_Target);
                }
                else if (tempType == typeof(BaseData<bool>))
                {
                    ((BaseData<bool>)item.GetValue(this)).RemoveListenerByTarget(_Target);
                }
                else if (tempType == typeof(BaseData<string>))
                {
                    ((BaseData<string>)item.GetValue(this)).RemoveListenerByTarget(_Target);
                }
            }
        }

        public void RemoveAllListener()
        {
            FieldInfo[] array = GetType().GetFields();
            foreach (var item in array)
            {
                Type tempType = item.FieldType;
                if (tempType == typeof(BaseData<int>))
                {
                    ((BaseData<int>)item.GetValue(this)).RemoveAllListenner();
                }
                else if (tempType == typeof(BaseData<float>))
                {
                    ((BaseData<float>)item.GetValue(this)).RemoveAllListenner();
                }
                else if (tempType == typeof(BaseData<bool>))
                {
                    ((BaseData<bool>)item.GetValue(this)).RemoveAllListenner();
                }
                else if (tempType == typeof(BaseData<string>))
                {
                    ((BaseData<string>)item.GetValue(this)).RemoveAllListenner();
                }
            }
        }

        public void ResetData()
        {
            FieldInfo[] array = GetType().GetFields();
            foreach (var item in array)
            {
                Type tempType = item.FieldType;
                if (tempType == typeof(BaseData<int>))
                {
                    ((BaseData<int>)item.GetValue(this)).Reset();
                }
                else if (tempType == typeof(BaseData<float>))
                {
                    ((BaseData<float>)item.GetValue(this)).Reset();
                }
                else if (tempType == typeof(BaseData<bool>))
                {
                    ((BaseData<bool>)item.GetValue(this)).Reset();
                }
                else if (tempType == typeof(BaseData<string>))
                {
                    ((BaseData<string>)item.GetValue(this)).Reset();
                }
            }
        }

    }

}
