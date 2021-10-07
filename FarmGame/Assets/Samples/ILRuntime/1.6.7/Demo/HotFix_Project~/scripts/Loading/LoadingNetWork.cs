using System;
namespace HotFix_Project
{
    public class LoadingNetWork : BaseNetwork<LoadingNetWork>
    {
        public LoadingNetWork()
        {
        }

        enum MsgID
        {
            LoginMsg = 1001,

        }

        public override void RegisterMsgEvent()
        {
        }

        public override void UnRegisterMsgEvent()
        {
        }

    }
}
