
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_BreakDown), nameof(Panel_BreakDown.Update))]
internal class UIQoLBreakdown
{
	private static void Postfix(ref Panel_BreakDown __instance)
	{
		if (__instance.IsBreakingDown()) return;

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnBreakDown();
			return;
		}
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.OnPrevTool();
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.OnNextTool();
		}
	}
}
