
using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.UI;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

namespace QoL
{
    public class Implementation : MelonMod
	{
		public const string VERSION = "1.7.3";
        public override void OnInitializeMelon()
		{
			MelonLogger.Msg($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
		}

        public override void OnLateInitializeMelon()
        {
			var eapiType = Type.GetType("ExamineActionsAPI.ExamineActionsAPI, ExamineActionsAPI");
			if (eapiType != null)
			{
				EAPISupport.Instance = new EAPISupport(eapiType);
			}
        }
    }
}
