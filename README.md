
# Message Bus

* An EventBus pattern using `interfaces` for use in Unity.
* Based on [adammyhre's Event Bus](https://github.com/adammyhre/Unity-Event-Bus) - notably for `PredefinedAssembleyUtil.cs`.

## Features
* Highly accessible event system.
* Can create and subscribe to messages with minimal verbosity.

## Issues
* Can forget to `Subscribe()` or `Unsubscribe()`.
* Visual Studio intellisense doesn't seem to recognize the above extension methods when a class inherits multiple subscriptions.

## To Consider
* A class inheriting from `MonoBehaviour` that handles subscribing.
* Ordering Subscribers when `OnReceive()` is fired.
* May want to group or filter Subscribers.
* An editor window to visualize Subscribers during runtime.
* May want a generic `MessageListener.cs` Unity component.
#

# Example
#### Define Message
1. Define a new message with a struct inheriting `IMessage`.
2. Give it any relevant fields.

```csharp
public struct HealthMSG : IMessage
{
	public string Target;
	public int Health;
}
```

#### Subscribing
1. Inherit `ISubscriber<T>` where `T` : `IMessage`,
2. Use the interface method `OnReceive(IMessage message)`.
3. Manage subscription with `this.Subscribe<T>()` and `this.Unsubscribe<T>()`

```csharp
public class UIController : MonoBehaviour,
	ISubscriber<HealthMSG>
{
	public void OnReceive(HealthMSG message)
	{
		// Do stuff
	}

	void OnEnable()
	{
		this.Subscribe<HealthMSG>();
	}

	void OnDisable()
	{
		this.Unsubscribe<HealthMSG>();
	}
}
```

With multiple subscriptions, the type must be specified when subscribing.

#### Publishing
1. Initalize a new `IMessage` struct with the necessary data. 
2. Publish the message to all subscribers with `message.Publish()`.
```csharp
	void Start()
	{
		var hp = new HealthMSG { Target = "Player"; Health = 20; };
		hp.Publish();
	}
```
