
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;


[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.Update))]
internal class UIQoLContainer
{
	private static void Postfix(ref Panel_Container __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			var canMove = __instance.m_SelectedTable == SelectedTableEnum.ContainerTable && __instance.CanMoveItemToInventory(__instance.GetCurrentlySelectedItem());
			canMove |= __instance.m_SelectedTable == SelectedTableEnum.InventoryTable && __instance.CanMoveItemToContainer(__instance.GetCurrentlySelectedItem());
			if (canMove)
				__instance.OnMoveItem();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
		{
			__instance.m_SelectedSortIndex = Math.Clamp(++__instance.m_SelectedSortIndex, 0, __instance.m_SortButtons.Length - 1);
			__instance.OnSortInventoryChange(__instance.m_SortButtons[__instance.m_SelectedSortIndex]);
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
		{
			__instance.m_SelectedSortIndex = Math.Clamp(--__instance.m_SelectedSortIndex, 0, __instance.m_SortButtons.Length - 1);
			__instance.OnSortInventoryChange(__instance.m_SortButtons[__instance.m_SelectedSortIndex]);
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
		{
			Panel_Inventory panel_Inventory = __instance.m_Inventory.GetPanel();
			panel_Inventory.m_SelectedFilterIndex = Math.Clamp(--panel_Inventory.m_SelectedFilterIndex, 0, __instance.m_FilterButtons.Length - 1);
			__instance.OnFilterInventoryChange(__instance.m_FilterButtons[panel_Inventory.m_SelectedFilterIndex]);
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
		 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
		{
			Panel_Inventory panel_Inventory = __instance.m_Inventory.GetPanel();
			panel_Inventory.m_SelectedFilterIndex = Math.Clamp(++panel_Inventory.m_SelectedFilterIndex, 0, __instance.m_FilterButtons.Length - 1);
			__instance.OnFilterInventoryChange(__instance.m_FilterButtons[panel_Inventory.m_SelectedFilterIndex]);
		}

	}
}
