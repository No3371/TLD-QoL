using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace QoL
{
	public class Implementation : MelonMod
	{
        public override void OnApplicationStart()
		{
			MelonLogger.Msg($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
		}

		public static void MaybeGoBack ()
        {
            // if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.backKey))
            // {
            //     Panel_Crafting craftingPanel = InterfaceManager.GetPanel<Panel_Crafting>();
            //     if (craftingPanel == null || uConsole.IsOn()) return;
            //     if (craftingPanel.IsEnabled() && !craftingPanel.m_CraftingInProgress)
            //     {
            //         craftingPanel.OnBackButton();
            //         return;
            //     }
            //     else
            //     {
            //         Panel_FirstAid firstAid = InterfaceManager.GetPanel<Panel_FirstAid>();
            //         if (firstAid != null && firstAid.IsEnabled())
            //         {
            //             firstAid.OnBack();
            //             return;
            //         }
            //         Panel_Clothing clothing = InterfaceManager.GetPanel<Panel_Clothing>();
            //         if (clothing != null && clothing.IsEnabled())
            //         {
            //             clothing.OnCancel();
            //             return;
            //         }
            //         Panel_Inventory inventory = InterfaceManager.GetPanel<Panel_Inventory>();
            //         if (inventory != null && inventory.IsEnabled())
            //         {
            //             inventory.OnBack();
            //             return;
            //         }
            //         Panel_Log journal = InterfaceManager.GetPanel<Panel_Log>();
            //         if (journal != null && journal.IsEnabled() && journal.m_ReadyForInput)
            //         {
            //             journal.OnBack();
            //             return;
            //         }
            //         Panel_Map map = InterfaceManager.GetPanel<Panel_Map>();
            //         if (map != null && (map.IsEnabled() || !InterfaceManager.IsOverlayActiveImmediate()))
            //         {
            //             map.OnCancel();
            //             return;
            //         }
			// 		Panel_Harvest harvest = InterfaceManager.GetPanel<Panel_Harvest>();
			// 		if (harvest != null && harvest.IsEnabled())
			// 		{
			// 			harvest.OnCancel();
			// 			return;
			// 		}
			// 		Panel_Rest rest = InterfaceManager.GetPanel<Panel_Rest>();
			// 		if (rest != null && rest.IsEnabled())
			// 		{
			// 			rest.OnCancel();
			// 			return;
			// 		}
			// 		Panel_Container container = InterfaceManager.GetPanel<Panel_Container>();
			// 		if (container != null && container.IsEnabled())
			// 		{
			// 			container.OnDone();
			// 			return;
			// 		}
            //     }
            // }
        }
	}

	[HarmonyLib.HarmonyPatch(typeof(InputManager), nameof(InputManager.GetInteractPressed))]
	internal class AlternativeInteract
	{
		private static bool Postfix(bool __result)
		{
            if (!__result && InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
			{
				__result = true;
			}

			return __result;
		}
	}

	[HarmonyLib.HarmonyPatch(typeof(InputManager), nameof(InputManager.GetPutBackPressed))]
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

	[HarmonyLib.HarmonyPatch(typeof(InputManager), nameof(InputManager.GetEscapePressed))]
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

	
	[HarmonyLib.HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForTransferToInventory))]
	internal class CToI
	{
		static void Postfix (ref Panel_PickUnits __instance) {
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
			{
				__instance.OnExecuteAll();
			}

		}
	}

	[HarmonyLib.HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForTransferToContainer))]
	internal class IToC
	{
		static void Postfix (ref Panel_PickUnits __instance) {
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
			{
				__instance.OnExecuteAll();
			}

		}
	}

	// [HarmonyLib.HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
	// internal class InPick
	// {
		
	// 	private static bool Postfix()
	// 	{
	// 		if (KeyboardUtilities.InputManager.GetMouseButtonUp(0))
	// 			if (Time.unscaledTime - PrePick.pickingAt > 0.35f)
	// 			{
	// 				Debug.Log("BINGO");
	// 				PlayerManager.
	// 				GameObject objectUnderCrosshair = __instance.GetInteractiveObjectUnderCrosshairs(20f);
	// 				GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(objectUnderCrosshair.GetComponent<GearItem>(), false, false, false);
	// 					return false;
	// 			}
	// 	}
	// }

	[HarmonyLib.HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
	internal class TryPick
	{
		static void Postfix (ref PlayerManager __instance)
		{
				// MelonLogger.Msg("Mouse up (" + (Time.unscaledTime - Inspect.pickingAt));
			if (KeyboardUtilities.InputManager.GetMouseButtonUp(0) || KeyboardUtilities.InputManager.GetKeyUp(Settings.options.interactKey))
			{
				// MelonLogger.Msg("Mouse up (" + (Time.unscaledTime - Inspect.pickingAt));
				if (Time.unscaledTime - Inspect.pickingAt > Settings.options.fastPickMin && Time.unscaledTime - Inspect.pickingAt < Settings.options.fastPickMax)
				{
                    GearItem gearItem = __instance.GearItemBeingInspected();
                    // MelonLogger.Msg("Fast picking up " + gearItem.name + "( " + (Time.unscaledTime - Inspect.pickingAt));
					__instance.ProcessPickupItemInteraction(gearItem, false, false, false);
					__instance.ExitInspectGearMode(false);
				}
			}
		}
	}

	[HarmonyLib.HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.EnterInspectGearMode), new Type[] { typeof(GearItem), typeof(Container), typeof(IceFishingHole), typeof(Harvestable), typeof(CookingPotItem) } )]
	internal class Inspect
	{
		public static float pickingAt;
		static void Postfix()
		{
			Inspect.pickingAt = Time.unscaledTime;
		}
	}

	// [HarmonyLib.HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateActiveInteraction))]
	// internal class TryPick
	// {
	// 	static void PostFix (ref PlayerManager __instance)
	// 	{
	// 		if (KeyboardUtilities.InputManager.GetKeyUp(KeyCode.Mouse0))
	// 			if (Time.unscaledTime - PrePick.pickingAt > 0.35f)
	// 			{
	// 				Debug.Log("BINGO");
	// 				if (__instance.ActiveInteraction != null && __instance.GearItemBeingInspected() != null)
	// 				{
	// 					__instance.ProcessPickupItemInteraction(__instance.GearItemBeingInspected(), false, false, false);
	// 					__instance.ExitInspectGearMode(false);
	// 				}
					
	// 			}
	// 	}
	// // }

	// static class Inspect
	// {
	// 	public static float pickingAt;
	// }

	// [HarmonyLib.HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.EnterInspectGearMode), new Type[] { typeof(GearItem) })]
	// internal class InspectEIGM
	// {
	// 	static void Postfix()
	// 	{
	// 		MelonLogger.Msg("EIGM Inspecting...(");
	// 		Inspect.pickingAt = Time.unscaledTime;
	// 	}
	// }

	// [HarmonyLib.HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.EnterInspectGearModeWithCallback))]
	// internal class InspectEIGMWC
	// {
	// 	static void Postfix()
	// 	{
	// 		MelonLogger.Msg("EIGMWC Inspecting...(");
	// 		Inspect.pickingAt = Time.unscaledTime;
	// 	}
	// }

	// // [HarmonyLib.HarmonyPatch(typeof(InspectGearController), nameof(InspectGearController.SetGearItem))]
	// // internal class GI
	// // {
	// // 	static void Postfix(GearItem gi)
	// // 	{
	// // 		MelonLogger.Msg("Inspect Gear...( " + gi.name);
	// // 	}
	// // }

	// [HarmonyLib.HarmonyPatch(typeof(GearItem), nameof(GearItem.PerformInteraction))]
	// internal class Inspect
	// {
	// 	public static float pickingAt;
	// 	static void Postfix()
	// 	{
	// 		Inspect.pickingAt = Time.unscaledTime;
	// 	}
	// }
}
