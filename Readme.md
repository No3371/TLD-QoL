# TLD-QoL

This is a QoL mod for The Long Dark.

## Dependencies

- [KeyboardUtilites by ds8678](https://github.com/ds5678/KeyboardUtilities)

## Note
The mod tries best to invoke the UI if possible, this is the safer way.
However some features may act weird for the first you trigger them per game session, because the mod tries to call the UI before it's ready.

**Known issues**
- First StackTransfer may force container UI to close.
- First QuickDrop in which the item stack is big enough to ask you how many to drop may force the inventory UI to close and lock the control. Press TAB (Check Status key) to reset.
- Probably the comined result of the above 2, using QuickDrop may open the last container once. Not sure exactly when to happen.

## Features

- **Alternative Back button**: Alternative to ESC. Default is Mouse3.
- **Alternative Interact button**: Alternative to LeftMouseButton.
    - Need to change the key, default is LMB, which is no effect if your Interact is default.
    - (My personal preference is F with Status moved to H, Put Away moved to Mouse4)
- **Stack Transfer**: Without operating the PickUnits UI, transfer a whole stack of items between the inventory and containers by holding the key.
    - Default is LeftShift.
    - ![](https://imgur.com/fpqn05t.gif)
- **Fast Pickup**: Directly "Take It" items by holding Interact for a bit then release.
    - Explanation: Instead of click a item to inspect then click to "Take It", press and hold for 0.3~1 seconds to perform "Take It".
    - The triggering duration can be configured.
- **QuickDrop**: Press a key to drop items from inventory.
    - Default is Q.
    - ![](https://imgur.com/B2W7jzl.gif)
    - If the stack is big enough to trigger the PickUnits UI, holding the StackTransfer Key will drop the whole stack.
- **QuickPlace**: Hold the Place Modifier when performing QuickDrop will directly start placing 1 of the stack into the world.
    - Default is LeftCtrl.
    - ![](https://imgur.com/YWmLUtC.gif)
- **BulkPickUnits**: Hold the StackTransfer Key to increase/decrease the units by 5.
    - ![](https://imgur.com/JCACe0R.gif)
- **GearSelectADScroll**: Use A/D or Scroll to switch selection in GearSelect panel (food to cook, container to boil water).
    - (Notice the scrolling without clicking)
    - ![](https://imgur.com/7O89m10.gif)
