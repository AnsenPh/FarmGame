using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix_Project
{
    public abstract class BaseNetwork
    {
        public abstract void RegisterMsgEvent();
        public abstract void UnRegisterMsgEvent();
    }
}
