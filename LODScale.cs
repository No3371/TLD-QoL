
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
internal class GearItemLOD
{
	static void Postfix(GearItem __instance)
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
	static void Postfix(BodyHarvest __instance)
	{
		float lodScale = Settings.options.lodScale;
		if (Mathf.Approximately(lodScale, 1)) return;
		if (__instance.GetComponent<GearItem>() == null && __instance.TryGetComponent<LODGroup>(out var g))
		{
			g.size *= 1.5f;
		}
	}
}
