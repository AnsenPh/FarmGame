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
            KeyValuePair<string, PrefabInfo> PrefabInfo = GetPrefabInfo(_ClassName);
            string PrefabName = PrefabInfo.Key;
            string Path = PrefabInfo.Value.m_Path;
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
            BaseUIMgr TempScripts = ShowUI(_ClassName, _Show, m_RootUIObj.transform);
            m_RootUI.Add(TempScripts);
            return TempScripts;
        }

        public BaseUIMgr ShowWindowUI(string _ClassName, bool _Show)
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
            public PrefabInfo(string _ClassName , string _Path)
            {
                m_ClassName = _ClassName;
                m_Path = _Path;
            }
            public string m_ClassName;
            public string m_Path;
        }
        //第一个参数是对应UI的Prefab，第二个参数是控制这个prefab的脚本名字和这个prefab的路径
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
            m_UIInfo.Add("LoginUI",new PrefabInfo("LoginMgr", "src/login"));
            m_UIInfo.Add("TestWindow", new PrefabInfo("TestWindow", "src/login"));
            m_UIInfo.Add("LoadingUI", new PrefabInfo("LoadingMgr", "src/Loading"));
        }



        KeyValuePair<string, PrefabInfo> GetPrefabInfo( string _ClassName)
        {
            for(int i = 0; i < m_UIInfo.Count; i++)
            {
                KeyValuePair<string, PrefabInfo> element = m_UIInfo.ElementAt(i);
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

