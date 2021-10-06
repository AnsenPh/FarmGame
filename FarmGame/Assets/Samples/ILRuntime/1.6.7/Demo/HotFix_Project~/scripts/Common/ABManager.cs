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

        private static AssetBundle TryToLoad(string _Path)
        {
            string ABName = ConvertToABName(_Path);
            if (m_AssetsBundleCache.ContainsKey(ABName))
            {
                AssetBundle CurrentValue = m_AssetsBundleCache[ABName];
                if (!CurrentValue)
                {
                    CurrentValue = AssetBundle.LoadFromFile(ABName);
                    m_AssetsBundleCache[ABName] = CurrentValue;
                    return CurrentValue;
                }
                else
                {
                    return CurrentValue;
                }
            }
            else
            {
                AssetBundle NewAB = AssetBundle.LoadFromFile(ABName);
                m_AssetsBundleCache.Add(ABName, NewAB);
                return NewAB;
            }


        }

        public static GameObject LoadAssetFromAB_GameObject(string _Path, string _FileName)
        {
            if(DebugMode)
            {
                string FullPath = _Path + "/" + _FileName;
                object obj = Resources.Load(FullPath, typeof(GameObject));
                GameObject Temp = (GameObject)obj;
                return Temp;
            }
            else
            {
                AssetBundle TempAB = TryToLoad(_Path);
                GameObject Src = TempAB.LoadAsset<GameObject>(_FileName.ToLower());
                return Src;
            }

        }

        public static TextAsset LoadAssetFromAB_TextAsset(string _Path, string _FileName)
        {
            if (DebugMode)
            {
                string FullPath = _Path + "/" + _FileName;
                object obj = Resources.Load(FullPath, typeof(TextAsset));
                TextAsset Temp = (TextAsset)obj;
                return Temp;
            }
            else
            {
                AssetBundle TempAB = TryToLoad(_Path);
                TextAsset Src = TempAB.LoadAsset<TextAsset>(_FileName.ToLower());
                return Src;
            }
        }

        public static Sprite LoadAssetFromAB_Sprite(string _Path, string _FileName)
        {
            if (DebugMode)
            {
                string FullPath = _Path + "/" + _FileName;
                object obj = Resources.Load(FullPath, typeof(Sprite));
                Sprite Temp = (Sprite)obj;
                return Temp;
            }
            else
            {
                AssetBundle TempAB = TryToLoad(_Path);
                Sprite Src = TempAB.LoadAsset<Sprite>(_FileName.ToLower());
                return Src;
            }
        }
    }
}

