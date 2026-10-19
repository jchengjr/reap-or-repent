using UnityEngine;

// Demo script for the sake of testing the ink dialogue integration. This script is not used in the final game.
public class SkinnerBox : MonoBehaviour
{
    [SerializeField]
    private string dialogueKnotName;

    private void SubmitPressed()
    {
        if(!dialogueKnotName.Equals(""))
        {
            EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
}
