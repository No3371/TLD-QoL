
#if DEBUG
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace QoL
{
	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.ProcessPickupItemInteraction))]
	internal class LogPickupItemInteraction
	{
		static void Prefix (GearItem __0)
		{
			MelonLogger.Msg("Pre-ProcessPickupItemInteraction" + __0.name);
		}
		static void Postfix (GearItem __0)
		{
			MelonLogger.Msg("Post-ProcessPickupItemInteraction" + __0.name);
		}
	}
	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.ProcessPickupWithNoInspectScreen))]
	internal class LogPOProcessPickupWithNoInspectScreen
	{
		static void Prefix (GearItem __0)
		{
			MelonLogger.Msg("Pre-ProcessPickupWithNoInspectScreen" + __0.name);
		}
		static void Postfix (GearItem __0)
		{
			MelonLogger.Msg("Post-ProcessPickupWithNoInspectScreen" + __0.name);
		}
	}

	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.ProcessPickupWithNoInspectScreenDropCurrent))]
	internal class LogPOProcessPickupWithNoInspectScreenDropCurrent
	{
		static void Prefix ()
		{
			MelonLogger.Msg("Pre-ProcessPickupWithNoInspectScreenDropCurrent");
		}
		static void Postfix ()
		{
			MelonLogger.Msg("Post-ProcessPickupWithNoInspectScreenDropCurrent");
		}
	}

	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.ProcessPickupWithNoInspectScreenEquip))]
	internal class LogPOProcessPickupWithNoInspectScreenEquip
	{
		static void Prefix ()
		{
			MelonLogger.Msg("Pre-ProcessPickupWithNoInspectScreenEquip");
		}
		static void Postfix ()
		{
			MelonLogger.Msg("Post-ProcessPickupWithNoInspectScreenEquip");
		}
	}
}
#endif