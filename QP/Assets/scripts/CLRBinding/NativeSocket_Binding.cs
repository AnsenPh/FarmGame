using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class NativeSocket_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NativeSocket);
            args = new Type[]{typeof(System.String), typeof(System.Int32)};
            method = type.GetMethod("ConnectSocket", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ConnectSocket_0);
            args = new Type[]{typeof(System.Byte[])};
            method = type.GetMethod("TryToSendMsg", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TryToSendMsg_1);
            args = new Type[]{typeof(System.Action<System.Byte[]>)};
            method = type.GetMethod("SetRevieveCallback", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetRevieveCallback_2);

            field = type.GetField("OnConnect", flag);
            app.RegisterCLRFieldGetter(field, get_OnConnect_0);
            app.RegisterCLRFieldSetter(field, set_OnConnect_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnConnect_0, AssignFromStack_OnConnect_0);
            field = type.GetField("OnConnectFailed", flag);
            app.RegisterCLRFieldGetter(field, get_OnConnectFailed_1);
            app.RegisterCLRFieldSetter(field, set_OnConnectFailed_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnConnectFailed_1, AssignFromStack_OnConnectFailed_1);
            field = type.GetField("OnClosed", flag);
            app.RegisterCLRFieldGetter(field, get_OnClosed_2);
            app.RegisterCLRFieldSetter(field, set_OnClosed_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnClosed_2, AssignFromStack_OnClosed_2);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* ConnectSocket_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_Port = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @_Ip = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::NativeSocket instance_of_this_method = (global::NativeSocket)typeof(global::NativeSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ConnectSocket(@_Ip, @_Port);

            return __ret;
        }

        static StackObject* TryToSendMsg_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Byte[] @_MsgBuffer = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::NativeSocket instance_of_this_method = (global::NativeSocket)typeof(global::NativeSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.TryToSendMsg(@_MsgBuffer);

            return __ret;
        }

        static StackObject* SetRevieveCallback_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Byte[]> @_Callback = (System.Action<System.Byte[]>)typeof(System.Action<System.Byte[]>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::NativeSocket instance_of_this_method = (global::NativeSocket)typeof(global::NativeSocket).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetRevieveCallback(@_Callback);

            return __ret;
        }


        static object get_OnConnect_0(ref object o)
        {
            return ((global::NativeSocket)o).OnConnect;
        }

        static StackObject* CopyToStack_OnConnect_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::NativeSocket)o).OnConnect;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnConnect_0(ref object o, object v)
        {
            ((global::NativeSocket)o).OnConnect = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnConnect_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnConnect = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::NativeSocket)o).OnConnect = @OnConnect;
            return ptr_of_this_method;
        }

        static object get_OnConnectFailed_1(ref object o)
        {
            return ((global::NativeSocket)o).OnConnectFailed;
        }

        static StackObject* CopyToStack_OnConnectFailed_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::NativeSocket)o).OnConnectFailed;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnConnectFailed_1(ref object o, object v)
        {
            ((global::NativeSocket)o).OnConnectFailed = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnConnectFailed_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnConnectFailed = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::NativeSocket)o).OnConnectFailed = @OnConnectFailed;
            return ptr_of_this_method;
        }

        static object get_OnClosed_2(ref object o)
        {
            return ((global::NativeSocket)o).OnClosed;
        }

        static StackObject* CopyToStack_OnClosed_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::NativeSocket)o).OnClosed;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnClosed_2(ref object o, object v)
        {
            ((global::NativeSocket)o).OnClosed = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnClosed_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnClosed = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::NativeSocket)o).OnClosed = @OnClosed;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new global::NativeSocket();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
