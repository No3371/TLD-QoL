
using HarmonyLib;
using Il2Cpp;

namespace QoL;

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.Update))]
internal class UIQoLCookWater
{
	private static void Postfix(Panel_CookWater __instance)
	{
		if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.interactKey))
		{
			if (__instance.SelectedFoodIsSnow())
				__instance.OnMeltSnow();
			else if (__instance.SelectedFoodIsWater())
				__instance.OnBoil();
		}
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnMeltSnowUp))]
internal class BulkIncreaseMeltUnits
{
	static int count = 0;
	private static void Postfix(Panel_CookWater __instance)
	{
		if (!Implementation.IM.GetKey(Settings.options.bulkKey)) return;
		if (count++ >= 4) count = 0;
		else
			__instance.OnMeltSnowUp();
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnMeltSnowDown))]
internal class BulkDecreaseMeltUnits
{
	static int count = 0;
	private static void Postfix(Panel_CookWater __instance)
	{
		if (!Implementation.IM.GetKey(Settings.options.bulkKey)) return;
		if (count++ >= 4) count = 0;
		else
			__instance.OnMeltSnowDown();
	}
}
[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnWaterUp))]
internal class BulkIncreaseCookingWaterUnits
{
	static int count = 0;
	private static void Postfix(Panel_CookWater __instance)
	{
		if (!Implementation.IM.GetKey(Settings.options.bulkKey)) return;
		if (count++ >= 4) count = 0;
		else
			__instance.OnWaterUp();
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnWaterDown))]
internal class BulkDecreaseCookingWaterUnits
{
	static int count = 0;
	private static void Postfix(Panel_CookWater __instance)
	{
		if (!Implementation.IM.GetKey(Settings.options.bulkKey)) return;
		if (count++ >= 4) count = 0;
		else
			__instance.OnWaterDown();
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnBoilUp))]
internal class BulkIncreaseBoilUnits
{
	static int count = 0;
	private static void Postfix(Panel_CookWater __instance)
	{
		if (!Implementation.IM.GetKey(Settings.options.bulkKey)) return;
		if (count++ >= 4) count = 0;
		else
			__instance.OnBoilUp();
	}
}

[HarmonyPatch(typeof(Panel_CookWater), nameof(Panel_CookWater.OnBoilDown))]
internal class BulkDecreaseBoilUnits
{
	static int count = 0;
	private static void Postfix(Panel_CookWater __instance)
	{
		if (!Implementation.IM.GetKey(Settings.options.bulkKey)) return;
		if (count++ >= 4) count = 0;
		else
			__instance.OnBoilDown();
	}
}