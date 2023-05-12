# TLD-QoL

This is a QoL mod for The Long Dark.

Reduce UI frustration, even make it pleasant to use. You can now use keyboard most of the time to control your game.

## Dependencies

- [KeyboardUtilites by ds8678](https://github.com/ds5678/KeyboardUtilities)

## Known issues

- 1.3.2+ will reset your **AlternativeInteract** key util you turn on the Reset toggle in the settings, this is to prevent users updating from previous versions from being surprised by new features.

## Features

- **Alternative Back button**: Alternative to ESC. Default is Mouse3.
- **Alternative Interact button**: Alternative to LeftMouseButton.
    - Default is Insert, which is intended to be changed to your preference so you don't accidentally touch it.
    - (My personal preference is F, this enables me to effortlessly control UI without moving my left hand most of the time. And my Status is moved to H, Put Away moved to Mouse4)
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
            - Also works in crafting menu
            - There's a not-a-bug issue that after the first time you use this feature, it remember where you were but shows the first category when you open the UI next time
                - This feature use 100% official index value but somehow it seems unused (so it's not set to the first button when you open the Inventory/Crafting). At the moment there is no plan to "fix" this
        - A/D to switch between filters
- **QuickDoubleClickInventoryItem**: Press the **AlternativeInteract** key to simulate double click in inventory/containers.

https://github.com/No3371/TLD-QoL/assets/1814971/92e50a92-107f-407c-ae1d-1501f15fa865

*(Switching categories, selecting item, examining, repairing, all without the mouse)*

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
