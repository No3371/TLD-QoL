
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(InputManager), nameof(InputManager.GetInteractPressed))]
internal class AlternativeInteract
{
	private static bool Postfix(bool __result)
	{
		if (!__result && KeyboardUtilities.InputManager.GetKeyDown(Settings.options.interactKey))
		{
			__result = true;
		}

		return __result;
	}
}