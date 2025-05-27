namespace KFP.Messages
{
	/// <summary>
	/// Create a new IMessage - use a struct.
	/// </summary>
	public interface IMessage
	{

	}

	/// <summary>
	/// Implement this interface and manage the subscription to receive the message.
	/// </summary>
	/// <typeparam name="T">The message to be listened to.</typeparam>
	public interface ISubscriber<T> where T : IMessage 
	{
		/// <summary>
		/// Interface method when a message is published.
		/// </summary>
		/// <param name="message"></param>
		void OnReceive(T message);
	}

	public static class IMessageUtil
	{
		/// <summary>
		/// Static method for subscribing to Messages;
		/// Specify type if implementing multiple subscriptions.
		/// </summary>
		public static void Subscribe<T>(this ISubscriber<T> sub, bool subscribe = true) where T : IMessage
		{
			if (subscribe)
				MessageBus<T>.Subscribe(sub);
			else
				MessageBus<T>.Unsubscribe(sub);
		}

		/// <summary>
		/// Static method for unsubscribing to Messages;
		/// Specify type if implementing multiple subscriptions.
		/// </summary>
		public static void Unsubscribe<T>(this ISubscriber<T> sub) where T : IMessage
		{
			MessageBus<T>.Unsubscribe(sub);
		}

		/// <summary>
		/// Static method for sending a Message.
		/// </summary>
		public static void Publish<T>(this T message) where T : IMessage
		{
			MessageBus<T>.Publish(message);
		}
	}
}