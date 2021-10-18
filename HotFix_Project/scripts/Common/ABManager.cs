using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    public class ABManager
    {
        static bool DebugMode = true;
        private static Dictionary<string, AssetBundle> m_AssetsBundleCache = new Dictionary<string, AssetBundle>();
        private static string GetABFolder()
        {
            string TargetPath = Application.streamingAssetsPath;
            string OutPath = "";
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                OutPath = TargetPath + "/" + "StandaloneWindows64\\";
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                OutPath = TargetPath + "/" + "Android\\";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                OutPath = TargetPath + "/" + "iOS\\";
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                OutPath = TargetPath + "/" + "StandaloneWindows64\\";
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                OutPath = TargetPath + "/" + "WebGL\\";
            }

            return OutPath;
        }

        private static string ConvertToABName(string _Path)
        {
            string[] SplitPath = _Path.Split('/');

            string ABName = "";
            for (int i = 0; i < SplitPath.Length; i++)
            {
                ABName += SplitPath[i];
            }
            ABName = ABName.ToLower();
            string FinalPath = GetABFolder() + ABName;
            return FinalPath;
        }



        public static Object LoadAssetFromAB(string _Path, string _FileName)
        {
            if(DebugMode)
            {
                string FullPath = _Path + "/" + _FileName;
                Object obj = Resources.Load(FullPath);
                return obj;
            }
            else
            {
                AssetBundle TempAB = TryToLoad(_Path );
                Object Src = TempAB.LoadAsset<Object>(_FileName.ToLower());
                return Src;
            }

        }

        public static void LoadAssetFromABAsync(string _Path, string _FileName , System.Action<Object> _FinishCallback, System.Action<float> _UpdateCallBack = null)
        {
            if (DebugMode)
            {
                ILRunTimeStart.GetInstance().DoCoroutine(AsyncResourceLoad(_Path, _FileName , _FinishCallback , _UpdateCallBack));
            }
            else
            {
                ILRunTimeStart.GetInstance().DoCoroutine(AsyncABLoad(_Path, _FileName, _FinishCallback , _UpdateCallBack));
            }
            
        }

        public static IEnumerator AsyncResourceLoad(string _Path, string _FileName, System.Action<Object> _FinishCallback, System.Action<float> _UpdateCallBack = null)
        {
            string FullPath = _Path + "/" + _FileName;
            ResourceRequest Result = Resources.LoadAsync<Object>(FullPath);
            while (!Result.isDone)
            {
                if (_UpdateCallBack != null)
                {
                    _UpdateCallBack(Result.progress);
                }
                yield return null;

            }
            yield return Result;
            if(_FinishCallback != null)
            {
                _FinishCallback(Result.asset );
            }
        }

        public static IEnumerator AsyncABLoad(string _Path, string _FileName, System.Action<Object> _FinishCallback , System.Action<float> _UpdateCallBack = null)
        {
            string ABName = ConvertToABName(_Path);
            if (m_AssetsBundleCache.ContainsKey(ABName))
            {
                AssetBundle CurrentBundle = m_AssetsBundleCache[ABName];
                AssetBundleRequest AssetsRequest = CurrentBundle.LoadAssetAsync(_FileName);

                while (!AssetsRequest.isDone)
                {
                    if (_UpdateCallBack != null)
                    {
                        _UpdateCallBack(AssetsRequest.progress);
                    }
                    yield return null;

                }
                yield return AssetsRequest;

                if (_FinishCallback != null)
                {
                    _FinishCallback(AssetsRequest.asset);
                }
            }
            else
            {
                AssetBundleCreateRequest Bundlerequest = AssetBundle.LoadFromFileAsync(ABName);
                while (!Bundlerequest.isDone)
                {
                    if (_UpdateCallBack != null)
                    {
                        _UpdateCallBack(Bundlerequest.progress);
                    }
                    yield return null;

                }
                yield return Bundlerequest;

                AssetBundle CurrentBundle = Bundlerequest.assetBundle;
                if (m_AssetsBundleCache.ContainsKey(ABName) == false)
                {
                    m_AssetsBundleCache.Add(ABName, CurrentBundle);
                }

                AssetBundleRequest AssetsRequest = CurrentBundle.LoadAssetAsync(_FileName);

                while (!AssetsRequest.isDone)
                {
                    if (_UpdateCallBack != null)
                    {
                        _UpdateCallBack(AssetsRequest.progress);
                    }
                    yield return null;

                }
                yield return AssetsRequest;

                if (_FinishCallback != null)
                {
                    _FinishCallback(AssetsRequest.asset);
                }
            }
        }

        private static AssetBundle TryToLoad(string _Path)
        {
            string ABName = ConvertToABName(_Path);
            if (m_AssetsBundleCache.ContainsKey(ABName))
            {
                AssetBundle CurrentValue = m_AssetsBundleCache[ABName];
                return CurrentValue;
            }
            else
            {
                AssetBundle NewAB = AssetBundle.LoadFromFile(ABName);
                m_AssetsBundleCache.Add(ABName, NewAB);
                return NewAB;
            }
        }
    }
}

