
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(Panel_FireStart), nameof(Panel_FireStart.Update))]
internal class UIQoLFireStart
{
	private static void Postfix(Panel_FireStart __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnStartFire();
			return;
		}
	}
}
