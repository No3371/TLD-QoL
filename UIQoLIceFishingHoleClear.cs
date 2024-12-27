
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_IceFishingHoleClear), nameof(Panel_IceFishingHoleClear.Update))]
internal class UIQoLIceFishingHoleClear
{
	private static void Postfix(Panel_IceFishingHoleClear __instance)
	{
		if (__instance.IsClearingIce()) return;
		if (KeyboardUtilities.InputManager.GetKeyDown(Settings.options.interactKey))
		{
			__instance.OnBreakIce();
			return;
		}
		if (KeyboardUtilities.InputManager.GetKeyDown(KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.PrevTool();
		}
		else if (KeyboardUtilities.InputManager.GetKeyDown(KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.NextTool();
		}
	}
}
