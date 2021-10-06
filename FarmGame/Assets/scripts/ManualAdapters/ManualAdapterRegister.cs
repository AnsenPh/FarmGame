using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAdapterRegister 
{
    public static void RegisterAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        //appdomain.RegisterCrossBindingAdaptor(new BaseWindowAdaptor());
        //asyncµƒ  ≈‰
        appdomain.RegisterCrossBindingAdaptor(new MyCoroutineAdapter());
        appdomain.RegisterCrossBindingAdaptor(new MyMonoBehaviourAdapter());
        appdomain.RegisterCrossBindingAdaptor(new MyExceptionAdapter());
        appdomain.RegisterCrossBindingAdaptor(new MyProtoAdapter());
        appdomain.RegisterCrossBindingAdaptor(new Adapt_IMessage());
    }
}
