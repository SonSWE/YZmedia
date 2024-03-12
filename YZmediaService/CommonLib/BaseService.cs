using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public abstract class BaseService<T> where T : BaseService<T>, new()
    {
        private static T _instance = null;

        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.ProcessAfterInitialize();
            }

            return _instance;
        }

        protected virtual void ProcessAfterInitialize()
        {

        }
    }
}
