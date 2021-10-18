using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Object = UnityEngine.Object;

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

        public Camera GetMainCamera()
        {
            return m_MainCamera;
        }

        public enum UIType
        {
            RootUI,
            WindowUI,
            UI3D,
        }

        public BaseUIMgr NewPrefab(string _ClassName , Transform _Parent)
        {
            PrefabInfo PrefabInfo = m_PrefabInfo[_ClassName];
            string PrefabName = PrefabInfo.m_PrefabName;
            string Path = PrefabInfo.m_Path;
            GameObject Prefab = ABManager.LoadAssetFromAB(Path, PrefabName)as GameObject;
            BaseUIMgr TempClass = CreateClass(_ClassName);
            TempClass.SetGameObj(Prefab , _Parent);
            return TempClass;
        }


        public void NewPrefabAsync(string _ClassName, Transform _Parent,System.Action<BaseUIMgr> _FinishCallback, System.Action<float> _UpdateCallBack = null)
        {
            PrefabInfo PrefabInfo = m_PrefabInfo[_ClassName];
            string PrefabName = PrefabInfo.m_PrefabName;
            string Path = PrefabInfo.m_Path;
            ABManager.LoadAssetFromABAsync(Path, PrefabName, (LoadTarget) => 
            {
                BaseUIMgr TempClass = CreateClass(_ClassName);
                TempClass.SetGameObj(LoadTarget as GameObject, _Parent);
                _FinishCallback(TempClass);

            }, _UpdateCallBack);
        }

        public void ChangeScene(Action _ChangeFinish , string _RootUIClassName , string _3dUIClassName = "")
        {
            DeleteAllRootUI();
            DeleteAllRoot3D();
            DeleteAllWindow();
            ILRunTimeStart.GetInstance().DoCoroutine(ExcutiveChageScene(_ChangeFinish, _RootUIClassName, _3dUIClassName));
        }

        IEnumerator ExcutiveChageScene(Action _ChangeFinish, string _RootUIClassName, string _3dUIClassName = "")
        {
            bool RootUILoaded = false;
            bool UI3DLoaded = false;
            NewPrefabAsync(_RootUIClassName, m_RootUIObj.transform, (RootScript) =>
            {
                RootUILoaded = true;
            });

            if (_3dUIClassName != "")
            {
                NewPrefabAsync(_3dUIClassName, m_Root3DObj.transform, (Script3D) =>
                {
                    UI3DLoaded = true;
                });
            }
            else
            {
                UI3DLoaded = true;
            }

            while (RootUILoaded == false || UI3DLoaded == false)
            {
                yield return null;
            }

            if(_ChangeFinish != null)
            {
                _ChangeFinish();
            }
        }

        public BaseUIMgr ShowUI(string _ClassName, bool _Show , UIType _Type)
        {
            Transform TempParent = null;
            Dictionary<string, BaseUIMgr> TempDic = null;
            switch (_Type)
            {
                case UIType.RootUI:
                    {
                        TempParent = m_RootUIObj.transform;
                        TempDic = m_RootUI;
                    }
                    break;
                case UIType.UI3D:
                    {
                        TempParent = m_Root3DObj.transform;
                        TempDic = m_Root3D;
                    }
                    break;
                case UIType.WindowUI:
                    {
                        TempParent = m_WindowUIObj.transform;
                        TempDic = m_WindowUI;
                    }
                    break;
            }

            if (TempDic.ContainsKey(_ClassName))
            {
                BaseUIMgr CurrentScripts = TempDic[_ClassName];
                CurrentScripts.Show(_Show);
                return CurrentScripts;
            }
            else
            {
                BaseUIMgr TempScripts = NewPrefab(_ClassName, TempParent);
                TempDic.Add(_ClassName, TempScripts);
                TempScripts.Show(_Show);
                return TempScripts;
            }
        }

        public void ShowLoadingUI(bool _Show)
        {
            m_LoadingObj.SetActive(_Show);
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
        //3D场景的根节点
        GameObject m_Root3DObj = null;
        //Loading读取界面，用于遮挡画面，等待场景读取完毕
        GameObject m_LoadingObj = null;
        //主相机
        Camera m_MainCamera = null;

        //存放所有UI代码的list。 
        private Dictionary<string , BaseUIMgr> m_WindowUI;
        private Dictionary<string , BaseUIMgr> m_RootUI;
        private Dictionary<string, BaseUIMgr> m_Root3D;

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
        private Dictionary<string, PrefabInfo> m_PrefabInfo; 


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
            m_LoadingObj = m_CanvasObj.transform.Find("Loading").gameObject;
            m_Root3DObj = GameObject.Find("3DRoot");
            m_MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        private void DeleteAllRootUI()
        {
            while (m_RootUI.Count > 0)
            {
                string Key = m_RootUI.ElementAt(0).Key;
                BaseUIMgr Value = m_RootUI.ElementAt(0).Value;
                Value.Delete();
                m_RootUI[Key] = null;
                m_RootUI.Remove(Key);
            }
            m_RootUI.Clear();
        }

        private void DeleteAllRoot3D()
        {
            while (m_Root3D.Count > 0)
            {
                string Key = m_Root3D.ElementAt(0).Key;
                BaseUIMgr Value = m_Root3D.ElementAt(0).Value;
                Value.Delete();
                m_Root3D[Key] = null;
                m_Root3D.Remove(Key);
            }
            m_Root3D.Clear();
        }

        private void DeleteAllWindow(bool HideOnly = false)
        {
            while (m_WindowUI.Count > 0)
            {
                string Key = m_WindowUI.ElementAt(0).Key;
                BaseUIMgr Value = m_WindowUI.ElementAt(0).Value;
                Value.Delete();
                m_WindowUI[Key] = null;
                m_WindowUI.Remove(Key);
            }
            m_WindowUI.Clear();
        }

        BaseUIMgr CreateClass(string _ClassName)
        {
            System.Type ClassType = System.Type.GetType("HotFix_Project." + _ClassName);
            BaseUIMgr ClassObj = Activator.CreateInstance(ClassType) as BaseUIMgr;
            if (ClassObj == null)
            {
                Debug.LogError("这个类无法生成，类名====" + _ClassName);
                return null;
            }
            else
            {
                return ClassObj;
            }
        }

        private void InitList()
        {
            m_WindowUI = new Dictionary<string, BaseUIMgr>();
            m_RootUI = new Dictionary<string, BaseUIMgr>();
            m_Root3D = new Dictionary<string, BaseUIMgr>();
            m_PrefabInfo = new Dictionary<string, PrefabInfo>();
            m_PrefabInfo.Add("LoginMgr", new PrefabInfo("LoginUI", "src/login/ui"));
            m_PrefabInfo.Add("TestWindow", new PrefabInfo("TestWindow", "src/login/ui"));
            m_PrefabInfo.Add("LoadingMgr", new PrefabInfo("LoadingUI", "src/Loading"));
            m_PrefabInfo.Add("LoginList", new PrefabInfo("LoginList", "src/login/ui"));
            m_PrefabInfo.Add("MainLandCtr", new PrefabInfo("MainLand", "src/mainScene/3D"));
            
        }

    }
}

