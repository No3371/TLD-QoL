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
