using System.Collections;
using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.UI;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

namespace QoL
{
	public class Implementation : MelonMod
	{
		public const string VERSION = "1.6.6";
        public override void OnInitializeMelon()
		{
			MelonLogger.Msg($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();

		}

        public override void OnLateInitializeMelon()
        {
			var eapiType = Type.GetType("ExamineActionsAPI.ExamineActionsAPI, ExamineActionsAPI");
			if (eapiType != null)
			{
				EAPISupport.Instance = new EAPISupport(eapiType);
			}
        }
    }

	internal class EAPISupport
	{
		internal static EAPISupport Instance { get; set; }

        public Type EapiType { get; }
		public EAPISupport (Type eapiType)
		{
            EapiType = eapiType;

			
			var methodInfo = AccessTools.FirstMethod(EapiType, mi => mi.Name == "TryPerformSelectedAction");
			TryPerformAction = (EAPI_TryPerformActionDelegate) methodInfo.CreateDelegate(typeof(EAPI_TryPerformActionDelegate), AccessTools.FirstProperty(EapiType, pi => pi.Name == "Instance").GetValue(null));
        }

		internal delegate void EAPI_TryPerformActionDelegate ();
        internal EAPI_TryPerformActionDelegate TryPerformAction { get; }
	}
	[HarmonyPatch(typeof(GameManager), "Awake", new Type[0])]
	internal class SetFPS
	{
		private static void Postfix()
		{
			if (Settings.options.fpsLimit != 0)
			{
				Application.targetFrameRate = Settings.options.fpsLimit;
				MelonLogger.Msg($"[QoL] Applied fpsLimit: {Settings.options.fpsLimit}");
			}
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
			 && __instance.m_PassTimeButtonObject.active)
			{
				__instance.OnSelectRest();
				return;
			}
	
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
			 && __instance.m_SleepButton.active)
			{
				__instance.OnSelectPassTime();
				return;
			}

            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.OnDecreaseHours();
				if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
				{
					__instance.OnDecreaseHours();__instance.OnDecreaseHours();__instance.OnDecreaseHours();__instance.OnDecreaseHours();
				}
			}
            else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.OnIncreaseHours();
				if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
				{
					__instance.OnIncreaseHours();__instance.OnIncreaseHours();__instance.OnIncreaseHours();__instance.OnIncreaseHours();
				}
			}
		}
	}

	// [HarmonyPatch(typeof(Panel_FirstAid), nameof(Panel_FirstAid.Update))]
	// internal class FirstAidADScrollAndAlternativeFirstAid
	// {
	// 	private static void Postfix(ref Panel_FirstAid __instance)
	// 	{
	// 		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
	// 		{
	// 			if (__instance.m_SelectedFAKButton != null)
	// 				__instance.UseSelectedItem();
	// 			return;
	// 		}
    //         if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A))
	// 		{
	// 			__instance.SelectMainTreatment();
	// 		}
    //         else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D))
	// 		{
	// 			__instance.SelectAltTreatment();
	// 		}
	// 	}
	// }

	[HarmonyPatch(typeof(Panel_Affliction), nameof(Panel_Affliction.Update))]
	internal class AfflicationADScrollAndAlternativeTreatWound
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

	[HarmonyPatch(typeof(Panel_IceFishingHoleClear), nameof(Panel_IceFishingHoleClear.Update))]
	internal class IceBreakingADScrollAndAlternativeConfirmation
	{
		private static void Postfix(Panel_IceFishingHoleClear __instance)
		{
			if (__instance.IsClearingIce()) return;
			if (KeyboardUtilities.InputManager.GetKeyDown(Settings.options.interactKey))
			{
				__instance.OnBreakIce();
				return;
			}
            if (KeyboardUtilities.InputManager.GetKeyDown(KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			{
				__instance.PrevTool();
			}
            else if (KeyboardUtilities.InputManager.GetKeyDown(KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			{
				__instance.NextTool();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_IceFishing), nameof(Panel_IceFishing.Update))]
	internal class FishingHoursADScrollAndAlternativeStartFishing
	{
		private static void Postfix(Panel_IceFishing __instance)
		{
			if (__instance.IsFishing()) return;
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnFish();
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

	[HarmonyPatch(typeof(Panel_ActionPicker), nameof(Panel_ActionPicker.EnableWithCurrentList))]
	internal class AcitonPickerMonitor
	{
		private static void Postfix(Panel_ActionPicker __instance)
		{
			__instance.NextItem();
			__instance.PreviousItem();
		}
	}

	[HarmonyPatch(typeof(Panel_ActionPicker), nameof(Panel_ActionPicker.Update))]
	internal class AcitonPickerADAndConfirmation
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

	[HarmonyPatch(typeof(Panel_BodyHarvest), nameof(Panel_BodyHarvest.Update))]
	internal class BodyHarvestToolScrollAndAlternativeHarvest
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

	[HarmonyPatch(typeof(Panel_FireStart), nameof(Panel_FireStart.Update))]
	internal class AlternativeStartFire
	{
		private static void Postfix(Panel_FireStart __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnStartFire();
				return;
			}
		}
	}

	[HarmonyPatch(typeof(Panel_SnowShelterBuild), nameof(Panel_SnowShelterBuild.Update))]
	internal class AlternativeBuildShelter
	{
		private static void Postfix(Panel_SnowShelterBuild __instance)
		{
			if (__instance.IsBuilding()) return;
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnBuild();
				return;
			}
		}
	}

	[HarmonyPatch(typeof(Panel_SnowShelterInteract), nameof(Panel_SnowShelterInteract.Update))]
	internal class AlternativeShelterInteract
	{
		private static void Postfix(Panel_SnowShelterInteract __instance)
		{
			if (__instance.IsDismantling() || __instance.IsRepairing()) return;
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				switch (__instance.m_SelectedButtonIndex)
				{
					case 0:
						if (!__instance.m_SnowShelter.IsRuined()) __instance.OnUse();
						break;
					case 1:
						if (__instance.m_SnowShelter.IsRuined()) __instance.OnRepair();
						break;
					case 2:
						if (__instance.m_SnowShelter.m_AllowDismantle) __instance.OnDismantle();
						break;

				}
				return;
			}
		}
	}

	[HarmonyPatch(typeof(Panel_BreakDown), nameof(Panel_BreakDown.Update))]
	internal class BreakDownToolADScrollAndAlternativeBreakDown
	{
		private static void Postfix(ref Panel_BreakDown __instance)
		{
			if (__instance.IsBreakingDown()) return;

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

	[HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Enable))]
	internal class FeedFireUITracker
	{
		internal static int lastEntered;
		private static void Postfix(ref Panel_FeedFire __instance)
		{
			lastEntered = Time.frameCount;
		}
	}

	[HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Update))]
	internal class AlternativeFeedFire
	{
		private static void Postfix(Panel_FeedFire __instance)
		{
			if (Time.frameCount - FeedFireUITracker.lastEntered < 30) return;
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnFeedFire();
				return;
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.Update))]
	internal class AlternativeCook
	{
		private static void Postfix(Panel_Cooking __instance)
		{
			if (__instance.m_RecipePrepOperation.InProgress) return;
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnCook();
				return;
			}

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

	[HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.Update))]
	internal class AlternativeLightTorch
	{
		private static void Postfix(Panel_TorchLight __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnUseSelectedItem();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.Update))]
	internal class AlternativeCookWater
	{
		private static void Postfix(Panel_CookWater __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				if (__instance.SelectedFoodIsSnow())
					__instance.OnMeltSnow();
				else if (__instance.SelectedFoodIsWater())
					__instance.OnBoil();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_SprayPaint), nameof(Panel_SprayPaint.Update))]
	internal class SprayADScrollAndAlternativeConfirmSprayShape
	{
		private static void Postfix(Panel_SprayPaint __instance)
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnButtonConfirm();
				return;
			}
            // if (InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
			// {
			// 	__instance.OnButtonScrollLeft();
			// }
            // else if (InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
			// {
			// 	__instance.OnButtonScrollRight();
			// }
		}
	}

	[HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Update))]
	internal class InventoryPlus
	{
		private static void Postfix(Panel_Inventory __instance)
		{
			if (__instance.m_PickUnits.IsEnabled()) return;
			if (__instance.m_ItemDescriptionPage.m_ProgressBar.IsEnabled()) return;
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				if (__instance.m_ItemDescriptionPage.m_MouseButtonExamine.active && KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
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
				if (KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey)
				&& !KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
				{
					if (GameManager.GetSafehouseManager()?.IsCustomizing() == true)
						GameManager.GetSafehouseManager()?.StopCustomizing();
					var toDrop = gi.m_StackableItem?.DefaultUnitsInItem ?? 1;
					toDrop = Mathf.Clamp(toDrop, 0, gi?.m_StackableItem?.m_Units?? 1);
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
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
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

	// [HarmonyPatch(typeof(InventoryGridItem), nameof(InventoryGridItem.Update))]
	// internal class QuickInteractInventoryGrid
	// {
	// 	static int lastTriggerFrame;
	// 	private static void Postfix(InventoryGridItem __instance)
	// 	{
	// 		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey)
	// 		 && Time.frameCount - lastTriggerFrame > 12 && __instance.IsSelected())
	// 		{
	// 			if (KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
	// 			{
	// 				__instance.m_Inventory.GetPanel().OnExamine();
	// 			}
	// 			else __instance.DoubleClick();
	// 			lastTriggerFrame = Time.frameCount;
	// 		}
	// 	}
	// }

	[HarmonyPatch(typeof(Panel_Clothing), nameof(Panel_Clothing.Update))]
	internal class QuickInteractClothing
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

	[HarmonyPatch(typeof(Panel_Repair), nameof(Panel_Repair.Update))]
	internal class AlternativeRepairPanelRepair
	{
		private static void Postfix(Panel_Repair __instance)
		{
			if (__instance.RepairInProgress()) return;

			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnRepair();
			}
		}
	}

	[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.Update))]
	internal class ExaminePlus
	{
		private static void Postfix(Panel_Inventory_Examine __instance)
		{
			if (__instance.IsCleaning() || __instance.IsRepairing() || __instance.IsHarvesting() ||__instance.IsReading() || __instance.IsSharpening() || __instance.m_ActionInProgressWindow.active || InterfaceManager.GetPanel<Panel_GenericProgressBar>().isActiveAndEnabled)
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
					EAPISupport.Instance?.TryPerformAction();

				return;
			}
			
			if (__instance.m_ReadPanel.active)
			{
				if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A) || InputManager.GetScroll(InputManager.m_CurrentContext) > 0)
				{
					__instance.OnReadHoursDecrease();
					if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
					{
						__instance.OnReadHoursDecrease();__instance.OnReadHoursDecrease();__instance.OnReadHoursDecrease();__instance.OnReadHoursDecrease();
					}
				}
				else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D) || InputManager.GetScroll(InputManager.m_CurrentContext) < 0)
				{
					__instance.OnReadHoursIncrease();
					if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
					{
						__instance.OnReadHoursIncrease();__instance.OnReadHoursIncrease();__instance.OnReadHoursIncrease();__instance.OnReadHoursIncrease();
					}
				}
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
		private static void Postfix(Panel_Crafting __instance)
		{
			if (__instance.m_CraftingOperation?.InProgress ?? false) return;

			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__instance.OnBeginCrafting();
				return;
			}

			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.W)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				__instance.m_ScrollBehaviour.SetSelectedIndex(Mathf.Max(0, __instance.m_ScrollBehaviour.SelectedIndex - 4));
				return;
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.S)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
			{
				__instance.m_ScrollBehaviour.SetSelectedIndex(Mathf.Min(__instance.m_ScrollBehaviour.m_TotalItems - 1, __instance.m_ScrollBehaviour.SelectedIndex + 4));
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
					if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
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
					if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
					{
						CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();CraftingUILocator.UIComp.m_QuantitySelect.OnIncrease();
					}
				}
			}
		}
	}

	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnMeltSnowUp))]
	internal class BulkIncreaseMeltUnits
	{
		static int count = 0;
		private static void Postfix(Panel_CookWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnMeltSnowUp();
		}
	}

	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnMeltSnowDown))]
	internal class BulkDecreaseMeltUnits
	{
		static int count = 0;
		private static void Postfix(Panel_CookWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnMeltSnowDown();
		}
	}

	[HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.Update))]
	internal class BulkSelectFoodUp
	{
		static int count = 0;
		private static void Postfix(Panel_Cooking __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
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
	}

	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnWaterUp))]
	internal class BulkIncreaseCookingWaterUnits
	{
		static int count = 0;
		private static void Postfix(Panel_CookWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnWaterUp();
		}
	}

	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnWaterDown))]
	internal class BulkDecreaseCookingWaterUnits
	{
		static int count = 0;
		private static void Postfix(Panel_CookWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnWaterDown();
		}
	}

	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnBoilUp))]
	internal class BulkIncreaseBoilUnits
	{
		static int count = 0;
		private static void Postfix(Panel_CookWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnBoilUp();
		}
	}

	[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnBoilDown))]
	internal class BulkDecreaseBoilUnits
	{
		static int count = 0;
		private static void Postfix(Panel_CookWater __instance)
		{
			if (!KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)) return;
			if (count++ >= 4) count = 0;
			else 
				__instance.OnBoilDown();
		}
	}

	[HarmonyPatch(typeof(Panel_PickWater), nameof(Panel_PickWater.OnIncrease))]
	internal class BulkIncreaseWaterUnits
	{
		static int count = 0;
		private static void Postfix(Panel_PickWater __instance)
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
		private static void Postfix(Panel_PickWater __instance)
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

	// [HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.OnIncrease))]
	// internal class BulkIncreaseUnits
	// {
	// 	static int count = 0;
	// 	static bool Prefix(Panel_PickUnits __instance)
	// 	{
	// 		// MelonLogger.Msg(__instance.m_numUnits + "/" + __instance.m_maxUnits);
    //         // if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
	// 		// {
	// 		// 	__instance.m_numUnits += 4;
	// 		// 	__instance.m_numUnits = Math.Min(__instance.m_maxUnits, __instance.m_numUnits);
	// 		// }
	// 		// This doesn't really work, so let's go rude.
	// 		MelonLogger.Msg($"Panel_PickUnits.OnIncrease ({count})");
	// 		if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
	// 		{
	// 			if (count++ >= 4) count = 0;
	// 			else __instance.OnIncrease();
	// 		}

	// 		return true;
	// 	}
	// }

	// [HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.OnDecrease))]
	// internal class BulkDecreaseUnits
	// {
	// 	static int count = 0;
	// 	static bool Prefix(Panel_PickUnits __instance)
	// 	{
	// 		// MelonLogger.Msg(__instance.m_numUnits + "/" + __instance.m_maxUnits);
    //         // if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
	// 		// {
	// 		// 	__instance.m_numUnits -= 4;
	// 		// 	__instance.m_numUnits = Math.Max(0, __instance.m_numUnits);
	// 		// }
	// 		// This doesn't really work, so let's go rude.
	// 		if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
	// 		{
	// 			if (count++ >= 4) count = 0;
	// 			else __instance.OnDecrease();
	// 		}

	// 		return true;
	// 	}
	// }

	// [HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.Enable))]
	// internal class PickUnitsEnable
	// {
	// 	static void Postfix (Panel_PickUnits __instance, bool enable)
	// 	{
	// 		MelonLogger.Msg($"Panel_PickUnits.Enable ({enable})");
	// 	}
	// }

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.Update))]
	internal class PickUnitsUpdate
	{
		internal static int lastOpened, lastOn, lastExecuted;
		static void Postfix (Panel_PickUnits __instance)
		{
			if (lastOn != Time.frameCount - 1) // Updated continuously when opened
				lastOpened = Time.frameCount;
			lastOn = Time.frameCount;
			
			if (lastOpened == Time.frameCount)
			{
				if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey))
				{
					lastExecuted = Time.frameCount;
					__instance.OnExecuteAll();
					return;
				}
			}

			if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
			 && InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.A))
			{
				// __instance.m_numUnits = Mathf.Max(0, __instance.m_numUnits - 4);
				__instance.OnDecrease();
				__instance.OnDecrease();
				__instance.OnDecrease();
				__instance.OnDecrease();
				return;
			}
			else if (KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
			 && InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.D))
			{
				// __instance.m_numUnits = Mathf.Min(__instance.m_maxUnits, __instance.m_numUnits + 4);
				__instance.OnIncrease();
				__instance.OnIncrease();
				__instance.OnIncrease();
				__instance.OnIncrease();
				return;
			}

			// if (lastOpened <= lastExecuted) return;
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.interactKey)
			 && KeyboardUtilities.InputManager.GetKey(Settings.options.bulkKey)
			 && !KeyboardUtilities.InputManager.GetKey(Settings.options.modifierKey))
			{
				if (Time.frameCount - lastOpened <= 1) return;
				__instance.OnExecuteAll();
				lastExecuted = lastOpened;
				return;
			}
			else if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				if (Time.frameCount - lastOpened <= 1) return;
				__instance.OnExecute();
				lastExecuted = lastOpened;
				return;
			}
		}
	}

	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
	internal class FastPickAndAlternativeTakeIt
	{
		public static int LastTriggerFrameConsumed { get; private set; }
		public static float LastFastPickTriggered { get; private set; }
		static void Postfix (PlayerManager __instance)
		{
			GearItem? gearItem = __instance?.GearItemBeingInspected();
			if (gearItem == null || !__instance.IsInspectModeActive()
			 || GearItemPreInspect.LastTriggerFrame <= LastTriggerFrameConsumed
			 || GearItemPreInspect.LastInspectType != GearItemPreInspect.InspectType.None) return; // Only active for items in world

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

	[HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
	internal class GearItemLOD
	{
		static void Postfix (GearItem __instance)
		{
			float lodScale = Settings.options.lodScale;
			if (Mathf.Approximately(lodScale, 1)) return;
			if (__instance.TryGetComponent<LODGroup>(out var g))
            {
				var delta = 0.5f - g.size;
				if (delta > 0) g.size += delta * 0.5f;
                g.size *= lodScale;
            }
        }
	}

	[HarmonyPatch(typeof(BodyHarvest), nameof(BodyHarvest.Deserialize))]
	internal class CarcassLOD
	{
		static void Postfix (BodyHarvest __instance)
		{
			float lodScale = Settings.options.lodScale;
			if (Mathf.Approximately(lodScale, 1)) return;
			if (__instance.GetComponent<GearItem>() == null && __instance.TryGetComponent<LODGroup>(out var g))
            {
                g.size *= 1.5f;
            }
        }
	}

	[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.OnDone))]
	internal class TrashCanWarning
	{
		static void Postfix (Panel_Container __instance)
		{
			if (Settings.options.voidTrashCan
			 && __instance.m_Container.name == "CONTAINER_TrashCanister"
			 && !__instance.m_Container.IsEmpty())
			{
				MelonCoroutines.Start(DelayedWarning());
			}
        }

		static IEnumerator DelayedWarning ()
		{
			yield return new WaitForSeconds(0.5f);
			InterfaceManager.GetPanel<Panel_HUD>().DisplayWarningMessage("VOID TRASH CAN");
			yield return new WaitForSeconds(4f);
			InterfaceManager.GetPanel<Panel_HUD>().ClearWarningMessage();
		}

	}

	[HarmonyPatch(typeof(Container), nameof(Container.UpdateContainer))]
	internal class TrashCan
	{
		static Il2CppSystem.Collections.Generic.List<GearItem> cache = new Il2CppSystem.Collections.Generic.List<GearItem>();
		static void Postfix (Container __instance)
		{
			if (!__instance.m_StartHasBeenCalled || !Settings.options.voidTrashCan) return;
			if (!__instance.IsInspected() || __instance.name != "CONTAINER_TrashCanister" || __instance.IsEmpty()) return;

			cache.Clear();
			__instance.GetItems(cache);
			foreach (var i in cache)
			{
				i.Degrade(i.GearItemData.MaxHP * 0.000005f);
			}
        }
	}

	[HarmonyPatch(typeof(Container), nameof(Container.BeginContainerOpen))]
	internal static class PatchFasterSearchUpdated
	{
		static void Prefix(Container __instance)
		{
			if (!Settings.options.containterTimeTweak) return;
			ContainerInteraction interaction = __instance.GetComponent<ContainerInteraction>();
			if (interaction == null) return;

			var defaultTime = interaction.HoldTime;
			if (__instance.IsInspected())
				interaction.HoldTime *= Settings.options.containterOpenTimeScale;
			else
			{
				interaction.HoldTime = Mathf.Clamp(
					defaultTime * Settings.options.containterSearchTimeScalePerItem * __instance.m_GearToInstantiate.Count,
					defaultTime * Settings.options.containterSearchTimeScaleMin,
					defaultTime * Settings.options.containterSearchTimeScaleMax
				);

				// Use a user-defined amount of variance to decrease or increase hold time by a random amount
				// This is done after the fact so that containers with 0 items (common) won't constantly use TimeScaleMin
				var rand = new System.Random();
				interaction.HoldTime *= 1.0f + (rand.Next(-100, 100) * Settings.options.containterSearchTimeScaleVar / 100f);
			}
        }
	}
}
