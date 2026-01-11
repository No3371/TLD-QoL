
using HarmonyLib;

namespace QoL;

internal class EAPISupport
{
	internal static EAPISupport Instance { get; set; }

	public Type EapiType { get; }
	public EAPISupport(Type eapiType)
	{
		EapiType = eapiType;

		var methodInfo = AccessTools.FirstMethod(EapiType, mi => mi.Name == "PerformAction");
		PerformAction = (VoidDelegate)methodInfo.CreateDelegate(typeof(VoidDelegate), AccessTools.FirstProperty(EapiType, pi => pi.Name == "Instance").GetValue(null));
		methodInfo = AccessTools.FirstMethod(EapiType, mi => mi.Name == "OnNextSubAction");
		OnNextSubAction = (VoidDelegate)methodInfo.CreateDelegate(typeof(VoidDelegate), AccessTools.FirstProperty(EapiType, pi => pi.Name == "Instance").GetValue(null));
		methodInfo = AccessTools.FirstMethod(EapiType, mi => mi.Name == "OnPreviousSubAction");
		OnPreviousSubAction = (VoidDelegate)methodInfo.CreateDelegate(typeof(VoidDelegate), AccessTools.FirstProperty(EapiType, pi => pi.Name == "Instance").GetValue(null));
	}

	internal delegate void VoidDelegate();
	internal VoidDelegate PerformAction { get; }
	internal VoidDelegate OnNextSubAction { get; }
	internal VoidDelegate OnPreviousSubAction { get; }
}
