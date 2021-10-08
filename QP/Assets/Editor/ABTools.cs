using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class ABTools 
{
    //每次要打AB包的时候，都必须先执行
    //1------拷贝HotDll到Resources目录
    //2------自动生产AB命名
    //3------选择相应的平台打包AB
    //为什么要把HotDll移动到Resource目录？
    //因为Resource目录最后会被打包成AB包，把热更DLL也打包成AB后方便上传服务器
    //然后由客户端热更的时候下载
    [MenuItem("AB打包/拷贝HotDll到Resources目录")]
    static void MoveHotDllToResource()
    {
        string CopyFrom = Path.Combine(DllDir, HotfixDll);
        string CopyTo = Path.Combine(CopyToFolder, HotfixDll + ".bytes");
        File.Copy(CopyFrom, CopyTo, true);
        CopyFrom = Path.Combine(DllDir, HotfixPdb);
        CopyTo = Path.Combine(CopyToFolder, HotfixPdb + ".bytes");
        File.Copy(CopyFrom, CopyTo, true);
        Debug.Log("Dll拷贝  完成");
    }


    [MenuItem("AB打包/自动生产AB命名")]
    public static void AutoSetABName()
    {
        ClearAllABName();//清理所有已经被命名的AB资源
        string TargetFolder = Application.dataPath + "/Resources/src/";
        SetABNames(TargetFolder);
        Debug.Log("自动生产AB命名 完成");
    }

    [MenuItem("AB打包/Android")]
    public static void BuildAB_Android()
    {
        BuildAssetsBundle(BuildTarget.Android);
        Debug.Log("Android打包AB 完成");
    }

    [MenuItem("AB打包/IOS")]
    public static void BuildAB_IOS()
    {
        BuildAssetsBundle(BuildTarget.iOS);
        Debug.Log("IOS打包AB 完成");
    }

    [MenuItem("AB打包/Windows")]
    public static void BuildAB_Windows()
    {
        BuildAssetsBundle(BuildTarget.StandaloneWindows64);
        Debug.Log("Windows打包AB 完成");
    }

    [MenuItem("AB打包/Web")]
    public static void BuildAB_Web()
    {
        BuildAssetsBundle(BuildTarget.WebGL);
        Debug.Log("Web打包AB 完成");
    }


    private static void BuildAssetsBundle(BuildTarget _TargetPlatfrom)
    {
        string TargetPath = Application.streamingAssetsPath;
        string OutPath = TargetPath + "/" + _TargetPlatfrom.ToString() + "/";

        if (Directory.Exists(OutPath))
        {
            Directory.Delete(OutPath, true);
        }
        Directory.CreateDirectory(OutPath);

        BuildPipeline.BuildAssetBundles(OutPath, BuildAssetBundleOptions.None, _TargetPlatfrom);
    }

    public static void SetABNames(string _assetsPath)
    {
        DirectoryInfo Dir = new DirectoryInfo(_assetsPath);
        FileSystemInfo[] files = Dir.GetFileSystemInfos();

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)
            {
                SetABNames(files[i].FullName);
            }
            else if (!files[i].Name.EndsWith(".meta"))
            {
                SetABName(files[i].FullName);
            }
        }
    }

    static void SetABName(string _AssetPath)
    {
        string ImporterPath = "Assets" + _AssetPath.Substring(Application.dataPath.Length); //这里路径必须以Assets开始
        AssetImporter Importer = AssetImporter.GetAtPath(ImporterPath);
        string[] TempNames = ImporterPath.Split('\\');
        string AssetName = "";
        for (int i = 2; i < TempNames.Length - 1; i++)
        {
            AssetName += TempNames[i];
        }
        Importer.assetBundleName = AssetName;
    }


    public static void ClearAllABName()
    {
        string[] ABNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < ABNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(ABNames[i], true);
        }
    }

    const string DllDir = "../dll";
    const string CopyToFolder = "Assets/Resources/src/hotfix";
    const string HotfixDll = "HotFix_Project.dll";
    const string HotfixPdb = "HotFix_Project.pdb";
}
