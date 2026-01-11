// https://github.com/ds5678/KeyboardUtilities/blob/master/KeyboardUtilities/InputSystem.cs
using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace QoL
{
	internal sealed class LegacyInput
	{
		public LegacyInput()
		{
			ReflectionHelpers.LoadModule("UnityEngine.InputLegacyModule");
			TInput = ReflectionHelpers.GetTypeByName("UnityEngine.Input") ?? throw new NullReferenceException(nameof(TInput));

			m_mousePositionProp = TInput.GetProperty("mousePosition") ?? throw new NullReferenceException(nameof(m_mousePositionProp));
			m_mouseScrollDeltaProp = TInput.GetProperty("mouseScrollDelta") ?? throw new NullReferenceException(nameof(m_mouseScrollDeltaProp));
			m_getKeyMethod = TInput.GetMethod("GetKey", new Type[] { typeof(KeyCode) }) ?? throw new NullReferenceException(nameof(m_getKeyMethod));
			m_getKeyDownMethod = TInput.GetMethod("GetKeyDown", new Type[] { typeof(KeyCode) }) ?? throw new NullReferenceException(nameof(m_getKeyDownMethod));
			m_getKeyUpMethod = TInput.GetMethod("GetKeyUp", new Type[] { typeof(KeyCode) }) ?? throw new NullReferenceException(nameof(m_getKeyUpMethod));
			m_getMouseButtonMethod = TInput.GetMethod("GetMouseButton", new Type[] { typeof(int) }) ?? throw new NullReferenceException(nameof(m_getMouseButtonMethod));
			m_getMouseButtonDownMethod = TInput.GetMethod("GetMouseButtonDown", new Type[] { typeof(int) }) ?? throw new NullReferenceException(nameof(m_getMouseButtonDownMethod));
			m_getMouseButtonUpMethod = TInput.GetMethod("GetMouseButtonUp", new Type[] { typeof(int) }) ?? throw new NullReferenceException(nameof(m_getMouseButtonUpMethod));
		}

		public Type TInput { get; }

		private readonly PropertyInfo m_mousePositionProp;
		private readonly PropertyInfo m_mouseScrollDeltaProp;
		private readonly MethodInfo m_getKeyMethod;
		private readonly MethodInfo m_getKeyDownMethod;
		private readonly MethodInfo m_getKeyUpMethod;
		private readonly MethodInfo m_getMouseButtonMethod;
		private readonly MethodInfo m_getMouseButtonDownMethod;
		private readonly MethodInfo m_getMouseButtonUpMethod;

		public bool GetKey(KeyCode key) => (bool?)m_getKeyMethod.Invoke(null, new object[] { key }) ?? throw new NullReferenceException();

		public bool GetKeyDown(KeyCode key) => (bool?)m_getKeyDownMethod.Invoke(null, new object[] { key }) ?? throw new NullReferenceException();

		public bool GetKeyUp(KeyCode key) => (bool?)m_getKeyUpMethod.Invoke(null, new object[] { key }) ?? throw new NullReferenceException();

		public bool GetMouseButton(int btn) => (bool?)m_getMouseButtonMethod.Invoke(null, new object[] { btn }) ?? throw new NullReferenceException();

		public bool GetMouseButtonDown(int btn) => (bool?)m_getMouseButtonDownMethod.Invoke(null, new object[] { btn }) ?? throw new NullReferenceException();

		public bool GetMouseButtonUp(int btn) => (bool?)m_getMouseButtonUpMethod.Invoke(null, new object[] { btn }) ?? throw new NullReferenceException();

		public Vector2 MousePosition => (Vector3?)m_mousePositionProp.GetValue(null, null) ?? throw new NullReferenceException();

		public Vector2 MouseScrollDelta => (Vector2?)m_mouseScrollDeltaProp.GetValue(null, null) ?? throw new NullReferenceException();
	}
    internal sealed class InputSystem
	{
		public InputSystem()
		{
			MelonLoader.MelonLogger.Msg($"Loading Unity.InputSystem: {ReflectionHelpers.LoadModule("Unity.InputSystem")}");
			// MelonLoader.MelonLogger.Msg($"Loading UnityEngine.InputSystem: {ReflectionHelpers.LoadModule("UnityEngine.InputLegacyModule")}");
			TKeyboard = ReflectionHelpers.GetTypeByName("UnityEngine.InputSystem.Keyboard") ?? throw new NullReferenceException(nameof(TKeyboard));
			TMouse = ReflectionHelpers.GetTypeByName("UnityEngine.InputSystem.Mouse") ?? throw new NullReferenceException(nameof(TMouse));
			TKey = ReflectionHelpers.GetTypeByName("UnityEngine.InputSystem.Key") ?? throw new NullReferenceException(nameof(TKey));

			m_kbCurrentProp = TKeyboard.GetProperty("current") ?? throw new NullReferenceException(nameof(m_kbCurrentProp));
			CurrentKeyboard = m_kbCurrentProp.GetValue(null, null) ?? throw new NullReferenceException(nameof(CurrentKeyboard));
			m_kbIndexer = TKeyboard.GetProperty("Item", new Type[] { TKey }) ?? throw new NullReferenceException(nameof(m_kbIndexer));

			Type btnControl = ReflectionHelpers.GetTypeByName("UnityEngine.InputSystem.Controls.ButtonControl") ?? throw new NullReferenceException(nameof(btnControl));
			m_btnIsPressedProp = btnControl.GetProperty("isPressed") ?? throw new NullReferenceException(nameof(m_btnIsPressedProp));
			m_btnWasPressedProp = btnControl.GetProperty("wasPressedThisFrame") ?? throw new NullReferenceException(nameof(m_btnWasPressedProp));
			m_btnWasReleasedProp = btnControl.GetProperty("wasReleasedThisFrame") ?? throw new NullReferenceException(nameof(m_btnWasReleasedProp));

			PropertyInfo m_mouseCurrentProp = TMouse.GetProperty("current") ?? throw new NullReferenceException(nameof(m_mouseCurrentProp));
			CurrentMouse = m_mouseCurrentProp.GetValue(null, null) ?? throw new NullReferenceException(nameof(CurrentMouse));

			PropertyInfo m_leftButtonProp = TMouse.GetProperty("leftButton") ?? throw new NullReferenceException(nameof(m_leftButtonProp));
			LeftMouseButton = m_leftButtonProp.GetValue(CurrentMouse, null) ?? throw new NullReferenceException(nameof(LeftMouseButton));

			PropertyInfo m_rightButtonProp = TMouse.GetProperty("rightButton") ?? throw new NullReferenceException(nameof(m_rightButtonProp));
			RightMouseButton = m_rightButtonProp.GetValue(CurrentMouse, null) ?? throw new NullReferenceException(nameof(RightMouseButton));

			PropertyInfo m_middleButtonProp = TMouse.GetProperty("middleButton") ?? throw new NullReferenceException(nameof(m_middleButtonProp));
			MiddleMouseButton = m_middleButtonProp.GetValue(CurrentMouse, null) ?? throw new NullReferenceException(nameof(MiddleMouseButton));

			PropertyInfo m_scrollProp = TMouse.GetProperty("scroll") ?? throw new NullReferenceException(nameof(m_kbCurrentProp));
			MouseScrollInfo = m_scrollProp.GetValue(CurrentMouse, null) ?? throw new NullReferenceException(nameof(MouseScrollInfo));

			m_positionProp = ReflectionHelpers.GetTypeByName("UnityEngine.InputSystem.Pointer")?
				.GetProperty("position")
				?? throw new NullReferenceException(nameof(m_kbCurrentProp));
			MousePositionInfo = m_positionProp.GetValue(CurrentMouse, null) ?? throw new NullReferenceException(nameof(MousePositionInfo));

			m_readVector2InputMethod = ReflectionHelpers.GetTypeByName("UnityEngine.InputSystem.InputControl`1")?
				.MakeGenericType(typeof(Vector2))
				.GetMethod("ReadValue")
				?? throw new NullReferenceException(nameof(m_kbCurrentProp));
		}

		public Type TKeyboard { get; }

		public Type TMouse { get; }

		public Type TKey { get; }

		private readonly PropertyInfo m_btnIsPressedProp;
		private readonly PropertyInfo m_btnWasPressedProp;
		private readonly PropertyInfo m_btnWasReleasedProp;

		private object CurrentKeyboard { get; }

		private readonly PropertyInfo m_kbCurrentProp;
		private readonly PropertyInfo m_kbIndexer;

		private object CurrentMouse { get; }

		private object LeftMouseButton { get; }

		private object RightMouseButton { get; }

		private object MiddleMouseButton { get; }

		private object MouseScrollInfo { get; }

		private object MousePositionInfo { get; }

		private readonly PropertyInfo m_positionProp;
		private readonly MethodInfo m_readVector2InputMethod;

		internal Dictionary<KeyCode, object> ActualKeyDict { get; } = new();
		internal Dictionary<string, string> enumNameFixes = new()
		{
			{ "Control", "Ctrl" },
			{ "Return", "Enter" },
			{ "Alpha", "Digit" },
			{ "Keypad", "Numpad" },
			{ "Numlock", "NumLock" },
			{ "Print", "PrintScreen" },
			{ "BackQuote", "Backquote" }
		};

		internal object GetActualKey(KeyCode key)
		{
			if (!ActualKeyDict.ContainsKey(key))
			{
				string s = key.ToString();
				try
				{
					if (enumNameFixes.First(it => s.Contains(it.Key)) is KeyValuePair<string, string> entry)
					{
						s = s.Replace(entry.Key, entry.Value);
					}
				}
				catch { }

				object parsed = Enum.Parse(TKey, s);
				object actualKey = m_kbIndexer.GetValue(CurrentKeyboard, new object[] { parsed }) ?? throw new NullReferenceException();

				ActualKeyDict.Add(key, actualKey);
			}

			return ActualKeyDict[key];
		}

		public bool GetKeyDown(KeyCode key) => (bool?)m_btnWasPressedProp.GetValue(GetActualKey(key), null) ?? throw new NullReferenceException();

		public bool GetKey(KeyCode key) => (bool?)m_btnIsPressedProp.GetValue(GetActualKey(key), null) ?? throw new NullReferenceException();

		public bool GetKeyUp(KeyCode key) => (bool?)m_btnWasReleasedProp.GetValue(GetActualKey(key), null) ?? throw new NullReferenceException();

		public bool GetMouseButtonDown(int btn)
		{
			return btn switch
			{
				0 => (bool?)m_btnWasPressedProp.GetValue(LeftMouseButton, null),
				1 => (bool?)m_btnWasPressedProp.GetValue(RightMouseButton, null),
				2 => (bool?)m_btnWasPressedProp.GetValue(MiddleMouseButton, null),
				_ => throw new NotImplementedException(),
			} ?? throw new NullReferenceException();
		}

		public bool GetMouseButton(int btn)
		{
			return btn switch
			{
				0 => (bool?)m_btnIsPressedProp.GetValue(LeftMouseButton, null),
				1 => (bool?)m_btnIsPressedProp.GetValue(RightMouseButton, null),
				2 => (bool?)m_btnIsPressedProp.GetValue(MiddleMouseButton, null),
				_ => throw new NotImplementedException(),
			} ?? throw new NullReferenceException();
		}

		public bool GetMouseButtonUp(int btn)
		{
			return btn switch
			{
				0 => (bool?)m_btnWasReleasedProp.GetValue(LeftMouseButton, null),
				1 => (bool?)m_btnWasReleasedProp.GetValue(RightMouseButton, null),
				2 => (bool?)m_btnWasReleasedProp.GetValue(MiddleMouseButton, null),
				_ => throw new NotImplementedException(),
			} ?? throw new NullReferenceException();
		}

		public Vector2 MousePosition
		{
			get
			{
				try
				{
					return (Vector2?)m_readVector2InputMethod.Invoke(MousePositionInfo, Array.Empty<object>()) ?? throw new NullReferenceException();
				}
				catch
				{
					return Vector2.zero;
				}
			}
		}

		public Vector2 MouseScrollDelta
		{
			get
			{
				try
				{
					return (Vector2?)m_readVector2InputMethod.Invoke(MouseScrollInfo, Array.Empty<object>()) ?? throw new NullReferenceException();
				}
				catch
				{
					return Vector2.zero;
				}
			}
		}
	}
}