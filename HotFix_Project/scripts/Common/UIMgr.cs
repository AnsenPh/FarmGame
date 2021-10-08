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

        public BaseUIMgr NewPrefab(string _ClassName , Transform _Parent)
        {
            PrefabInfo PrefabInfo = m_UIInfo[_ClassName];
            string PrefabName = PrefabInfo.m_PrefabName;
            string Path = PrefabInfo.m_Path;
            GameObject Prefab = ABManager.LoadAssetFromAB_GameObject(Path, PrefabName);
            System.Type ClassType = System.Type.GetType("HotFix_Project." + _ClassName);
            BaseUIMgr ClassObj = Activator.CreateInstance(ClassType) as BaseUIMgr;
            if (ClassObj == null)
            {
                Debug.LogError(PrefabName + "这个预制体对应的类名无法生成，类名====" + _ClassName);
                return null;
            }
            else
            {
                ClassObj.SetGameObj(Prefab, _Parent);
                return ClassObj;
            }
        }

        public BaseUIMgr ShowRootUI(string _ClassName, bool _Show)
        {
            if(m_RootUI.ContainsKey(_ClassName))
            {
                BaseUIMgr CurrentScripts = m_RootUI[_ClassName];
                CurrentScripts.Show(_Show);
                return CurrentScripts;
            }
            else
            {
                BaseUIMgr TempScripts = NewPrefab(_ClassName, m_RootUIObj.transform);
                m_RootUI.Add(_ClassName , TempScripts);
                TempScripts.Show(_Show);
                return TempScripts;
            }
        }

        public BaseUIMgr ShowWindowUI(string _ClassName, bool _Show )
        {
            if(m_WindowUI.ContainsKey(_ClassName))
            {
                BaseUIMgr CurrentScripts = m_WindowUI[_ClassName];
                CurrentScripts.Show(_Show);
                return CurrentScripts;
            }
            else
            {
                BaseUIMgr TempScripts = NewPrefab(_ClassName, m_WindowUIObj.transform);
                m_WindowUI.Add(_ClassName , TempScripts);
                TempScripts.Show(_Show);
                return TempScripts;
            }
        }

        public void DeletePreviousRootUI(bool HideOnly = false)
        {
            if(HideOnly == false)
            {
                while (m_RootUI.Count > 1)
                {
                    string Key = m_RootUI.ElementAt(0).Key;
                    BaseUIMgr Value = m_RootUI.ElementAt(0).Value;
                    Value.Delete();
                    m_RootUI[Key] = null;
                    m_RootUI.Remove(Key);
                }
            }
            else
            {
                for(int i = 0; i < m_RootUI.Count - 1; i++)
                {
                    m_RootUI.ElementAt(i).Value.Show(false);
                }
            }
        }

        public void DeleteAllRootUI()
        {
            while (m_RootUI.Count > 0)
            {
                string Key = m_RootUI.ElementAt(0).Key;
                BaseUIMgr Value = m_RootUI.ElementAt(0).Value;
                Value.Delete();
                m_RootUI[Key] = null;
                m_RootUI.Remove(Key);
            }
        }

        public void DeleteAllWindow(bool HideOnly = false)
        {
            if (HideOnly == false)
            {
                while (m_WindowUI.Count > 0)
                {
                    string Key = m_WindowUI.ElementAt(0).Key;
                    BaseUIMgr Value = m_WindowUI.ElementAt(0).Value;
                    Value.Delete();
                    m_WindowUI[Key] = null;
                    m_WindowUI.Remove(Key);
                }
            }
            else
            {
                for (int i = 0; i < m_WindowUI.Count; i++)
                {
                    m_WindowUI.ElementAt(i).Value.Show(false);
                }
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

        //存放所有UI代码的list。 
        Dictionary<string , BaseUIMgr> m_WindowUI;
        Dictionary<string , BaseUIMgr> m_RootUI;


        struct PrefabInfo
        {
            public PrefabInfo(string PrefabName , string _Path)
            {
                m_PrefabName = PrefabName;
                m_Path = _Path;
            }
            public string m_PrefabName;
            public string m_Path;
        }
        //第一个参数是对应UI的脚本名称，第二个参数是控制这个prefab的名字和这个prefab的路径
        Dictionary<string, PrefabInfo> m_UIInfo; //用于主UI


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
            m_WindowUI = new Dictionary<string, BaseUIMgr>();
            m_RootUI = new Dictionary<string, BaseUIMgr>();
            m_UIInfo = new Dictionary<string, PrefabInfo>();
            m_UIInfo.Add("LoginMgr", new PrefabInfo("LoginUI", "src/login/ui"));
            m_UIInfo.Add("TestWindow", new PrefabInfo("TestWindow", "src/login/ui"));
            m_UIInfo.Add("LoadingMgr", new PrefabInfo("LoadingUI", "src/Loading"));
            m_UIInfo.Add("LoginList", new PrefabInfo("LoginList", "src/login/ui"));
        }

    }
}

