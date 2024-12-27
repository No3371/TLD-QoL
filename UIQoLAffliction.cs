
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_Affliction), nameof(Panel_Affliction.Update))]
internal class UIQoLAffliction
{
	private static void Postfix(Panel_Affliction __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.TreatWound();
			return;
		}
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.PreviousAffliction();
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.NextAffliction();
		}
	}
}
