
# Message Bus

Provides a centralized messaging system (EventBus) pattern using `interfaces` to handle events in Unity.

* <b>Based on [adammyhre's Event Bus](https://github.com/adammyhre/Unity-Event-Bus), notably for `PredefinedAssembleyUtil.cs`.

### Quick Start
1. Define a new message by inheriting `IMessage`.
2. Subscribe to any message by inheriting `ISubscriber<T>` where `T` : `IMessage`,
3. Use the inherited method `OnReceive(IMessage message)`.
4. Manage subscription with `this.Subscribe<T>()` and `this.Unsubscribe<T>()`
   1. Usually through`OnEnable` and `OnDisable`.
5. Send messages to all subscribers with the `IMessage` method `message.Publish()`.

### Features
* Defined and highly accessible event system.
* Can create new Messages with minimal verbosity.

### Issues
* Can forget to `Subscribe()` or `Unsubscribe()` which are necessary but not enforced.
* Visual Studio intellisense doesn't seem to recognize the above extension methods when a class inherits multiple subscriptions.

### To Consider
* Currently no ordering of when `OnReceive()` is fired.
* Group or filter Subscribers and be able to send Messages based on that.
* An editor window to view all active `ISubscribers` during runtime.
* May want a generic `MessageListener.cs` Unity component, though this muddies usage.
#
# Example
## Defining a new Message
Inherit from interface `IMessage`.
```csharp
public struct HealthMSG : IMessage
{
	public int OldHealth;
	public int NewHealth;
}
```
Message can be a `struct` or a `class`.

## Subscribing to Messages
Inherit from `ISubscriber<T>` where `T` : `IMessage`.
```csharp
public class PlayerController : MonoBehaviour,
	ISubscriber<HealthMSG>
```
Implement interface method `OnReceive(T message)`

```csharp
public class PlayerController : MonoBehaviour,
	ISubscriber<HealthMSG>
{
	public void OnReceive(HealthMSG message)
	{
		// Do stuff
	}
}
```
Manage subscription with extension method `this.Subscribe<T>()` and `this.Unsubscribe<T>()`.

With a single `ISubscriber`, you can leave out the `<T>`.
```csharp
public class PlayerController : MonoBehaviour,
	ISubscriber<HealthMSG>
{
	public void OnReceive(HealthMSG message)
	{
		// Do stuff
	}

	void OnEnable()
	{
		this.Subscribe();
	}

	void OnDisable()
	{
		this.Unsubscribe();
	}
}
```
Otherwise with multiple subscriptions, the type must be included:
```csharp
public class PlayerController : MonoBehaviour,
	ISubscriber<HealthMSG>,
	ISubscriber<BlockMSG>
{
	public void OnReceive(HealthMSG message)
	{
		// Do health stuff
	}

	public void OnReceive(BlockMSG message)
	{
		// Do block stuff
	}

	void OnEnable()
	{
		this.Subscribe<HealthMSG>();
		this.Subscribe<BlockMSG>();
	}

	void OnDisable()
	{
		this.Unsubscribe<HealthMSG>();
		this.Unsubscribe<BlockMSG>();
	}
}
```
   
# Publishing Messages

Use extension method for `IMessage`.
```csharp
	void Start()
	{
		var hp = new HealthMSG { OldHP = 100; NewHP = 20; };
		hp.Publish();

		// or in one method line if preferred
		new HealthMSG { OldHP = 100; NewHP = 20; }.Publish();
	}
```
