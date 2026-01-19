
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(Panel_GearSelect), nameof(Panel_GearSelect.Update))]
internal class UIQoLGearSelect
{
	private static void Postfix(Panel_GearSelect __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.SelectGear();
			return;
		}
	}
}