using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace HWEnchCalc.Common
{
    public static class Extentions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> res)
        {
            return new ObservableCollection<T>(res);
        }
    }
}