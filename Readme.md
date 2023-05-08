# TLD-QoL

This is a QoL mod for The Long Dark.

## Dependencies

- [KeyboardUtilites by ds8678](https://github.com/ds5678/KeyboardUtilities)

## Features

- **Alternative Back button**: Alternative to ESC. Default is Mouse3.
- **Alternative Interact button**: Alternative to LeftMouseButton.
    - Need to change the key, default is LMB, which is no effect if your Interact is default.
    - (My personal preference is F with Status moved to H, Put Away moved to Mouse4)
- **Stack Transfer**: Without operating the PickUnits UI, transfer a whole stack of items between the inventory and containers by holding the key.
    - Default is LeftShift.
    - **if you haven't trasfered anything after launching the game, inventory will be forced to close for the first time. No problems after that.**
    - Note: Somehow the first usage after launch will unintentionally close the interface.
- **Fast Pickup**: Directly "Take It" items by holding Interact for a bit then release.
    - Explanation: Instead of click a item to inspect then click to "Take It", press and hold for 0.3~1 seconds to perform "Take It".
    - The triggering duration can be configured.
- **QuickDrop**: Press a key to drop items from inventory.
    - Default is Q.
    - If the stack is big enough to trigger the PickUnits UI, holding the StackTransfer Key will drop the whole stack.
    - **if you haven't dropped anything after launching the game, the control will lock for the first time, you need to press TAB to reset. No problems after that.**
- **QuickPlace**: Hold the Place Modifier when performing QuickDrop will directly start placing 1 of the stack into the world.
    - Default is LeftCtrl.
- **BulkPickUnits**: Hold the StackTransfer Key to increase/decrease the units by 5.
- **GearSelectADScroll**: Use A/D or Scroll to switch selection in GearSelect panel (food to cook, container to boil water).