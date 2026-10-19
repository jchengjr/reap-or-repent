using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }

    // Define your events here

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instance of EventManager found!");
        }
        instance = this;

        // initialize all events
    }
}
