
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Update))]
internal class UIQoLInventory
{
	private static void Postfix(Panel_Inventory __instance)
	{
		if (__instance.m_PickUnits.IsEnabled()) return;
		if (__instance.m_ItemDescriptionPage.m_ProgressBar.IsEnabled()) return;
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			if (__instance.m_ItemDescriptionPage.m_MouseButtonExamine.active && Implementation.IM.GetKey(Settings.options.modifierKey))
			{
				__instance.OnExamine();
			}
			else __instance.OnEquip();
			return;
		}
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.dropKey))
		{
			var gi = __instance.GetCurrentlySelectedItem().m_GearItem;
			if (gi == null || gi.m_CantDropItem) return;
			if (Implementation.IM.GetKey(Settings.options.modifierKey)
			&& !Implementation.IM.GetKey(Settings.options.bulkKey))
			{
				if (GameManager.GetSafehouseManager()?.IsCustomizing() == true)
					GameManager.GetSafehouseManager()?.StopCustomizing();
				var toDrop = gi.m_StackableItem?.DefaultUnitsInItem ?? 1;
				toDrop = Mathf.Clamp(toDrop, 0, gi?.m_StackableItem?.m_Units ?? 1);
				var dropped = gi.Drop(toDrop);
				__instance.OnBack();
				dropped.PerformAlternativeInteraction();
				return;
			}
			else
			{
				__instance.OnDrop();
				return;
			}
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.End))
		{
			__instance.ScrollToBottom();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
			 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			__instance.m_SelectedSortIndex = Math.Clamp(++__instance.m_SelectedSortIndex, 0, __instance.m_SortButtons.Length - 1);
			__instance.OnSortChange(__instance.m_SortButtons[__instance.m_SelectedSortIndex]);
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
			 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			__instance.m_SelectedSortIndex = Math.Clamp(--__instance.m_SelectedSortIndex, 0, __instance.m_SortButtons.Length - 1);
			__instance.OnSortChange(__instance.m_SortButtons[__instance.m_SelectedSortIndex]);
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
		 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			if (__instance.m_SelectedFilterIndex <= 0) __instance.m_SelectedFilterIndex += 10;
			__instance.m_SelectedFilterIndex = Math.Clamp(--__instance.m_SelectedFilterIndex, 0, __instance.m_FilterButtons.Length - 1);
			__instance.OnFilterChange(__instance.m_FilterButtons[__instance.m_SelectedFilterIndex]);
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
		 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			if (__instance.m_SelectedFilterIndex >= __instance.m_FilterButtons.Length - 1) __instance.m_SelectedFilterIndex -= 10;
			__instance.m_SelectedFilterIndex = Math.Clamp(++__instance.m_SelectedFilterIndex, 0, __instance.m_FilterButtons.Length - 1);
			__instance.OnFilterChange(__instance.m_FilterButtons[__instance.m_SelectedFilterIndex]);
		}

	}
}
