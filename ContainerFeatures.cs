
using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.IntBackedUnit;
using UnityEngine;

namespace QoL;


[HarmonyPatch(typeof(Container), nameof(Container.Awake))]
internal class ContainerWeightLimitPatch
{
    private static void Postfix(Container __instance)
    {
        var kg = (__instance.m_Capacity * Settings.options.containerWeightLimitScale).ToQuantity(1f);
        kg = (float)System.Math.Round(kg, 2);
        __instance.m_Capacity = ItemWeight.FromKilograms(kg);
    }
}

[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.OnInventoryToContainer))]
internal class ContainerSlotLimitPatch
{
    private static bool Prefix(Panel_Container __instance)
    {
        if (Settings.options.containerSlotLimitRatio >= 0.1f && __instance.m_Container != null)
        {
            Container container = __instance.m_Container;
            var slotLimit = Mathf.FloorToInt(container.m_Capacity.ToQuantity(1f) * Settings.options.containerSlotLimitRatio);
            if (container.m_Items.Count >= slotLimit)
            {
                GameAudioManager.PlayGUIError();
                return false;
            }
        }
        return true;
    }
}
[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.RefreshContainerTable))]
internal class ContainerLabelPatch
{
    private static void Postfix(Panel_Container __instance)
    {
        if (Settings.options.containerSlotLimitRatio < 0.1f
         || __instance.m_Container == null) return;

        Container container = __instance.m_Container;
        var slotLimit = Mathf.FloorToInt(container.m_Capacity.ToQuantity(1f) * Settings.options.containerSlotLimitRatio);
        __instance.m_Label_ContainerCapacity.text += $" ({__instance.m_Container.m_Items.Count}/{slotLimit})";
    }
}