using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

//will auto register in unity
#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static private void RegisterBindingAction()
        {
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.RegisterBindingAction(Initialize);
        }


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UnityEngine_Application_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Char_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_AssetBundle_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_AssetBundle_Binding.Register(app);
            System_Type_Binding.Register(app);
            UnityEngine_Resources_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            DG_Tweening_ShortcutExtensions_Binding.Register(app);
            DG_Tweening_DOTween_Binding.Register(app);
            DG_Tweening_TweenSettingsExtensions_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Activator_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            BaseButton_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            System_Single_Binding.Register(app);
            UnityEngine_WaitForSeconds_Binding.Register(app);
            System_NotSupportedException_Binding.Register(app);
            System_Action_Binding.Register(app);
            NativeSocket_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Action_1_ILTypeInstance_Binding.Register(app);
            System_Int32_Binding.Register(app);
            System_Action_1_ILTypeInstance_Binding.Register(app);
            System_IO_MemoryStream_Binding.Register(app);
            ICSharpCode_SharpZipLib_GZip_GZipInputStream_Binding.Register(app);
            System_Byte_Binding.Register(app);
            System_IO_Stream_Binding.Register(app);
            System_BitConverter_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            Google_Protobuf_ByteString_Binding.Register(app);
            System_Buffer_Binding.Register(app);
            System_Object_Binding.Register(app);
            Google_Protobuf_MessageExtensions_Binding.Register(app);
            UnityEngine_Networking_UnityWebRequest_Binding.Register(app);
            System_Security_Cryptography_MD5_Binding.Register(app);
            System_Security_Cryptography_HashAlgorithm_Binding.Register(app);
            System_Convert_Binding.Register(app);
            System_Reflection_FieldInfo_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_String_Binding.Register(app);
            System_Action_1_String_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_String_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Object_Action_1_String_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Int32_Binding.Register(app);
            System_Action_1_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Int32_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Object_Action_1_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Boolean_Binding.Register(app);
            System_Action_1_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Boolean_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Object_Action_1_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Single_Binding.Register(app);
            System_Action_1_Single_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Single_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Object_Action_1_Single_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Object_Binding.Register(app);
            System_Action_1_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_Action_1_Object_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Object_Action_1_Object_Binding.Register(app);
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
