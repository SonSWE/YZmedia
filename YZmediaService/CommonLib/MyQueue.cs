using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class MyQueue
    {
        string Name;
        AutoResetEvent autoReset;
        Queue queue;
        object objLock;

        public int Count()
        {
            return queue.Count;
        }

        public MyQueue(string name)
        {
            try
            {
                Name = name;
                autoReset = new AutoResetEvent(false);
                queue = new Queue();
                objLock = new object();
            }
            catch
            {

            }
        }

        public MyQueue()
        {
            try
            {
                Name = "";
                autoReset = new AutoResetEvent(false);
                queue = new Queue();
                objLock = new object();
            }
            catch
            {

            }
        }

        public void Enqueue(object obj)
        {
            lock (objLock)
            {
                queue.Enqueue(obj);
                autoReset.Set();
            }
        }


        public object Dequeue()
        {
            if (queue.Count > 0)
                lock (objLock)
                {
                    return queue.Dequeue();
                }
            else
            {
                autoReset.WaitOne();
                lock (objLock)
                {
                    if (queue.Count > 0)
                        return queue.Dequeue();
                    else return null;
                }
            }
        }

        public void Clearqueue()
        {
            lock (objLock)
            {
                queue.Clear();
            }
        }

        public void ReleaseMyQueue()
        {
            autoReset.Set();
            autoReset.Set();
        }
    }
}
