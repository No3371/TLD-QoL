
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace QoL;



[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.OnDone))]
internal class TrashCanWarning
{
	static void Postfix(Panel_Container __instance)
	{
		if (Settings.options.voidTrashCan
		 && __instance.m_Container.name.Contains("CONTAINER_TrashCanister")
		 && !__instance.m_Container.IsEmpty())
		{
			MelonCoroutines.Start(DelayedWarning());
		}
	}

	static System.Collections.IEnumerator DelayedWarning()
	{
		yield return new WaitForSeconds(0.5f);
		InterfaceManager.GetPanel<Panel_HUD>().DisplayWarningMessage("VOID TRASH CAN");
		yield return new WaitForSeconds(4f);
		InterfaceManager.GetPanel<Panel_HUD>().ClearWarningMessage();
	}

}

[HarmonyPatch(typeof(Container), nameof(Container.UpdateContainer))]
internal class TrashCan
{
	static Il2CppSystem.Collections.Generic.List<GearItem> cache = new Il2CppSystem.Collections.Generic.List<GearItem>();
	static void Postfix(Container __instance)
	{
		if (!__instance.m_StartHasBeenCalled || !Settings.options.voidTrashCan) return;
		if (!__instance.IsInspected() || !__instance.name.Contains("CONTAINER_TrashCanister") || __instance.IsEmpty()) return;

		cache.Clear();
		__instance.GetItems(cache);
		foreach (var i in cache)
		{
			i.Degrade(i.GearItemData.MaxHP * 0.000005f);
		}
	}
}
