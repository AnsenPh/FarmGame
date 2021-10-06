using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace HotFix_Project
{
    public class UIMgr
    {
        public static UIMgr GetInstance()
        {
            if (Instance == null)
            {
                Instance = new UIMgr();
            }
            return Instance;
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
                    Debug.LogError(PrefabName + "���Ԥ�����Ӧ�������޷����ɣ�����====" + _ClassName);
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
                    Debug.LogError(PrefabName + "�������Ԥ�����Ӧ�������޷����ɣ�����====" + _ClassName);
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
        ///�����Ǳ�����ڲ�ʵ�� �ⲿ������Ҫ���õ�
        ///////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        private static UIMgr Instance = null;
        //����UI�ĸ��ڵ�
        GameObject m_CanvasObj = null;
        //��UI�ĸ��ڵ�
        GameObject m_RootUIObj = null;
        //�μ�UI�ĸ��ڵ�
        GameObject m_WindowUIObj = null;

        //���������UI��list�� ʲô����UI���໥֮�䲻����ͬʱ��ʾ�Ĵ�UI�������¼UI�ʹ���UI������UI��
        //������UI�д򿪵ĸ��ֵ����Ͳ�����UI
        List<BaseUIMgr> m_RootUIScripts;
        //������дμ�UI��LIST
        //��UI�б��򿪵ĸ��ִμ����� ���Ǵμ�UI
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
        //��һ�������Ƕ�ӦUI��Root Prefab���ڶ��������ǿ������prefab�Ľű����ֺ����prefab��·��
        Dictionary<string, PrefabInfo> m_RootUIInfo; //������UI
        Dictionary<string, PrefabInfo> m_WindowUIInfo; //���ڵ���

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

            Debug.LogError("û���ҵ���ǰ���ֵ��� === " + _ClassName);
            return new KeyValuePair<string, PrefabInfo>();
        }
    }
}

