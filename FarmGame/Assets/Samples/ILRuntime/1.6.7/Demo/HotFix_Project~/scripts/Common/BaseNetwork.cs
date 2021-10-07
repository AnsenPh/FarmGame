using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix_Project
{
    public abstract class BaseNetwork<T> where T : new()
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        public abstract void RegisterMsgEvent();
        public abstract void UnRegisterMsgEvent();
    }
}
