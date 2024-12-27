
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Container), nameof(Container.BeginContainerOpen))]
internal static class DynamicSearchTime
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
