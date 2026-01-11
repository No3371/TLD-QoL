
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(CraftingRequirementContainer), nameof(CraftingRequirementContainer.Enable))]
internal class CraftingUILocator
{
	internal static CraftingRequirementContainer UIComp { get; private set; }
	private static void Postfix(CraftingRequirementContainer __instance)
	{
		UIComp = __instance;
	}
}

[HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.Update))]
internal class UIQoLCrafting
{
	private static void Postfix(Panel_Crafting __instance)
	{
		if (__instance.m_CraftingOperation?.InProgress ?? false) return;

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnBeginCrafting();
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
		 && Implementation.IM.GetKey(Settings.options.bulkKey))
		{
			__instance.m_ScrollBehaviour.SetSelectedIndex(Mathf.Max(0, __instance.m_ScrollBehaviour.SelectedIndex - 4));
			return;
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
		 && Implementation.IM.GetKey(Settings.options.bulkKey))
		{
			__instance.m_ScrollBehaviour.SetSelectedIndex(Mathf.Min(__instance.m_ScrollBehaviour.m_TotalItems - 1, __instance.m_ScrollBehaviour.SelectedIndex + 4));
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
		 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			__instance.m_CurrentFilter = __instance.m_CurrentFilter - 1;
			__instance.m_CurrentFilter = (Panel_Crafting.Filter)Math.Clamp((int)__instance.m_CurrentFilter, 0, __instance.m_FilterButtons.Count - 1);
			__instance.OnFilterChange(__instance.m_FilterButtons[(int)__instance.m_CurrentFilter]);
			return;
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
		 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			__instance.m_CurrentFilter = __instance.m_CurrentFilter + 1;
			__instance.m_CurrentFilter = (Panel_Crafting.Filter)Math.Clamp((int)__instance.m_CurrentFilter, 0, __instance.m_FilterButtons.Count - 1);
			__instance.OnFilterChange(__instance.m_FilterButtons[(int)__instance.m_CurrentFilter]);
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
		 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			__instance.m_CategoryNavigation.OnNavigateUp();
			// __instance.m_CurrentCategory = __instance.m_CurrentCategory - 1;
			// __instance.m_CurrentCategory = (Panel_Crafting.Category) Math.Clamp((int) __instance.m_CurrentCategory, 0, __instance.m_CategoryNavigation.m_NavigationButtons.Count);
			// __instance.OnCategoryChanged((int) __instance.m_CurrentCategory);
			return;
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
		 && Implementation.IM.GetKey(Settings.options.modifierKey))
		{
			__instance.m_CategoryNavigation.OnNavigateDown();
			// __instance.m_CurrentCategory = __instance.m_CurrentCategory + 1;
			// __instance.m_CurrentCategory = (Panel_Crafting.Category) Math.Clamp((int) __instance.m_CurrentCategory, 0, __instance.m_CategoryNavigation.m_NavigationButtons.Count);
			// __instance.OnCategoryChanged((int) __instance.m_CurrentCategory);
			return;
		}

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
			 && CraftingUILocator.UIComp.enabled)
		{
			if (CraftingUILocator.UIComp.m_SingleTool.enabled)
			{
				CraftingUILocator.UIComp.OnPrevious();
			}
			else if (CraftingUILocator.UIComp.m_QuantitySelect.enabled)
			{
				CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease();
				if (Implementation.IM.GetKey(Settings.options.bulkKey))
				{
					CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease(); CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease(); CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease(); CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease();
				}
			}
		}
		else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
			 && CraftingUILocator.UIComp.enabled)
		{
			if (CraftingUILocator.UIComp.m_SingleTool.enabled)
			{
				CraftingUILocator.UIComp.OnNext();
			}
			else if (CraftingUILocator.UIComp.m_QuantitySelect.enabled)
			{
				CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();
				if (Implementation.IM.GetKey(Settings.options.bulkKey))
				{
					CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease(); CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease(); CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease(); CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();
				}
			}
		}
	}
}
