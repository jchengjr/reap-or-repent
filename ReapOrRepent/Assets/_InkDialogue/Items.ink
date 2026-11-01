// Item functions the game provides. DialogueManager connects them to the player's inventory.
// INCLUDE this file at the top of any master ink file that gives or checks items.
//
//   ~ GiveItem("washcloth")                  gives the player an item (put it under a choice)
//   {HasItem("washcloth"): ...}              text that only shows if they have it
//   +{HasItem("washcloth")} [Clean it]       a choice that only shows if they have it
//
// Item IDs: "lie_detector", "skeleton_key", "washcloth", "dictionary"

EXTERNAL GiveItem(itemId)
EXTERNAL HasItem(itemId)

// Fallbacks so dialogue still plays in Inky, where the game isn't running. In Unity, the game's versions are used instead.
VAR inkyHeldItems = ""

=== function GiveItem(itemId) ===
~ inkyHeldItems += " " + itemId

=== function HasItem(itemId) ===
~ return inkyHeldItems ? itemId
