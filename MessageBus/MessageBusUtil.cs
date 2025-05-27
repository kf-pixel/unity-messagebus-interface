using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace KFP.Messages
{
	public static class MessageBusUtil
	{
		public static IReadOnlyList<Type> MessageTypes { get; set; }
		public static IReadOnlyList<Type> MessageBusTypes { get; set; }

		/// <summary>
		/// Initializes the MessageBusUtil class at runtime before the loading of any scene.
		/// This guarantees that necessary initialization of bus-related types and events is
		/// done before any game objects, scripts or components have started.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Initialize()
		{
			MessageTypes = PredefinedAssemblyUtil.GetTypes(typeof(IMessage));
			MessageBusTypes = InitializeAllBuses();
		}

		static List<Type> InitializeAllBuses()
		{
			List<Type> eventBusTypes = new List<Type>();

			var typedef = typeof(MessageBus<>);
			foreach (var eventType in MessageTypes)
			{
				var busType = typedef.MakeGenericType(eventType);
				eventBusTypes.Add(busType);
				//Debug.Log($"Initialized EventBus<{eventType.Name}>");
			}

			return eventBusTypes;
		}

		/// <summary>
		/// Removes all listeners from all event buses in the application.
		/// </summary>
		public static void ClearAllBuses()
		{
			for (int i = 0; i < MessageBusTypes.Count; i++)
			{
				var busType = MessageBusTypes[i];
				var clearMethod = busType.GetMethod(
					"Clear",
					BindingFlags.Static | BindingFlags.NonPublic
				);
				clearMethod?.Invoke(null, null);
			}
		}

#if UNITY_EDITOR
		public static UnityEditor.PlayModeStateChange PlayModeState { get; set; }

		/// <summary>
		/// Clears all buses when PlayMode changes
		/// </summary>
		[UnityEditor.InitializeOnLoadMethod]
		public static void InitializeEditor()
		{
			UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
		{
			PlayModeState = state;
			if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
			{
				ClearAllBuses();
			}
		}
#endif
	}
}