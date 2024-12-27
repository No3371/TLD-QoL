
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_Rest), nameof(Panel_Rest.Update))]
internal class UIQoLRestPanel
{
	private static void Postfix(ref Panel_Rest __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			if (__instance.m_PassTimeButtonObject.active)
				__instance.OnPassTime();
			else if (__instance.m_SleepButton.active)
				__instance.OnRest();
			return;
		}
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
		 && __instance.m_PassTimeButtonObject.active)
		{
			__instance.OnSelectRest();
			return;
		}

		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
		 && __instance.m_SleepButton.active)
		{
			__instance.OnSelectPassTime();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.OnDecreaseHours();
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				__instance.OnDecreaseHours(); __instance.OnDecreaseHours(); __instance.OnDecreaseHours(); __instance.OnDecreaseHours();
			}
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.OnIncreaseHours();
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				__instance.OnIncreaseHours(); __instance.OnIncreaseHours(); __instance.OnIncreaseHours(); __instance.OnIncreaseHours();
			}
		}
	}
}
