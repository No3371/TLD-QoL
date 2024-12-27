
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(Panel_Repair), nameof(Panel_Repair.Update))]
internal class UIQoLRepair
{
	private static void Postfix(Panel_Repair __instance)
	{
		if (__instance.RepairInProgress()) return;

		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnRepair();
		}
	}
}
