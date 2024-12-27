
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_IceFishing), nameof(Panel_IceFishing.Update))]
internal class UIQoLIceFishing
{
	private static void Postfix(Panel_IceFishing __instance)
	{
		if (__instance.IsFishing()) return;
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnFish();
			return;
		}
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.OnDecreaseHours();
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.OnIncreaseHours();
		}
	}
}
