
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_GearSelect), nameof(Panel_GearSelect.Update))]
internal class UIQoLGearSelect
{
	private static void Postfix(ref Panel_GearSelect __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.SelectGear();
			return;
		}
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.PreviousGear();
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.NextGear();
		}
	}
}
