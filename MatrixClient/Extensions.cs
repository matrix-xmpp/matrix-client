namespace MatrixClient
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class Extensions
    {
        /// <summary>
        /// Adds an item to a collection when it does not exist, or replaces the item when it already exists.
        /// The given predicate is used to check if the item exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <param name="item">The item to add or replace</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static void AddOrReplace<T>(this ObservableCollection<T> col, T item, Func<T, bool> predicate) 
        {            
            var curItem = col.FirstOrDefault(predicate);
            if (curItem != null)
            {
                int index = col.IndexOf(curItem);
                col[index] = item;
            }
            else
            {
                col.Add(item);
            }
        }

        /// <summary>
        /// Removes an item from the collection when it exist.
        /// The given predicate is used to remove the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Remove<T>(this ObservableCollection<T> col, Func<T, bool> predicate)
        {
            var curItem = col.FirstOrDefault(predicate);
            return col.Remove(curItem);
        }        
    }
}
