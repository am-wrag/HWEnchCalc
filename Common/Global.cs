using System;
using System.Threading.Tasks;

namespace HWEnchCalc.Common
{
    public static class Global<T>
    {
        private static Task<T> _awaiter;

        public static Task<T> ShowAwaitDialog()
        {
            return _awaiter;
        }

        public static void SetAwaitDialog(Task<T> awaiter)
        {
            _awaiter = awaiter;
        }
    }
}