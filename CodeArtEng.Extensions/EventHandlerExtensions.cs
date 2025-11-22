namespace System
{
    /// <summary>
    /// Provides extension methods for safely invoking event handlers.
    /// </summary>
    /// <remarks>These methods help prevent race conditions when raising events by capturing the event handler
    /// delegate before invocation. This is particularly useful in multithreaded scenarios where the event handler may
    /// be unsubscribed between the null check and invocation.</remarks>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Invokes the specified event handler with the provided sender and event data if the handler is not null.
        /// </summary>
        /// <remarks>This extension method simplifies safe event invocation by checking for null before
        /// calling the handler. It is typically used to raise events in a thread-safe manner.</remarks>
        /// <typeparam name="T">The type of the event data, which must derive from EventArgs.</typeparam>
        /// <param name="eventHandler">The event handler to invoke. If null, the method does nothing.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        public static void HandleEvent<T>(this EventHandler<T> eventHandler, object sender, T e) where T : EventArgs
        {
            EventHandler<T> handler = eventHandler;
            if (handler != null) handler(sender, e);
        }
    }
}
