using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public static class MyCLRRedirectionRegister
{
	unsafe public static void Register(AppDomain appdomain)
	{
        //注册Get和Add Component()
        Type gameObjectType = typeof(GameObject);
        var addComponentMethod = gameObjectType.GetMethods().ToList()
            .Find(i => i.Name == "AddComponent" && i.GetGenericArguments().Length == 1);
        appdomain.RegisterCLRMethodRedirection(addComponentMethod, AddComponent);

        var getComponentMethod = gameObjectType.GetMethods().ToList()
            .Find(i => i.Name == "GetComponent" && i.GetGenericArguments().Length == 1);
        appdomain.RegisterCLRMethodRedirection(getComponentMethod, GetComponent);

        // NOTE: Activator.CreateInstance不需要注册，因为AppDomain在初始化时已经自动注册了！

        //注册3种Log
        Type debugType = typeof(Debug);
        var logMethod = debugType.GetMethod("Log", new[] { typeof(object) });
        appdomain.RegisterCLRMethodRedirection(logMethod, Log);
        var logWarningMethod = debugType.GetMethod("LogWarning", new[] { typeof(object) });
        appdomain.RegisterCLRMethodRedirection(logWarningMethod, LogWarning);
        var logErrorMethod = debugType.GetMethod("LogError", new[] { typeof(object) });
        appdomain.RegisterCLRMethodRedirection(logErrorMethod, LogError);
		var logExptMethod = debugType.GetMethod("LogException", new[] { typeof(Exception) });
		appdomain.RegisterCLRMethodRedirection(logExptMethod, LogExpt);
	}
    unsafe static StackObject* LogExpt(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
    CLRMethod __method, bool isNewObj)
    {
        AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        Exception expt = (Exception)typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        __intp.Free(ptr_of_this_method);

        var stacktrace = __domain.DebugService.GetStackTrace(__intp);
        Debug.LogError("expt: "+expt + "\n>>>>>>>>>>>>>>>>>\n" + stacktrace);
        return __ret;
    }
    unsafe static StackObject* LogError(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
    CLRMethod __method, bool isNewObj)
    {
        AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        __intp.Free(ptr_of_this_method);

        var stacktrace = __domain.DebugService.GetStackTrace(__intp);
        Debug.LogError("err: "+message + "\n>>>>>>>>>>>>>>>>>\n" + stacktrace);
        return __ret;
    }

    /// <summary>
    /// Debug.LogWarning 实现
    /// </summary>
    /// <param name="__intp"></param>
    /// <param name="__esp"></param>
    /// <param name="__mStack"></param>
    /// <param name="__method"></param>
    /// <param name="isNewObj"></param>
    /// <returns></returns>
    unsafe static StackObject* LogWarning(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
        CLRMethod __method, bool isNewObj)
    {
        AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        __intp.Free(ptr_of_this_method);

        var stacktrace = __domain.DebugService.GetStackTrace(__intp);
        Debug.LogWarning("warn: "+message + "\n>>>>>>>>>>>>>>>>>\n" + stacktrace);
        return __ret;
    }

    /// <summary>
    /// Debug.Log 实现
    /// </summary>
    /// <param name="__intp"></param>
    /// <param name="__esp"></param>
    /// <param name="__mStack"></param>
    /// <param name="__method"></param>
    /// <param name="isNewObj"></param>
    /// <returns></returns>
    unsafe static StackObject* Log(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
        CLRMethod __method, bool isNewObj)
    {
        AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        __intp.Free(ptr_of_this_method);

        var stacktrace = __domain.DebugService.GetStackTrace(__intp);
        Debug.Log("log: " + message + "\n>>>>>>>>>>>>>>>>>\n" + stacktrace);
        return __ret;
    }

    unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //CLR重定向的说明请看相关文档和教程，这里不多做解释
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

        var ptr = __esp - 1;
        //成员方法的第一个参数为this
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
        if (instance == null)
            throw new System.NullReferenceException();
        __intp.Free(ptr);

        var genericArgument = __method.GenericArguments;
        //AddComponent应该有且只有1个泛型参数
        if (genericArgument != null && genericArgument.Length == 1)
        {
            var type = genericArgument[0];
            object res;
            if (type is CLRType)
            {
                //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                res = instance.AddComponent(type.TypeForCLR);
            }
            else
            {
                //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                var ilInstance = new ILTypeInstance(type as ILType, false);//手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                //接下来创建Adapter实例
                var clrInstance = instance.AddComponent<MyMonoBehaviourAdapter.Adaptor>();
                //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                clrInstance.ILInstance = ilInstance;
                clrInstance.AppDomain = __domain;
                //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                ilInstance.CLRInstance = clrInstance;

                res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance

                clrInstance.Awake();//因为Unity调用这个方法时还没准备好所以这里补调一次
            }

            return ILIntepreter.PushObject(ptr, __mStack, res);
        }

        return __esp;
    }

    unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //CLR重定向的说明请看相关文档和教程，这里不多做解释
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

        var ptr = __esp - 1;
        //成员方法的第一个参数为this
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
        if (instance == null)
            throw new System.NullReferenceException();
        __intp.Free(ptr);

        var genericArgument = __method.GenericArguments;
        //AddComponent应该有且只有1个泛型参数
        if (genericArgument != null && genericArgument.Length == 1)
        {
            var type = genericArgument[0];
            object res = null;
            if (type is CLRType)
            {
                //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                res = instance.GetComponent(type.TypeForCLR);
            }
            else
            {
                //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                var clrInstances = instance.GetComponents<MyMonoBehaviourAdapter.Adaptor>();
                for (int i = 0; i < clrInstances.Length; i++)
                {
                    var clrInstance = clrInstances[i];
                    if (clrInstance.ILInstance != null)//ILInstance为null, 表示是无效的MonoBehaviour，要略过
                    {
                        if (clrInstance.ILInstance.Type == type)
                        {
                            res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance
                            break;
                        }
                    }
                }
            }

            return ILIntepreter.PushObject(ptr, __mStack, res);
        }

        return __esp;
    }
}
