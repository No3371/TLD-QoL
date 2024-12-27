
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_Clothing), nameof(Panel_Clothing.Update))]
internal class UIQoLClothing
{
	static int lastTriggerFrame;
	private static void Postfix(Panel_Clothing __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.dropKey))
		{
			__instance.OnDropItem();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey)
		 && Time.frameCount - lastTriggerFrame > 15
		 && __instance.GetCurrentlySelectedGearItem() != null)
		{
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.OnActionsButton();
			}
			else __instance.OnUseClothingItem();
			lastTriggerFrame = Time.frameCount;
			return;
		}
		if (InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
		{
			__instance.PrevTool();
		}
		else if (InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
		{
			__instance.NextTool();
		}
	}
}
