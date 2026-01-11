using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppTLD.UI;
using MelonLoader;
using MonoMod.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace QoL
{
    public class Implementation : MelonMod
	{
		internal static LegacyInput IM { get; private set; }
		public const string VERSION = "1.7.5";
        public override void OnInitializeMelon()
		{
			MelonLogger.Msg($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
		}

        public override void OnLateInitializeMelon()
        {
			IM = new LegacyInput();
			var eapiType = Type.GetType("ExamineActionsAPI.ExamineActionsAPI, ExamineActionsAPI");
			if (eapiType != null)
			{
				EAPISupport.Instance = new EAPISupport(eapiType);
			}
        }
    }
}