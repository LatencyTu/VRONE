using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Templete
{
    public class TestDebug:SingleBase<TestDebug>
    {
        public bool EnableDebug = false;
        public void Log(object msg)
        {
            if (EnableDebug)
            {
                Debug.Log(msg);
            }
        }
        public void Log(params object[] msgList)
        {
            if (EnableDebug)
            {
                StringBuilder str = new StringBuilder("");
                foreach (object msg in msgList)
                {
                    str.Append(msg + " ");
                }
                Debug.Log(str);
            }
        }
    }
}

