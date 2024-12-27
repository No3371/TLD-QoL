
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QoL;

[HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Update))]
internal class UIQoLFeedFire
{
	private static void Postfix(Panel_FeedFire __instance)
	{
		if (Time.frameCount - UIQoLFeedFireMonitor.lastEntered < 30) return;
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnFeedFire();
			return;
		}
	}
}

[HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Enable))]
internal class UIQoLFeedFireMonitor
{
	internal static int lastEntered;
	private static void Postfix(ref Panel_FeedFire __instance)
	{
		lastEntered = Time.frameCount;
	}
}
