
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(InputManager), nameof(InputManager.GetPickupPressed))]
internal class AlternativePickup
{
	private static bool Postfix(bool __result)
	{
		if (!__result && InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__result = true;
		}

		return __result;
	}
}