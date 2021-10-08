using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class LoginDataNotify : BaseDataNotify<LoginDataNotify>
    {
        //用于控制是否显示TestWindow
        public BaseData<bool> Data_TestWindowShow = new BaseData<bool>();

        //玩家点击了登录按钮
        public BaseData<bool> Data_LoginBtnClicked = new BaseData<bool>();

        //往下按照自己的需求添加其他需要Prefab之间进行沟通的变量

    }

}
