using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class IListExtensions
    {
        public static void MoveUp(this IList sender, int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex >= sender.Count)) throw new IndexOutOfRangeException();
            if (itemIndex == 0) return; //First item, do nothing.

            object item = sender[itemIndex];
            sender.Remove(item);
            sender.Insert(itemIndex - 1, item);
        }

        public static void MoveDown(this IList sender, int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex >= sender.Count)) throw new IndexOutOfRangeException();
            if (itemIndex == sender.Count - 1) return; //Last item, do 

            object item = sender[itemIndex];
            sender.Remove(item);
            sender.Insert(itemIndex + 1, item);
        }
    }
}
