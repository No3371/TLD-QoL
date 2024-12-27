
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_ActionPicker), nameof(Panel_ActionPicker.Update))]
internal class UIQoLActionPicker
{
	// internal static int index;
	private static void Postfix(Panel_ActionPicker __instance)
	{
		if (__instance.m_GenericProgressBar.IsEnabled()) return;
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnSelect(__instance.m_SelectedIndex);
			return;
		}
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.PreviousItem();
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.NextItem();
		}
	}
}


[HarmonyPatch(typeof(Panel_ActionPicker), nameof(Panel_ActionPicker.EnableWithCurrentList))]
internal class AcitonPickerResetOnEnable
{
	private static void Postfix(Panel_ActionPicker __instance)
	{
		__instance.NextItem();
		__instance.PreviousItem();
	}
}