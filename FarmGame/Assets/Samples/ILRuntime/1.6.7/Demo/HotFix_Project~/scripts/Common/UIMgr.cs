using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace HotFix_Project
{
    public class UIMgr
    {
        static UIMgr instance;

        public static UIMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIMgr();
                }
                return instance;
            }
        }

        public void ShowRootUI(string _ClassName , bool _Show)
        {
            BaseUIMgr CurrentScript = GetBaseUIScripts(_ClassName);
            if(CurrentScript == null)
            {
                KeyValuePair<string, PrefabInfo> PrefabInfo = GetPrefabInfo(m_RootUIInfo,_ClassName);
                string PrefabName = PrefabInfo.Key;
                string Path = PrefabInfo.Value.m_Path;
                GameObject Prefab = ABManager.LoadAssetFromAB_GameObject(Path, PrefabName);
                System.Type ClassType = System.Type.GetType("HotFix_Project." + _ClassName);
                BaseUIMgr ClassObj = Activator.CreateInstance(ClassType) as BaseUIMgr;
                if (ClassObj == null)
                {
                    Debug.LogError(PrefabName + "这个预制体对应的类名无法生成，类名====" + _ClassName);
                }
                else
                {
                    m_RootUIScripts.Add(ClassObj);
                    ClassObj.SetGameObj(Prefab, m_RootUIObj.transform);
                }
            }
            else
            {
                CurrentScript.Show(_Show);
            }
        }

        public void ShowWindowUI(string _ClassName, bool _Show)
        {
            BaseWindow CurrentWindowScripts = GetWindowScripts(_ClassName);
            if(CurrentWindowScripts==null)
            {
                KeyValuePair<string, PrefabInfo> PrefabInfo = GetPrefabInfo(m_WindowUIInfo, _ClassName);
                string PrefabName = PrefabInfo.Key;
                string Path = PrefabInfo.Value.m_Path;

                GameObject Prefab = ABManager.LoadAssetFromAB_GameObject(Path, PrefabName);
                System.Type ClassType = System.Type.GetType("HotFix_Project." + _ClassName);
                BaseWindow ClassObj = Activator.CreateInstance(ClassType) as BaseWindow;
                if (ClassObj == null)
                {
                    Debug.LogError(PrefabName + "这个窗口预制体对应的类名无法生成，类名====" + _ClassName);
                }
                else
                {
                    m_WindowUIScripts.Add(ClassObj);
                    ClassObj.SetGameObj(Prefab, m_WindowUIObj.transform);
                }
            }
            else
            {
                CurrentWindowScripts.Show(_Show);
            }

        }


        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///以下是本类的内部实现 外部不会需要调用的
        ///////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //所有UI的根节点
        GameObject m_CanvasObj = null;
        //主UI的根节点
        GameObject m_RootUIObj = null;
        //次级UI的根节点
        GameObject m_WindowUIObj = null;

        //存放所有主UI的list。 什么是主UI？相互之间不可能同时显示的大UI，例如登录UI和大厅UI就是主UI，
        //这两个UI中打开的各种弹窗就不是主UI
        List<BaseUIMgr> m_RootUIScripts;
        //存放所有次级UI的LIST
        //主UI中被打开的各种次级弹窗 就是次级UI
        List<BaseWindow> m_WindowUIScripts;
        
        
        struct PrefabInfo
        {
            public PrefabInfo(string _ClassName , string _Path)
            {
                m_ClassName = _ClassName;
                m_Path = _Path;
            }
            public string m_ClassName;
            public string m_Path;
        }
        //第一个参数是对应UI的Root Prefab，第二个参数是控制这个prefab的脚本名字和这个prefab的路径
        Dictionary<string, PrefabInfo> m_RootUIInfo; //用于主UI
        Dictionary<string, PrefabInfo> m_WindowUIInfo; //用于弹窗

        private UIMgr()
        {
            InitRootObj();
            InitList();
        }

        private void InitRootObj()
        {
            m_CanvasObj = GameObject.Find("Canvas");
            m_RootUIObj = m_CanvasObj.transform.Find("RootUI").gameObject;
            m_WindowUIObj = m_CanvasObj.transform.Find("WindowUI").gameObject;
        }

        private void InitList()
        {
            m_RootUIScripts = new List<BaseUIMgr>();
            m_WindowUIScripts = new List<BaseWindow>();
            m_RootUIInfo = new Dictionary<string, PrefabInfo>();
            m_RootUIInfo.Add("LoginUI",new PrefabInfo("LoginMgr", "src/login"));
            m_RootUIInfo.Add("LoadingUI", new PrefabInfo("LoadingMgr", "src/Loading"));

            m_WindowUIInfo = new Dictionary<string, PrefabInfo>();
            m_WindowUIInfo.Add("TestWindow", new PrefabInfo("TestWindow", "src/login"));
        }

        BaseWindow GetWindowScripts(string _ClassName)
        {

            for (int i = 0; i < m_WindowUIScripts.Count; i++)
            {
                string CurrentClassName = m_WindowUIScripts[i].GetType().Name;
                if (CurrentClassName == _ClassName)
                {
                    return m_WindowUIScripts[i];
                }
            }

            return null;
        }

        BaseUIMgr GetBaseUIScripts(string _ClassName)
        {
            for (int i = 0; i < m_RootUIScripts.Count; i++)
            {
                string CurrentClassName = m_RootUIScripts[i].GetType().Name;
                if (CurrentClassName == _ClassName)
                {
                    return m_RootUIScripts[i];
                }
            }

            return null;
        }

        KeyValuePair<string, PrefabInfo> GetPrefabInfo(Dictionary<string, PrefabInfo> _InfoList , string _ClassName)
        {
            for(int i = 0; i < _InfoList.Count; i++)
            {
                KeyValuePair<string, PrefabInfo> element = _InfoList.ElementAt(i);
                string Key = element.Key;
                PrefabInfo Value = element.Value;
                if(Value.m_ClassName == _ClassName)
                {
                    return element;
                }
            }

            Debug.LogError("没有找到当前名字的类 === " + _ClassName);
            return new KeyValuePair<string, PrefabInfo>();
        }
    }
}

