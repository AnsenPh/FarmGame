using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Object = UnityEngine.Object;

namespace HotFix_Project
{
    public class UIMgr:Singleton<UIMgr>
    {

        public BaseUIMgr m_CurrentUI;
        public BaseUIMgr m_Current3D;


        public Camera GetMainCamera()
        {
            return m_MainCamera;
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
            ILRunTimeStart.GetInstance().DoCoroutine(ExcutiveChageScene(_ChangeFinish, _RootUIClassName, _3dUIClassName));
        }

        IEnumerator ExcutiveChageScene(Action _ChangeFinish, string _RootUIClassName, string _3dUIClassName = "")
        {
            DeleteCurrentScene();
            yield return new WaitForEndOfFrame();
            bool RootUILoaded = false;
            bool UI3DLoaded = false;
            NewPrefabAsync(_RootUIClassName, m_RootUIObj.transform, (RootScript) =>
            {
                RootUILoaded = true;
                m_CurrentUI = RootScript;
            });

            if (_3dUIClassName != "")
            {
                NewPrefabAsync(_3dUIClassName, m_Root3DObj.transform, (Script3D) =>
                {
                    UI3DLoaded = true;
                    m_Current3D = Script3D;
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

        public BaseUIMgr ShowWindow(string _ClassName, bool _Show)
        {
            if (m_WindowUI.ContainsKey(_ClassName))
            {
                BaseUIMgr CurrentScripts = m_WindowUI[_ClassName];
                CurrentScripts.Show(_Show);
                return CurrentScripts;
            }
            else
            {
                BaseUIMgr TempScripts = NewPrefab(_ClassName, m_WindowUIObj.transform);
                m_WindowUI.Add(_ClassName, TempScripts);
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
            m_Current3D = null;
            m_CurrentUI = null;
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

        private void DeleteCurrentScene()
        {
            if(m_CurrentUI !=null)
            {
                m_CurrentUI.Delete();
                m_CurrentUI = null;
            }

            if (m_Current3D != null)
            {
                m_Current3D.Delete();
                m_Current3D = null;
            }

            DeleteAllWindow();
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
            m_PrefabInfo = new Dictionary<string, PrefabInfo>();
            m_PrefabInfo.Add("LoginMgr", new PrefabInfo("LoginUI", "src/login/ui"));
            m_PrefabInfo.Add("TestWindow", new PrefabInfo("TestWindow", "src/login/ui"));
            m_PrefabInfo.Add("LoadingMgr", new PrefabInfo("LoadingUI", "src/Loading"));
            m_PrefabInfo.Add("LoginList", new PrefabInfo("LoginList", "src/login/ui"));
            m_PrefabInfo.Add("MainLand3DMgr", new PrefabInfo("MainLand", "src/mainLand/3D"));
            m_PrefabInfo.Add("MainLandUIMgr", new PrefabInfo("MainLandUI", "src/mainLand/UI"));
            m_PrefabInfo.Add("RoomUIMgr", new PrefabInfo("RoomUI", "src/roomScene/UI"));
            m_PrefabInfo.Add("RoomHallMgr", new PrefabInfo("RoomHall", "src/roomScene/prefab"));
        }

    }
}

