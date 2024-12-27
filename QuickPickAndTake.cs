
using HarmonyLib;
using Il2Cpp;
using UnityEngine;


[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
internal class FastPickAndAlternativeTakeIt
{
    public static int LastTriggerFrameConsumed { get; private set; }
    public static float LastFastPickTriggered { get; private set; }
    static void Postfix(PlayerManager __instance)
    {
        GearItem? gearItem = __instance?.GearItemBeingInspected();
        if (gearItem == null || !__instance.IsInspectModeActive()
         || GearItemPreInspect.LastTriggerFrame <= LastTriggerFrameConsumed
         || GearItemPreInspect.LastInspectType != GearItemPreInspect.InspectType.None) return; // Only active for items in world

        float sinceLastInspect = Time.unscaledTime - GearItemPreInspect.LastInspect;
        bool trigger = false;

        if (!trigger)
            trigger = (InputManager.GetFireReleased(InputManager.m_CurrentContext) || KeyboardUtilities.InputManager.GetKeyUp(QoL.Settings.options.interactKey))
                   && (sinceLastInspect > QoL.Settings.options.fastPickMin && sinceLastInspect < QoL.Settings.options.fastPickMax);

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

[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.EnterInspectGearMode), new Type[] { typeof(GearItem), typeof(Container), typeof(IceFishingHole), typeof(Harvestable), typeof(CookingPotItem) })]
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
        if (!KeyboardUtilities.InputManager.GetKey(QoL.Settings.options.interactKey)
         && !InputManager.GetFirePressed(InputManager.m_CurrentContext)) return;

        // MelonLogger.Msg("May trigger FastPick! Type: " + LastInspectType.ToString());
        GearItemPreInspect.LastTriggerFrame = Time.frameCount;
    }
}