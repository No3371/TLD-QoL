using System.Collections;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace QoL
{
	public class Implementation : MelonMod
	{
		public static float lastBulkDown = 0, lastBulkUp = 1;
		public static bool HoldingBulkKey => lastBulkDown > lastBulkUp;
        public override void OnInitializeMelon()
		{
			MelonLogger.Msg($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
		}

        public override void OnUpdate()
        {
			if (KeyboardUtilities.InputManager.GetKeyDown(Settings.options.bulkKey))
				lastBulkDown = Time.unscaledTime;
			else if (KeyboardUtilities.InputManager.GetKeyUp(Settings.options.bulkKey))
				lastBulkUp = Time.unscaledTime;
            base.OnUpdate();
        }
    }

	[HarmonyPatch(typeof(Panel_GearSelect), nameof(Panel_GearSelect.Update))]
	internal class GearSelectADScroll
	{
		private static void Postfix(ref Panel_GearSelect __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.SelectGear();
				return;
			}
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.PreviousGear();
			}
            else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.NextGear();
			}
		}
	}
	[HarmonyPatch(typeof(Panel_Rest), nameof(Panel_Rest.Update))]
	internal class RestHoursADScrollAndAlternativeSleep
	{
		private static void Postfix(ref Panel_Rest __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				if (__instance.m_PassTimeButtonObject.active)
					__instance.OnPassTime();
				else if (__instance.m_SleepButton.active)
					__instance.OnRest();
				return;
			}
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
			 && __instance.m_SleepButton.active)
			{
				__instance.OnSelectRest();
				return;
			}
	
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
			 && __instance.m_PassTimeButtonObject.active)
			{
				__instance.OnSelectPassTime();
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

	[HarmonyPatch(typeof(Panel_FirstAid), nameof(Panel_FirstAid.Update))]
	internal class FirstAidADScrollAndAlternativeFirstAid
	{
		private static void Postfix(ref Panel_FirstAid __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.UseSelectedItem();
				return;
			}
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.SelectPrevFAKItem();
			}
            else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.SelectNextFAKItem();
			}
		}
	}
	[HarmonyPatch(typeof(Panel_Affliction), nameof(Panel_Affliction.Update))]
	internal class AfflicationADScrollAndAlternativeTreatWound
	{
		private static void Postfix(ref Panel_Affliction __instance)
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

	[HarmonyPatch(typeof(Panel_BodyHarvest), nameof(Panel_BodyHarvest.Update))]
	internal class BodyHarvestToolScrollAndAlternativeHarvest
	{
		private static void Postfix(ref Panel_BodyHarvest __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnHarvest();
				return;
			}
            if (InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.OnToolPrev();
			}
            else if (InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.OnToolNext();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_FireStart), nameof(Panel_FireStart.Update))]
	internal class AlternativeStartFire
	{
		private static void Postfix(ref Panel_FireStart __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnStartFire();
				return;
			}
		}
	}

	[HarmonyPatch(typeof(Panel_BreakDown), nameof(Panel_BreakDown.Update))]
	internal class BreakDownToolADScrollAndAlternativeBreakDown
	{
		private static void Postfix(ref Panel_BreakDown __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnBreakDown();
				return;
			}
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.OnPrevTool();
			}
            else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.OnNextTool();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Update))]
	internal class AlternativeFeedFire
	{
		private static void Postfix(ref Panel_FeedFire __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnFeedFire();
				return;
			}
		}
	}

	[HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.Update))]
	internal class AlternativeLightTorch
	{
		private static void Postfix(ref Panel_TorchLight __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnUseSelectedItem();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.Update))]
	internal class AlternativeCook
	{
		private static void Postfix(ref Panel_Cooking __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				if (__instance.SelectedFoodIsSnow())
					__instance.OnMeltSnow();
				else if (__instance.SelectedFoodIsWater())
					__instance.OnBoil();
				else
					__instance.OnCook();
			}
		}
	}


	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.Update))]
	internal class AlternativeConfirmUnits
	{
		private static void Postfix(ref Panel_PickUnits __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnExecute();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_SprayPaint), nameof(Panel_SprayPaint.Update))]
	internal class SprayADScrollAndAlternativeConfirmSprayShape
	{
		private static void Postfix(ref Panel_SprayPaint __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnButtonConfirm();
				return;
			}
            if (InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.OnButtonScrollLeft();
			}
            else if (InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.OnButtonScrollRight();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Update))]
	internal class InventoryPlus
	{
		private static void Postfix(Panel_Inventory __instance)
		{
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.dropKey)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
			 && !KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				var gi = __instance.GetCurrentlySelectedGearItem();
				if (gi.m_CantDropItem) return;
				var singleGI = gi.Drop(1);
				__instance.OnBack();
				singleGI.TryStartPlaceMeshInteraction();
				return;
			}

            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.End))
			 	__instance.ScrollToBottom();

            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.m_SelectedSortIndex = Math.Clamp(++__instance.m_SelectedSortIndex, 0, __instance.m_SortButtons.Length - 1);
			 	__instance.OnSortChange(__instance.m_SortButtons[__instance.m_SelectedSortIndex]);
			}
            else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.m_SelectedSortIndex = Math.Clamp(--__instance.m_SelectedSortIndex, 0, __instance.m_SortButtons.Length - 1);
			 	__instance.OnSortChange(__instance.m_SortButtons[__instance.m_SelectedSortIndex]);
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				if (__instance.m_SelectedFilterIndex <= 0) __instance.m_SelectedFilterIndex += 10;
				__instance.m_SelectedFilterIndex = Math.Clamp(--__instance.m_SelectedFilterIndex, 0, __instance.m_FilterButtons.Length - 1);
			 	__instance.OnFilterChange(__instance.m_FilterButtons[__instance.m_SelectedFilterIndex]);
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				if (__instance.m_SelectedFilterIndex >= __instance.m_FilterButtons.Length - 1) __instance.m_SelectedFilterIndex -= 10;
				__instance.m_SelectedFilterIndex = Math.Clamp(++__instance.m_SelectedFilterIndex, 0, __instance.m_FilterButtons.Length - 1);
			 	__instance.OnFilterChange(__instance.m_FilterButtons[__instance.m_SelectedFilterIndex]);
			}

		}
	}

	/// REMINDER: Can't patch this
	// [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Enable))]
	// internal class Panel_InventoryEnabled
	// {
	// 	static void Postfix (Panel_Inventory __instance)
	// 	{
	// 		__instance.m_SelectedFilterIndex = 0;
	// 	}
	// }

	[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.Update))]
	internal class ContainerPlus
	{
		private static void Postfix(ref Panel_Container __instance)
		{
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

	[HarmonyPatch(typeof(InventoryGridItem), nameof(InventoryGridItem.Update))]
	internal class QuickInteractInventoryGrid
	{
		static int lastTriggerFrame;
		private static void Postfix(ref InventoryGridItem __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey)
			 && Time.frameCount - lastTriggerFrame > 15 && __instance.IsSelected())
			{
				if (KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
				{
					__instance.m_Inventory.GetPanel().OnExamine();
				}
				else __instance.DoubleClick();
				lastTriggerFrame = Time.frameCount;
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Clothing), nameof(Panel_Clothing.Update))]
	internal class QuickInteractClothing
	{
		static int lastTriggerFrame;
		private static void Postfix(ref Panel_Clothing __instance)
		{
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

	[HarmonyPatch(typeof(Panel_Repair), nameof(Panel_Repair.Update))]
	internal class AlternativeRepairPanelRepair
	{
		private static void Postfix(ref Panel_Repair __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnRepair();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.Update))]
	internal class AlternativeActionsPanelExecute
	{
		private static void Postfix(ref Panel_Inventory_Examine __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{			
				if (__instance.m_MenuItemHarvest != null && __instance.m_MenuItemHarvest.m_Selected)
					if (__instance.m_ActionToolSelect.active)
						__instance.OnSelectActionTool();
					else
						__instance.OnHarvest();
				else if (__instance.m_MenuItemSharpen != null && __instance.m_MenuItemSharpen.m_Selected)
					__instance.OnSharpen();
				else if (__instance.m_MenuItemRepair != null && (__instance.m_MenuItemRepair.m_Selected))
				{
					if (__instance.m_ActionToolSelect.active)
						__instance.OnSelectActionTool();
					else
						__instance.OnRepair();
				}
				else if (__instance.m_MenuItemClean != null && __instance.m_MenuItemClean.m_Selected)
					if (__instance.m_ActionToolSelect.active)
						__instance.OnSelectActionTool();
					else
						__instance.OnClean();
				else if (__instance.m_MenuItemRefuel != null && __instance.m_MenuItemRefuel.m_Selected)
					if (__instance.m_ActionToolSelect.active)
						__instance.OnSelectActionTool();
					else
						__instance.OnRefuel();
				else if (__instance.m_MenuItemUnload != null && __instance.m_MenuItemUnload.m_Selected)
					__instance.OnUnload();
				else if (__instance.m_ReadPanel.active)
					__instance.OnRead();
			}
		}
	}


	[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.Update))]
	internal class ReadHoursADScroll
	{
		private static void Postfix(ref Panel_Inventory_Examine __instance)
		{
			if (!__instance.m_ReadPanel.active)
				return;
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.OnReadHoursDecrease();
			}
            else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.OnReadHoursIncrease();
			}
		}
	}

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
	internal class AlternatvieCraft
	{
		private static void Postfix(ref Panel_Crafting __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnBeginCrafting();
				return;
			}

			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				// __instance.m_CurrentBlueprintIndex -= 4;
				// __instance.m_CurrentBlueprintIndex = Math.Clamp((int) __instance.m_CurrentBlueprintIndex, 0, __instance.m_BlueprintDisplays.Count - 1);
				__instance.HandleVerticalNavigation(1);__instance.HandleVerticalNavigation(1);__instance.HandleVerticalNavigation(1);
			 	// __instance.OnBlueprintDisplayClicked(__instance.m_CurrentBlueprintIndex);
				return;
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				// __instance.m_CurrentBlueprintIndex += 4;
				// __instance.m_CurrentBlueprintIndex = Math.Clamp((int) __instance.m_CurrentBlueprintIndex, 0, __instance.m_BlueprintDisplays.Count - 1);
			 	// __instance.OnBlueprintDisplayClicked(__instance.m_CurrentBlueprintIndex);
				__instance.HandleVerticalNavigation(-1);__instance.HandleVerticalNavigation(-1);__instance.HandleVerticalNavigation(-1);
				return;
			}

			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.m_CurrentFilter = __instance.m_CurrentFilter - 1;
				__instance.m_CurrentFilter = (Panel_Crafting.Filter) Math.Clamp((int) __instance.m_CurrentFilter, 0, __instance.m_FilterButtons.Count - 1);
			 	__instance.OnFilterChange(__instance.m_FilterButtons[(int)__instance.m_CurrentFilter]);
				return;
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.m_CurrentFilter = __instance.m_CurrentFilter + 1;
				__instance.m_CurrentFilter = (Panel_Crafting.Filter) Math.Clamp((int) __instance.m_CurrentFilter, 0, __instance.m_FilterButtons.Count - 1);
			 	__instance.OnFilterChange(__instance.m_FilterButtons[(int)__instance.m_CurrentFilter]);
				return;
			}

			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				__instance.m_CategoryNavigation.OnNavigateUp();
				// __instance.m_CurrentCategory = __instance.m_CurrentCategory - 1;
				// __instance.m_CurrentCategory = (Panel_Crafting.Category) Math.Clamp((int) __instance.m_CurrentCategory, 0, __instance.m_CategoryNavigation.m_NavigationButtons.Count);
			 	// __instance.OnCategoryChanged((int) __instance.m_CurrentCategory);
				return;
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
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
					if (Implementation.HoldingBulkKey)
					{
						CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease();CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease();CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease();CraftingUILocator.UIComp.m_QuantitySelect.OnDecrease();
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
					if (Implementation.HoldingBulkKey)
					{
						CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();
					}
				}
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Update))]
	internal class QuickDrop
	{
		static bool warned;
		private static void Postfix(ref Panel_Inventory __instance)
		{
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.dropKey))
			{
				var gi = __instance.GetCurrentlySelectedGearItem();
				if (gi.m_CantDropItem) return;
				__instance.OnDrop();
            	// if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.stackTransferKey))
				// 	__instance.GetCurrentlySelectedGearItem()?.Drop(__instance.m_NumColumns.GetCurrentlySelectedGearItem().);
				// else
				// 	__instance.GetCurrentlySelectedGearItem()?.Drop(1);
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnMeltSnowUp))]
	internal class BulkIncreaseMeltUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_Cooking __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnMeltSnowUp();
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnMeltSnowDown))]
	internal class BulkDecreaseMeltUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_Cooking __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnMeltSnowDown();
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnWaterUp))]
	internal class BulkIncreaseCookingWaterUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_Cooking __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnWaterUp();
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnWaterDown))]
	internal class BulkDecreaseCookingWaterUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_Cooking __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnWaterDown();
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnBoilUp))]
	internal class BulkIncreaseBoilUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_Cooking __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnBoilUp();
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnBoilDown))]
	internal class BulkDecreaseBoilUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_Cooking __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnBoilDown();
		}
	}

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.OnIncrease))]
	internal class BulkIncreaseUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_PickUnits __instance)
		{
			// MelonLogger.Msg(__instance.m_numUnits + "/" + __instance.m_maxUnits);
            // if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
			// {
			// 	__instance.m_numUnits += 4;
			// 	__instance.m_numUnits = Math.Min(__instance.m_maxUnits, __instance.m_numUnits);
			// }
			// This doesn't really work, so let's go rude.
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnIncrease();
		}
	}

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.OnDecrease))]
	internal class BulkDecreaseUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_PickUnits __instance)
		{
			// MelonLogger.Msg(__instance.m_numUnits + "/" + __instance.m_maxUnits);
            // if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
			// {
			// 	__instance.m_numUnits -= 4;
			// 	__instance.m_numUnits = Math.Max(0, __instance.m_numUnits);
			// }
			// This doesn't really work, so let's go rude.
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnDecrease();
		}
	}

	[HarmonyPatch(typeof(Panel_PickWater), nameof(Panel_PickWater.OnIncrease))]
	internal class BulkIncreaseWaterUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_PickWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnIncrease();
		}
	}

	[HarmonyPatch(typeof(Panel_PickWater), nameof(Panel_PickWater.OnDecrease))]
	internal class BulkDecreaseWaterUnits
	{
		static int count = 0;
		private static void Postfix(ref Panel_PickWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnDecrease();
		}
	}

	[HarmonyPatch(typeof(InputManager), nameof(InputManager.GetInteractPressed))]
	internal class AlternativeInteract
	{
		private static bool Postfix(bool __result)
		{
            if (!__result && KeyboardUtilities.InputManager.GetKeyDown(Settings.options.interactKey))
			{
				__result = true;
			}

			return __result;
		}
	}
	[HarmonyPatch(typeof(InputManager), nameof(InputManager.GetPickupPressed))]
	internal class AlternativePickup
	{
		private static bool Postfix(bool __result)
		{
            if (!__result && KeyboardUtilities.InputManager.GetKeyDown(Settings.options.interactKey))
			{
				__result = true;
			}

			return __result;
		}
	}

	[HarmonyPatch(typeof(InputManager), nameof(InputManager.GetPutBackPressed))]
	internal class AlternativePutBack
	{
		private static bool Postfix(bool __result)
		{
            if (!__result && InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.backKey))
			{
				__result = true;
			}

			return __result;
		}
	}

	[HarmonyPatch(typeof(InputManager), nameof(InputManager.GetEscapePressed))]
	internal class AlternativeEscape
	{
		private static bool Postfix(bool __result)
		{
            if (!__result && InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.backKey))
			{
				__result = true;
			}

			return __result;
		}
	}

	// [HarmonyPatch(typeof(DragDropContainer), nameof(DragDropContainer.OnDragOut))]
	// internal class DragDropContainerOnDragOutPatch
	// {
	// 	private static void Prefix () => MelonLogger.Msg("Pre- OnDragOut");
	// 	private static void Postfix() => MelonLogger.Msg("Post- OnDragOut");
	// }

	// [HarmonyPatch(typeof(DragDropContainer), nameof(DragDropItem.OnDragOver))]
	// internal class DragDropContainerOnDragOverPatch
	// {
	// 	private static void Prefix () => MelonLogger.Msg("Pre- OnDragOver");
	// 	private static void Postfix() => MelonLogger.Msg("Post- OnDragOver");
	// }
	
	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForTransferToInventory))]
	internal class PickUnitsToInventory
	{
		internal static bool warned;
		static void Postfix (ref Panel_PickUnits __instance)
		{
			if (Implementation.HoldingBulkKey)
			{
				__instance.OnExecuteAll();
				if (!warned)
				{
					warned = true;
					MelonCoroutines.Start(Warning());
				}
			}
		}


		internal static IEnumerator Warning ()
		{
			InterfaceManager.GetPanel<Panel_HUD>().DisplayWarningMessage("QoL: First StackTransfer, could have closed the UI");
			yield return new WaitForSeconds(5);
			InterfaceManager.GetPanel<Panel_HUD>().ClearWarningMessage();
		}
	}

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.Update))]
	internal class PickUnitsToContainer
	{
		static void Postfix (ref Panel_PickUnits __instance)
		{
			if (Implementation.HoldingBulkKey)
			{
				__instance.OnAll();
				__instance.Refresh();
				__instance.OnExecuteAll();
				if (!PickUnitsToInventory.warned)
				{
					PickUnitsToInventory.warned = true;
					MelonCoroutines.Start(PickUnitsToInventory.Warning());
				}
			}

		}
	}

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForTransferToContainer))]
	internal class PrePickUnitsToContainer
	{
		static void Postfix (ref Panel_PickUnits __instance)
		{
			if (Implementation.HoldingBulkKey)
			{
				__instance.OnAll();
				__instance.Refresh();
				__instance.OnExecuteAll();
				if (!PickUnitsToInventory.warned)
				{
					PickUnitsToInventory.warned = true;
					MelonCoroutines.Start(PickUnitsToInventory.Warning());
				}
			}

		}
	}

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForDrop))]
	internal class PickUnitsToDrop
	{
		static bool warned;
		static void Postfix (ref Panel_PickUnits __instance)
		{
			if (Implementation.HoldingBulkKey)
			{
				__instance.OnExecuteAll();
				if (!warned)
				{
					warned = true;
					MelonCoroutines.Start(Warning());
				}
			}
		}

		static IEnumerator Warning ()
		{
			InterfaceManager.GetPanel<Panel_HUD>().DisplayWarningMessage("QoL: First trigger, if stuck, press Tab (or your status key) to reset");
			yield return new WaitForSeconds(5);
			InterfaceManager.GetPanel<Panel_HUD>().ClearWarningMessage();
		}
	}

	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
	internal class FastPickAndAlternativeTakeIt
	{
		public static int LastTriggerFrameConsumed { get; private set; }
		public static float LastFastPickTriggered { get; private set; }
		static void Postfix (ref PlayerManager __instance)
		{
			GearItem? gearItem = __instance?.GearItemBeingInspected();
			if (gearItem == null || !__instance.IsInspectModeActive()
			 || GearItemPreInspect.LastTriggerFrame <= LastTriggerFrameConsumed) return; 

			// Only active for items in world
			if (GearItemPreInspect.LastInspectType != GearItemPreInspect.InspectType.None) return;

            float sinceLastInspect = Time.unscaledTime - GearItemPreInspect.LastInspect;
            bool trigger = false;

			if (!trigger)
				trigger = (InputManager.GetFireReleased(InputManager.m_CurrentContext) || KeyboardUtilities.InputManager.GetKeyUp(Settings.options.interactKey))
					   && (sinceLastInspect > Settings.options.fastPickMin && sinceLastInspect < Settings.options.fastPickMax);

			if (trigger)
			{
				// if (gearItem.m_WaterSupply != null) __instance.RemoveWater(gearItem.m_WaterSupply, gearItem.m_WaterSupply.m_VolumeInLiters);
				// else if (gearItem.m_CookingPotItem != null)
				// MelonLogger.Msg("FastPick triggered!");
				__instance.ProcessPickupItemInteraction(gearItem, false, false, false);
				__instance.ExitInspectGearMode(false);
				LastFastPickTriggered = Time.unscaledTime;
				LastTriggerFrameConsumed = GearItemPreInspect.LastTriggerFrame;
			}
		}
	}

	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.EnterInspectGearMode), new Type[] { typeof(GearItem), typeof(Container), typeof(IceFishingHole), typeof(Harvestable), typeof(CookingPotItem) } )]
	internal class GearItemPreInspect
	{
		public static int LastTriggerFrame { get; private set; }
		public static float LastInspect { get; private set; }
		public static InspectType LastInspectType { get; private set; }

		public enum InspectType
		{
			None,
			Container,
			IceFishingHole,
			Harvestable,
			CookingPot
		}

		static void Postfix(GearItem __0, Container __1, IceFishingHole __2, Harvestable __3, CookingPotItem __4)
		{
			// MelonLogger.Msg(String.Format("Fastpick: {0} / Container: {1} / Fishing: {2} / Harvestable: {3} / Cooking: {4}", __0.name, __1 != null, __2 != null, __3 != null, __4 != null));
			LastInspectType = InspectType.None;
			if (__1 != null) LastInspectType = InspectType.Container;
			if (__2 != null) LastInspectType = InspectType.IceFishingHole;
			if (__3 != null) LastInspectType = InspectType.Harvestable;
			if (__4 != null) LastInspectType = InspectType.CookingPot;

			
			LastInspect = Time.unscaledTime;
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.interactKey)
			 && !InputManager.GetFirePressed(InputManager.m_CurrentContext)) return;

			// MelonLogger.Msg("May trigger FastPick! Type: " + LastInspectType.ToString());
			GearItemPreInspect.LastTriggerFrame = Time.frameCount;
		}
	}
}
