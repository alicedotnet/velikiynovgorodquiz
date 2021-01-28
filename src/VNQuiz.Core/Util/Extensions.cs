using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNQuiz.Core.Util
{
    public static class Extensions
    {
        public static int BinarySearch<T>(this List<T> list, int id, Func<T, int> getId)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            var left = 0;
            var right = list.Count;
            
            while (left <= right)
            {
                var middle = (left + right) / 2;

                var itemId = getId(list[middle]);
                if (itemId == id)
                    return middle;
                else if (itemId > id)
                {
                    right = middle - 1;
                } 
                else
                {
                    left = middle + 1;
                }
            }

            return -1;
        }
    }
}
