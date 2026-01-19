
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.Update))]
internal class UIQoLExamine
{
	private static void Postfix(Panel_Inventory_Examine __instance)
	{
		if (__instance.IsCleaning() || __instance.IsRepairing() || __instance.IsHarvesting() || __instance.IsReading() || __instance.IsSharpening() || __instance.m_ActionInProgressWindow.active || InterfaceManager.GetPanel<Panel_GenericProgressBar>().isActiveAndEnabled)
			return;

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			if (__instance.m_MenuItemHarvest != null && __instance.m_MenuItemHarvest.m_Selected && __instance.m_Button_Harvest.enabled && __instance.CanHarvest())
				if (__instance.m_ToolWindowActive)
					__instance.OnSelectActionTool();
				else
					__instance.OnHarvest();
			else if (__instance.m_MenuItemSharpen != null && __instance.m_MenuItemSharpen.m_Selected && __instance.m_Button_Sharpen.enabled)
			{
				if (__instance.m_ToolWindowActive)
					__instance.OnSelectActionTool();
				else
					__instance.OnSharpen();
			}
			else if (__instance.m_MenuItemRepair != null && __instance.m_MenuItemRepair.m_Selected && __instance.m_Button_Repair.enabled && __instance.CanRepair())
			{
				if (__instance.m_ToolWindowActive)
					__instance.OnSelectActionTool();
				else
					__instance.OnRepair();
			}
			else if (__instance.m_MenuItemClean != null && __instance.m_MenuItemClean.m_Selected && __instance.m_Button_Clean.enabled)
			{
				if (__instance.m_ToolWindowActive)
					__instance.OnSelectActionTool();
				else
					__instance.OnClean();
			}
			else if (__instance.m_MenuItemRefuel != null && __instance.m_MenuItemRefuel.m_Selected && __instance.m_Button_Refuel.enabled && __instance.CanRefuel() && __instance.m_RefuelPanel.active)
			{
				if (__instance.m_ToolWindowActive)
					__instance.OnSelectActionTool();
				else
					__instance.OnRefuel();
			}
			else if (__instance.m_MenuItemSafehouseCustomizationRepair != null && __instance.m_MenuItemSafehouseCustomizationRepair.m_Selected && __instance.m_Button_SafehouseCustomizationRepair.enabled && __instance.CanRepair() && __instance.m_SafehouseCustomizationRepairPanel.active)
			{
				if (__instance.m_ToolWindowActive)
					__instance.OnSelectActionTool();
				else
					__instance.OnRepair();
			}
			else if (__instance.m_MenuItemUnload != null && __instance.m_MenuItemUnload.m_Selected && __instance.m_Button_Unload.enabled && __instance.m_RifleUnloadPanel.active)
				__instance.OnUnload();
			else if (__instance.m_ReadPanel.active)
				__instance.OnRead();
			else
				EAPISupport.Instance?.OnPerformSelectedAction();

			return;
		}

		if (__instance.m_ReadPanel.active)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.OnReadHoursDecrease();
				if (Implementation.IM.GetKey(Settings.options.bulkKey))
				{
					__instance.OnReadHoursDecrease(); __instance.OnReadHoursDecrease(); __instance.OnReadHoursDecrease(); __instance.OnReadHoursDecrease();
				}
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.OnReadHoursIncrease();
				if (Implementation.IM.GetKey(Settings.options.bulkKey))
				{
					__instance.OnReadHoursIncrease(); __instance.OnReadHoursIncrease(); __instance.OnReadHoursIncrease(); __instance.OnReadHoursIncrease();
				}
			}
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A))
		{
			EAPISupport.Instance?.OnPreviousSubAction();
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D))
		{
			EAPISupport.Instance?.OnNextSubAction();
		}
	}
}
