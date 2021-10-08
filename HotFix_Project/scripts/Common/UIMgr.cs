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

        public BaseUIMgr ShowUI(string _ClassName , bool _Show , Transform _Parent)
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

        public BaseUIMgr ShowRootUI(string _ClassName, bool _Show, bool _HidePrevious = true)
        {
            BaseUIMgr TempScripts = ShowUI(_ClassName, _Show, m_RootUIObj.transform);
            m_RootUI.Add(TempScripts);

            if(_HidePrevious)
            {
                for (int i = 0; i < m_RootUI.Count - 1; i++)
                {
                    m_RootUI[i].Show(false);
                }
            }


            return TempScripts;
        }

        public BaseUIMgr ShowWindowUI(string _ClassName, bool _Show )
        {
            BaseUIMgr TempScripts = ShowUI(_ClassName, _Show, m_WindowUIObj.transform);
            m_WindowUI.Add(TempScripts);
            return TempScripts;
        }

        public BaseUIMgr ShowSubUI(string _ClassName, bool _Show , Transform _Parent)
        {
            BaseUIMgr TempScripts = ShowUI(_ClassName, _Show, _Parent);
            return TempScripts;
        }

        public void HideAllRootUI(bool Destroy = true)
        {
            if(Destroy)
            {
                while (m_RootUI.Count > 1)
                {
                    m_RootUI[0].Delete();
                    m_RootUI[0] = null;
                    m_RootUI.RemoveAt(0);
                }
            }
            else
            {
                for(int i = 0; i < m_RootUI.Count - 1; i++)
                {
                    m_RootUI[i].Show(false);
                }
            }

        }

        public void HideAllWindow(bool Destroy = true)
        {
            if (Destroy)
            {
                while (m_WindowUI.Count > 1)
                {
                    m_WindowUI[0].Delete();
                    m_WindowUI[0] = null;
                    m_WindowUI.RemoveAt(0);
                }
            }
            else
            {
                for (int i = 0; i < m_WindowUI.Count - 1; i++)
                {
                    m_WindowUI[i].Show(false);
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
        List<BaseUIMgr> m_WindowUI;
        List<BaseUIMgr> m_RootUI;


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
            m_WindowUI = new List<BaseUIMgr>();
            m_RootUI = new List<BaseUIMgr>();
            m_UIInfo = new Dictionary<string, PrefabInfo>();
            m_UIInfo.Add("LoginMgr", new PrefabInfo("LoginUI", "src/login"));
            m_UIInfo.Add("TestWindow", new PrefabInfo("TestWindow", "src/login"));
            m_UIInfo.Add("LoadingMgr", new PrefabInfo("LoadingUI", "src/Loading"));
            m_UIInfo.Add("LoginList", new PrefabInfo("LoginList", "src/login"));
        }

    }
}

