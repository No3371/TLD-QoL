using ModSettings;
using UnityEngine;

namespace QoL
{
	internal class BackHotkeySettings : JsonModSettings
	{
        [Name("Alternative Back/Cancel")]
        public KeyCode backKey = KeyCode.Mouse3;
        [Name("Alternative Interact Key")]
        public KeyCode interactKey = KeyCode.Mouse0;
        [Name("Whole Stack Transfer Modifier Key")]
        public KeyCode stackTransferKey = KeyCode.LeftShift;
        [Name("Inventory Item Drop Key")]
        public KeyCode dropKey = KeyCode.Q;

		[Name("Fast Pickup Holding Time: Min")]
		[Slider(0.25f, 0.6f)]
		public float fastPickMin = 0.3f;

		[Name("Fast Pickup Holding Time: Max")]
		[Slider(0.8f, 2f)]
		public float fastPickMax = 1f;
	}
	internal static class Settings
	{
		internal static BackHotkeySettings options;

		public static void OnLoad()
		{
			options = new BackHotkeySettings();
			options.AddToModSettings("QoL");
		}
	}
}
