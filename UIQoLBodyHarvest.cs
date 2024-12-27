
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_BodyHarvest), nameof(Panel_BodyHarvest.Update))]
internal class UIQoLBodyHarvest
{
	private static void Postfix(ref Panel_BodyHarvest __instance)
	{
		if (__instance.IsHarvestingOrQuartering()) return;


		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
		 && __instance.IsTabHarvestSelected()
		 && __instance.m_SelectedButtonIndex == 0)
		{
			__instance.OnIncreaseMeatHarvest();
			__instance.OnIncreaseMeatHarvest();
			__instance.OnIncreaseMeatHarvest();
			__instance.OnIncreaseMeatHarvest();
			return;
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
		 && __instance.IsTabHarvestSelected()
		 && __instance.m_SelectedButtonIndex == 0)
		{
			__instance.OnDecreaseMeatHarvest();
			__instance.OnDecreaseMeatHarvest();
			__instance.OnDecreaseMeatHarvest();
			__instance.OnDecreaseMeatHarvest();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
		 && __instance.IsTabHarvestSelected()
		 && __instance.m_SelectedButtonIndex == 2)
		{
			__instance.OnIncreaseGutHarvest();
			__instance.OnIncreaseGutHarvest();
			__instance.OnIncreaseGutHarvest();
			__instance.OnIncreaseGutHarvest();
			return;
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
		 && __instance.IsTabHarvestSelected()
		 && __instance.m_SelectedButtonIndex == 2)
		{
			__instance.OnDecreaseGutHarvest();
			__instance.OnDecreaseGutHarvest();
			__instance.OnDecreaseGutHarvest();
			__instance.OnDecreaseGutHarvest();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
		 && __instance.IsTabHarvestSelected()
		 && __instance.m_HarvestTabButtonRight.active)
		{
			__instance.OnTabQuarterSelected();
			return;
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
			 && __instance.IsTabQuarterSelected())
		{
			__instance.OnTabHarvestSelected();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			if (__instance.IsTabHarvestSelected())
				__instance.OnHarvest();
			else if (__instance.IsTabQuarterSelected())
				__instance.OnQuarter();
			return;
		}

		float v = InputManager.GetScroll(InputManager.m_CurrentContext);
		if (v > 0)
			__instance.OnToolPrev();
		else if (InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			__instance.OnToolNext();
	}
}
