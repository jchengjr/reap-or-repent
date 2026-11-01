using System.Collections.Generic;
using UnityEngine;

// Reaper's inventory. Items are ID strings, the same ones in the Ink scripts.
public static class Inventory
{
    private static readonly HashSet<string> items = new HashSet<string>();

    public static void Add(string itemId)
    {
        items.Add(itemId);
    }

    public static bool Has(string itemId)
    {
        return items.Contains(itemId);
    }

    public static void Clear()
    {
        items.Clear();
    }

    // Reset inventory on new session.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetForNewSession()
    {
        items.Clear();
    }
}
