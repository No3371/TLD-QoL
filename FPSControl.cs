
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_OptionsMenu), nameof(Panel_OptionsMenu.ApplyVsyncAndFramerateCap))]
internal class FPSPatch
{
	private static void Postfix()
	{
		if (Settings.options.fpsLimit != 0)
		{
			Application.targetFrameRate = Settings.options.fpsLimit;
			MelonLogger.Msg($"[QoL] Applied fpsLimit: {Settings.options.fpsLimit} (ApplyVsyncAndFramerateCap)");
		}
	}
}

[HarmonyPatch(typeof(GameManager), "Awake", new Type[0])]
internal class FPSPatch2
{
	private static void Postfix()
	{
		if (Settings.options.fpsLimit != 0)
		{
			Application.targetFrameRate = Settings.options.fpsLimit;
			MelonLogger.Msg($"[QoL] Applied fpsLimit: {Settings.options.fpsLimit} (Awake)");
		}
	}
}
