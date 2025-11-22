namespace System.Collections.Generic
{
    /// <summary>
    /// Provides extension methods for moving items within an <see cref="IList"/> instance.
    /// </summary>
    /// <remarks>These methods enable repositioning of items in a list by moving them up or down one position.
    /// They operate directly on the provided <see cref="IList"/> and modify its contents in place. These extensions are
    /// useful for implementing item reordering functionality in user interfaces or data structures that rely on <see
    /// cref="IList"/>.</remarks>
    public static class IListExtensions
    {
        /// <summary>
        /// Moves the item at the specified index one position up in the list.
        /// </summary>
        /// <remarks>If the specified item is already at the first position, the method performs no
        /// action. The relative order of other items is preserved.</remarks>
        /// <param name="sender">The list whose item will be moved. Cannot be null.</param>
        /// <param name="itemIndex">The zero-based index of the item to move. Must be within the bounds of the list.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="itemIndex"/> is less than 0 or greater than or equal to the number of items in
        /// <paramref name="sender"/>.</exception>
        public static void MoveUp(this IList sender, int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex >= sender.Count)) throw new IndexOutOfRangeException();
            if (itemIndex == 0) return; //First item, do nothing.

            object item = sender[itemIndex];
            sender.Remove(item);
            sender.Insert(itemIndex - 1, item);
        }

        /// <summary>
        /// Moves the item at the specified index one position down in the list.
        /// </summary>
        /// <remarks>If the specified item is already the last item in the list, the method has no effect.
        /// The relative order of other items is preserved.</remarks>
        /// <param name="sender">The list whose item will be moved. Cannot be null.</param>
        /// <param name="itemIndex">The zero-based index of the item to move. Must be within the bounds of the list.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="itemIndex"/> is less than 0 or greater than or equal to the number of items in
        /// <paramref name="sender"/>.</exception>
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
