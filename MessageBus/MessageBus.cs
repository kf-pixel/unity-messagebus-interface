using System.Collections.Generic;

namespace KFP.Messages
{
	public static class MessageBus<T> where T : IMessage
	{
		public static readonly List<ISubscriber<T>> Receivers = new List<ISubscriber<T>>();

		public static void Subscribe(ISubscriber<T> rec) => Receivers.Add(rec);

		public static void Unsubscribe(ISubscriber<T> rec) => Receivers.Remove(rec);

		public static void Publish(T msg)
		{
            for (int i = Receivers.Count - 1; i >= 0; i--)
            {
				Receivers[i].OnReceive(msg);
            }
        }

		static void Clear()
		{
			Receivers.Clear();
		}
	}
}