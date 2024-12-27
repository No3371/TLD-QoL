
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