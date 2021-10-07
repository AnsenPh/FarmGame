using ILitJson;
//using Lockstep.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public static class MyILitJsonRegister
{
	#region Json数据序列化的辅助
	public static void WriteProperty(this JsonWriter w, string name, long value)
	{
		w.WritePropertyName(name);
		w.Write(value);
	}

	public static void WriteProperty(this JsonWriter w, string name, string value)
	{
		w.WritePropertyName(name);
		w.Write(value);
	}

	public static void WriteProperty(this JsonWriter w, string name, bool value)
	{
		w.WritePropertyName(name);
		w.Write(value);
	}

	public static void WriteProperty(this JsonWriter w, string name, double value)
	{
		w.WritePropertyName(name);
		w.Write(value);
	}
	#endregion

	public static void Register(AppDomain appdomain)
	{
		JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);

		// 注册Type类型的Exporter
		JsonMapper.RegisterExporter<Type>((v, w) =>
		{
			w.Write(v.FullName);
		});

		JsonMapper.RegisterImporter<string, Type>((s) =>
		{
			return Type.GetType(s);
		});

		//JsonMapper.RegisterExporter<LFloat>((o, w) =>
		//{
		//	//Debug.Log("export LFloat");
		//	w.WriteObjectStart(); // {
		//	w.WriteProperty("_val", o._val); // "_val" = 12345
		//	w.WriteObjectEnd(); // }
		//});

		//JsonMapper.RegisterExporter<LVector3>((o, w) =>
		//{
		//	//Debug.Log("export LVector3");
		//	w.WriteObjectStart();
		//	w.WriteProperty("x", o.x._val);
		//	w.WriteProperty("y", o.y._val);
		//	w.WriteProperty("z", o.z._val);
		//	w.WriteObjectEnd();
		//});

		//JsonMapper.RegisterExporter<LQuaternion>((o, w) =>
		//{
		//	//Debug.Log("export LQuaternion");
		//	w.WriteObjectStart();
		//	w.WriteProperty("x", o.x._val);
		//	w.WriteProperty("y", o.y._val);
		//	w.WriteProperty("z", o.z._val);
		//	w.WriteProperty("w", o.w._val);
		//	w.WriteObjectEnd();
		//});

		JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(obj.ToString())); //float->string
		JsonMapper.RegisterImporter<string, float>(input => Convert.ToSingle(input)); //string->float
	}
}
