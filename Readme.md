# TLD-QoL

This is a QoL mod for The Long Dark.

## Dependencies

- [KeyboardUtilites by ds8678](https://github.com/ds5678/KeyboardUtilities)

## Note
The mod tries best to invoke the UI if possible, this is the safer way.  
However some features may act weird for the first time you trigger them every game session, because the mod tries to use the UI's function before it's ready.

**Known issues**
- First Stack Transfer may force container UI to close.
- First QuickDrop in which the item stack is big enough to ask you how many to drop may force the inventory UI to close and lock the control. Press TAB (Check Status key) to reset.
- Probably the comined result of the above 2, using QuickDrop/StackTransfer may bring you to previous container once. Not sure exactly when to happen.

## Features

### Keys
- **Alternative Back button**: Alternative to ESC. Default is Mouse3.
- **Alternative Interact button**: Alternative to LeftMouseButton.
    - Default is LMB, which is no effect if your Interact is default.
    - (My personal preference is F with Status moved to H, Put Away moved to Mouse4, HIGHLY recommended)

- **Stack Transfer**: Skip the PickUnits UI, transfer a whole stack of items between the inventory and containers by holding the **Bulk** key.
    - Default is LeftShift.
    - ![](https://imgur.com/fpqn05t.gif)
- **QuickDrop**: Press the **Drop** key (Default:Q) to drop items from inventory.
    - ![](https://imgur.com/B2W7jzl.gif)
    - If the stack is big enough to trigger the PickUnits UI, holding the Bulk Key will drop the whole stack.
- **QuickPlace**: Hold the **Modifier** key when performing QuickDrop will directly start placing 1 of the stack into the world.
    - Default is LeftCtrl.
    - ![](https://imgur.com/YWmLUtC.gif)
- **Fast Pickup**: Directly pick up items by holding Interact for a bit (Default: 0.3 ~ 1 seconds) then release.
    - The triggering duration can be configured.
- **BulkPickUnits**: Hold the **Bulk** Key to increase/decrease the units by 5.
    - ![](https://imgur.com/JCACe0R.gif)
- **A/D & Scroll Selection**
    - ![](https://imgur.com/7O89m10.gif)
    - When selecting units to transfer.
        - Bulk key
    - When picking food to cook
    - When picking container to cook
    - When selecting clothing to swap **(Scroll Only)**
    - When selecting first aid item
    - When selecting affliction to tend
    - When selecting tool to use to harvest carcass **(Scroll Only)**
    - When selecting tool to use to break down objects
    - When selecting spray shape **(Added Scroll)**
    - When selecting hours to research
    - When selecting hours to rest
        - With Modifier key, switch between sleep / pass time
    - When selecting tool to use to repair, clean, etc.
    - When selecting units to melt/boil (**A/D Only**, **Bulk compatible**)
    - When selecting units to craft (**A/D Only**)
    - I don't know if I miss something...
- **W/S Selection**
    - When selecting actions in Examine menu: repair, clean, harvest, etc.
- **QuickInventoryNavigation**: Hold the **Modifer** key and press
        - W/S to switch between item categories
        - A/D to switch between filters
- **QuickDoubleClickInventoryItem**: Press the **AlternativeInteract** key to simulate double click in inventory/containers.
- **QuickExamine**:
    - Hold **Modifier** key and press the **AlternativeInteract** key to examine item (like clicking `Actions` button)
- **AlternativeInteractConfirmation**
    - Press the **AlternativeInteract** key to
        - Start fire / Feed fire / Break down / ...
        - Sleep / Pass time
        - Spray / Select food to cook / Select container to cook / ...
        - Light torch (with matches, strikers, etc.)
        - Perform actions in Examine menu: repair, clean, harvest, etc.
        - Confirm units: units to transfer, hours to read, etc.
        - "Take It" when inspecting item
        - I don't know if I miss something...