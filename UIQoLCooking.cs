
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.Update))]
internal class UIQoLCooking
{
	private static void Postfix(Panel_Cooking __instance)
	{
		if (__instance.m_RecipePrepOperation.InProgress) return;
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnCook();
			return;
		}


		if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
		{
			if (InputManager.GetScroll(InputManager.m_CurrentContext) > 0 || InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W))
			{
				__instance.m_ScrollBehaviour.SetSelectedIndex(Mathf.Max(0, __instance.m_ScrollBehaviour.SelectedIndex - 5));
			}
			else if ((InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
				&& __instance.m_ScrollBehaviour.m_TotalItems > 0f)
			{
				__instance.m_ScrollBehaviour.SetSelectedIndex(Mathf.Min(__instance.m_ScrollBehaviour.m_TotalItems - 1, __instance.m_ScrollBehaviour.SelectedIndex + 5));
			}
		}
		else
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
			&& KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.m_CategoryNavigation.OnNavigateUp();
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
			&& KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.m_CategoryNavigation.OnNavigateDown();
			}
		}
	}
}