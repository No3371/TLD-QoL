
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(Panel_SnowShelterBuild), nameof(Panel_SnowShelterBuild.Update))]
internal class UIQoLSnowShelterBuild
{
	private static void Postfix(Panel_SnowShelterBuild __instance)
	{
		if (__instance.IsBuilding()) return;
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnBuild();
			return;
		}
	}
}


[HarmonyPatch(typeof(Panel_SnowShelterInteract), nameof(Panel_SnowShelterInteract.Update))]
internal class UIQoLSnowShelterInteract
{
	private static void Postfix(Panel_SnowShelterInteract __instance)
	{
		if (__instance.IsDismantling() || __instance.IsRepairing()) return;
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			switch (__instance.m_SelectedButtonIndex)
			{
				case 0:
					if (!__instance.m_SnowShelter.IsRuined()) __instance.OnUse();
					break;
				case 1:
					if (__instance.m_SnowShelter.IsRuined()) __instance.OnRepair();
					break;
				case 2:
					if (__instance.m_SnowShelter.m_AllowDismantle) __instance.OnDismantle();
					break;

			}
			return;
		}
	}
}