using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private bool dialoguePlaying = false;

    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
    }

    private void OnDisable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
    }

    private void EnterDialogue(string knotName)
    {
        // only ever enter dialogue once.
        if(dialoguePlaying)
        {
            return;
        }

        dialoguePlaying = true;
    }
}
