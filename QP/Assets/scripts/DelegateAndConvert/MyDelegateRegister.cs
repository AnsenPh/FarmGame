using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using UnityEngine.ResourceManagement.AsyncOperations;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

/// <summary>
/// 同时注册方法和函数代理
/// </summary>
public static class MyDelegateRegister
{
	public static void Register(AppDomain appdomain)
	{
		//appdomain.DelegateManager.RegisterMethodDelegate<AsyncOperationHandle<GameObject>>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Single>();
		//#region for kbengine hotfix BeginInvoke(state, this._asyncConnectCB, state)
		//appdomain.DelegateManager.RegisterMethodDelegate<System.IAsyncResult>();
		//appdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
		//#endregion
		
		appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
		appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();
	}
}
