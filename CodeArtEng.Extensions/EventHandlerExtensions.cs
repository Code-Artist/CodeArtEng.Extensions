namespace System
{
    public static class EventHandlerExtensions
    {
        public static void HandleEvent<T>(this EventHandler<T> eventHandler, object sender, T e) where T : EventArgs
        {
            EventHandler<T> handler = eventHandler;
            if (handler != null) handler(sender, e);
        }
    }
}
