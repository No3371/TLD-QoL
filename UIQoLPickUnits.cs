
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.Update))]
internal class UIQoLPickUnits
{
	internal static int lastOpened, lastOn, lastExecuted;
	static void Postfix(Panel_PickUnits __instance)
	{
		if (lastOn != Time.frameCount - 1) // Updated continuously when opened
			lastOpened = Time.frameCount;
		lastOn = Time.frameCount;

		if (lastOpened == Time.frameCount)
		{
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				lastExecuted = Time.frameCount;
				__instance.OnExecuteAll();
				return;
			}
		}

		if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
		 && InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A))
		{
			// __instance.m_numUnits = Mathf.Max(0, __instance.m_numUnits - 4);
			__instance.OnDecrease();
			__instance.OnDecrease();
			__instance.OnDecrease();
			__instance.OnDecrease();
			return;
		}
		else if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
		 && InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D))
		{
			// __instance.m_numUnits = Mathf.Min(__instance.m_maxUnits, __instance.m_numUnits + 4);
			__instance.OnIncrease();
			__instance.OnIncrease();
			__instance.OnIncrease();
			__instance.OnIncrease();
			return;
		}

		// if (lastOpened <= lastExecuted) return;
		if (KeyboardUtilities.InputManager.GetKey(Settings.options.interactKey)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
		 && !KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
		{
			if (Time.frameCount - lastOpened <= 1) return;
			__instance.OnExecuteAll();
			lastExecuted = lastOpened;
			return;
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			if (Time.frameCount - lastOpened <= 1) return;
			__instance.OnExecute();
			lastExecuted = lastOpened;
			return;
		}
	}
}
