
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(Panel_SprayPaint), nameof(Panel_SprayPaint.Update))]
internal class UIQoLSprayPaint
{
	private static void Postfix(Panel_SprayPaint __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnButtonConfirm();
			return;
		}
	}
}
