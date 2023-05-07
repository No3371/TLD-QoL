using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace QoL
{
	public class Implementation : MelonMod
	{
        public override void OnInitializeMelon()
		{
			MelonLogger.Msg($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
		}
	}

	[HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Update))]
	internal class QuickDrop
	{
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
			if (count++ == 4) count = 0;
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
			if (count++ == 4) count = 0;
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

	
	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForTransferToInventory))]
	internal class PickUnitsToInventory
	{
		static void Postfix (ref Panel_PickUnits __instance) {
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
			{
				__instance.OnExecuteAll();
			}

		}
	}

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForTransferToContainer))]
	internal class PickUnitsToContainer
	{
		static void Postfix (ref Panel_PickUnits __instance) {
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
			{
				__instance.OnExecuteAll();
			}

		}
	}

	[HarmonyPatch(typeof(Panel_PickUnits), nameof(Panel_PickUnits.SetGearForDrop))]
	internal class PickUnitsToDrop
	{
		static void Postfix (ref Panel_PickUnits __instance) {
			if (KeyboardUtilities.InputManager.GetKey(Settings.options.stackTransferKey))
			{
				__instance.OnExecuteAll();
			}

		}
	}

	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
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

	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.EnterInspectGearMode), new Type[] { typeof(GearItem), typeof(Container), typeof(IceFishingHole), typeof(Harvestable), typeof(CookingPotItem) } )]
	internal class Inspect
	{
		public static float pickingAt;
		static void Postfix()
		{
			Inspect.pickingAt = Time.unscaledTime;
		}
	}

}
