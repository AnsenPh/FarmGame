
using ILRuntime.Runtime.Enviorment;

public static class AdapterRegister{
    public static void Register(AppDomain appDomain){
		appDomain.RegisterCrossBindingAdaptor(new System.Runtime.CompilerServices.IAsyncStateMachineAdapter());

    }
}
