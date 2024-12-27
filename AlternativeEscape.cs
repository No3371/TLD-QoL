
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(InputManager), nameof(InputManager.GetEscapePressed))]
internal class AlternativeEscape
{
	private static bool Postfix(bool __result)
	{
		if (!__result && InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.backKey))
		{
			__result = true;
		}

		return __result;
	}
}