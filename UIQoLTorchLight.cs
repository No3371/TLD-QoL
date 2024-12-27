
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.Update))]
internal class UIQoLTorchLight
{
	private static void Postfix(Panel_TorchLight __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			__instance.OnUseSelectedItem();
		}
	}
}
