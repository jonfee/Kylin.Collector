using ProductCollector.BackState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductCollector.Core
{
    public class Writer
    {
        public static Form OutputForm;

        public static WriterDelegate write;

        public delegate void WriterDelegate(CallBackState state);
                
        /// <summary>
        /// 写信息
        /// </summary>
        /// <param name="state"></param>
        public static void writeInvoke(CallBackState state)
        {
            if (null != OutputForm && null != write)
            {
                OutputForm.Invoke((EventHandler)delegate
                {
                    write(state);
                });
            }
        }
    }
}
