using System.Collections;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
public class ILRunTimeStart : MonoBehaviour
{
    AppDomain m_Appdomain;

    System.IO.MemoryStream m_Dll;
    System.IO.MemoryStream m_Pdb;

    public BaseButton m_StartBtn;
    private void Start()
    {
        Instance = this;
        m_StartBtn.SetClickCallback(OnBtnStart);
    }
    void StartLoadDll()
    {
        StartCoroutine(LoadHotFixDll());
    }

    IEnumerator LoadHotFixDll()
    {
        string TargetPath = Application.streamingAssetsPath;
        string DllName = "/HotFix_Project.dll";
        string PdbName = "/HotFix_Project.pdb";
#if UNITY_EDITOR
        TargetPath = Application.streamingAssetsPath + "/../../../dll";
#endif


        //首先实例化ILRuntime的AppDomain，AppDomain是一个应用程序域，每个AppDomain都是一个独立的沙盒
        m_Appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        UnityWebRequest DllRequest = new UnityWebRequest(TargetPath + DllName);
        DownloadHandlerBuffer DllDownloadBuffer = new DownloadHandlerBuffer();
        DllRequest.downloadHandler = DllDownloadBuffer;
        yield return DllRequest.SendWebRequest();
        if (DllRequest.result == UnityWebRequest.Result.ProtocolError || DllRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(DllRequest.error);
        }
        else
        {
            byte[] dll = DllRequest.downloadHandler.data;
            m_Dll = new MemoryStream(dll);
        }
        //DllRequest.Dispose();


#if UNITY_EDITOR
        UnityWebRequest PdbRequest = new UnityWebRequest(TargetPath + PdbName);
        DownloadHandlerBuffer PdbDownloadBuffer = new DownloadHandlerBuffer();
        PdbRequest.downloadHandler = PdbDownloadBuffer;
        yield return PdbRequest.SendWebRequest();
        if (PdbRequest.result == UnityWebRequest.Result.ProtocolError || PdbRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(PdbRequest.error);
        }
        else
        {
            byte[] pdb = PdbRequest.downloadHandler.data;
            m_Pdb = new MemoryStream(pdb);
        }
        //PdbRequest.Dispose();
#endif



        try
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            m_Appdomain.LoadAssembly(m_Dll, m_Pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#else
            m_Appdomain.LoadAssembly(m_Dll, null, null);
#endif
        }
        catch
        {
            Debug.LogError("加载热更DLL失败");
        }

        InitializeILRuntime();
        OnHotFixDllLoaded();
    }

    void InitializeILRuntime()
    {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
        m_Appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
        AdapterRegister.Register(m_Appdomain); //自动生成的适配器注册
        ManualAdapterRegister.RegisterAdaptor(m_Appdomain);//手动编写的适配器注册
        ILDelegate.RegisterDelegate(m_Appdomain);//委托适配器

        // 委托注册
        MyDelegateRegister.Register(m_Appdomain);

        // CLR重定向注册
        MyCLRRedirectionRegister.Register(m_Appdomain);

        // 委托转换器注册
        MyDelegateConverter.Register(m_Appdomain);

        // LitJson对特殊数据类型的支持（LVector2/LVector3/LQuaternion）
        MyILitJsonRegister.Register(m_Appdomain);

        //这个是只有生成了绑定代码之后 才能够调用的
        //ILRuntime.Runtime.Generated.CLRBindings.Initialize(m_Appdomain);
        m_Appdomain.DebugService.StartDebugService(56000);
    }

    //当热更DLL加载完毕后
    void OnHotFixDllLoaded()
    {
        m_Appdomain.Invoke("HotFix_Project.MainStart", "Start", null, null);
    }

    private void OnDestroy()
    {
        if (m_Dll != null)
            m_Dll.Close();
        if (m_Pdb != null)
            m_Pdb.Close();
        m_Dll = null;
        m_Pdb = null;
    }


    static ILRunTimeStart Instance;
    public static ILRunTimeStart GetInstance()
    {
        return Instance; 
    }

    public void DoCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void OnBtnStart(int _CustomerData)
    {
        StartLoadDll();
        m_StartBtn.gameObject.SetActive(false);
    }
}