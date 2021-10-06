using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class ABTools 
{
    //ÿ��Ҫ��AB����ʱ�򣬶�������ִ��
    //1------����HotDll��ResourcesĿ¼
    //2------�Զ�����AB����
    //3------ѡ����Ӧ��ƽ̨���AB
    //ΪʲôҪ��HotDll�ƶ���ResourceĿ¼��
    //��ΪResourceĿ¼���ᱻ�����AB�������ȸ�DLLҲ�����AB�󷽱��ϴ�������
    //Ȼ���ɿͻ����ȸ���ʱ������
    [MenuItem("AB���/����HotDll��ResourcesĿ¼")]
    static void MoveHotDllToResource()
    {
        string CopyFrom = Path.Combine(DllDir, HotfixDll);
        string CopyTo = Path.Combine(CopyToFolder, HotfixDll + ".bytes");
        File.Copy(CopyFrom, CopyTo, true);
        CopyFrom = Path.Combine(DllDir, HotfixPdb);
        CopyTo = Path.Combine(CopyToFolder, HotfixPdb + ".bytes");
        File.Copy(CopyFrom, CopyTo, true);
        Debug.Log("Dll����  ���");
    }


    [MenuItem("AB���/�Զ�����AB����")]
    public static void AutoSetABName()
    {
        ClearAllABName();//���������Ѿ���������AB��Դ
        string TargetFolder = Application.dataPath + "/Resources/src/";
        SetABNames(TargetFolder);
        Debug.Log("�Զ�����AB���� ���");
    }

    [MenuItem("AB���/Android")]
    public static void BuildAB_Android()
    {
        BuildAssetsBundle(BuildTarget.Android);
        Debug.Log("Android���AB ���");
    }

    [MenuItem("AB���/IOS")]
    public static void BuildAB_IOS()
    {
        BuildAssetsBundle(BuildTarget.iOS);
        Debug.Log("IOS���AB ���");
    }

    [MenuItem("AB���/Windows")]
    public static void BuildAB_Windows()
    {
        BuildAssetsBundle(BuildTarget.StandaloneWindows64);
        Debug.Log("Windows���AB ���");
    }

    [MenuItem("AB���/Web")]
    public static void BuildAB_Web()
    {
        BuildAssetsBundle(BuildTarget.WebGL);
        Debug.Log("Web���AB ���");
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
        string ImporterPath = "Assets" + _AssetPath.Substring(Application.dataPath.Length); //����·��������Assets��ʼ
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
