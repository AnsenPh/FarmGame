#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
[System.Reflection.Obfuscation(Exclude = true)]
public class ILRuntimeCLRBinding
{
   [MenuItem("ILRuntime/通过自动分析热更DLL生成CLR绑定")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        using (System.IO.FileStream fs = new System.IO.FileStream("../dll/HotFix_Project.dll", System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            domain.LoadAssembly(fs);

            //Crossbind Adapter is needed to generate the correct binding code
            string WorkDir = "Assets/scripts/CLRBinding";

            if (Directory.Exists(WorkDir))
            {
                Directory.Delete(WorkDir, true);
            }
            Directory.CreateDirectory(WorkDir);
            InitILRuntime(domain);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, WorkDir);
        }

        AssetDatabase.Refresh();
    }

    static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
    {
        //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
        AdapterRegister.Register(domain); //自动生成适配器注册
        ManualAdapterRegister.RegisterAdaptor(domain);//手动编写的适配器注册

    }
}
#endif
