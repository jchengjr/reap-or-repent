using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [Header("Dialogue (knot)")]
    [SerializeField] private string dialogueKnotName;

    private void OnEnable()
    {
        EventManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        EventManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed(InputEventContext context)
    {
        if (!context.Equals(InputEventContext.DEFAULT))
        {
            return;
        }

        if (!dialogueKnotName.Equals(""))
        {
            EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
}
