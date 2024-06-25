using ModSettings;
using UnityEngine;

namespace QoL
{
	internal class QoLSettings : JsonModSettings
	{
        [Name("Alternative Back/Cancel")]
		[Description("Work as ESC in most scenarios.")]
        public KeyCode backKey = KeyCode.Mouse3;
        [Name("Alternative Interact Key")]
		[Description("Work as Interact(the default LMB) in most scenarios, and can act as confirm button in a lot of UIs.")]
        public KeyCode interactKey = KeyCode.Insert;
        [Name("Bulk Key")]
		[Description("Hold this when transfering item between the inventory and containers to transfer the whole stack. Also works for QuickDrop.")]
        public KeyCode bulkKey = KeyCode.LeftShift;
        [Name("Drop Key")]
		[Description("Drop currently selected item in inventory (With ModiferKey: drop a whole stack")]
        public KeyCode dropKey = KeyCode.Q;
        [Name("Modifier Key")]
		[Description("Hold this to modify other features, check the mod's readme.")]
        public KeyCode modifierKey = KeyCode.LeftControl;

		[Name("Fast Pickup Holding Time: Min")]
		[Description("Hold Interact at least this long to perform FastPick")]
		[Slider(0.25f, 0.6f)]
		public float fastPickMin = 0.3f;

		[Name("Fast Pickup Holding Time: Max")]
		[Description("FastPick won't be triggered if you hold Interact for longer than this")]
		[Slider(0.8f, 2f)]
		public float fastPickMax = 1f;

		[Name("Stop Reseting Alternative Interact Key")]
		[Description("The default is LMB before 1.3.2 which will cause issues")]

		public bool stopResetAlternativeInteract;

		[Name("GearItem LOD Size Scale")]
		[Description("Adjust the distance an item becomes visible, could impact graphic performance.")]
		[Slider(1f, 3f)]
		public float lodScale = 1f;
		[Name("Void Trash Can")]
		[Description("Enable fast degrading for items in trash cans. Also destroys ruined items in them. Not very immersive, but help when you are flooded with unwanted items.")]
		public bool voidTrashCan = false;

		[Name("Container Time Tweaks")]
		[Description("Toggle the container open/search time tweaks.")]
		public bool containterTimeTweak = false;

		[Name("Container Open Time Scale")]
		[Description("Shortening the time to open searched containers.")]
		public float containterOpenTimeScale = 0.5f;

		[Name("Container Search Time Scale Per Item")]
		[Description("Shortening the time to search containers. For example, by default a container with 3 items will cost 60% of the default time to search.")]
		public float containterSearchTimeScalePerItem = 0.2f;

		[Name("Container Search Time Min")]
		[Description("The minimum search time scale.")]
		[Slider(0.1f, 1f)]
		public float containterSearchTimeScaleMin = 0.1f;

		[Name("Container Search Time Max")]
		[Description("The maximum search time scale.")]
		[Slider(0.4f, 3f)]
		public float containterSearchTimeScaleMax = 1f;

		[Name("Container Search Time Variance")]
		[Description("Random variance in search time scale.")]
		[Slider(0.0f, 1f)]
		public float containterSearchTimeScaleVar = 0.2f;
	}
	internal static class Settings
	{
		internal static QoLSettings options;

		public static void OnLoad()
		{
			options = new QoLSettings();
			options.AddToModSettings("QoL");
		}
	}
}
