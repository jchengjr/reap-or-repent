using Ink.Runtime;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;

    private bool dialoguePlaying = false;

    private void Awake()
    {
        story = new Story(inkJson.text);
    }

    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        EventManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        EventManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void Start()
    {
        // just for testing
        EnterDialogue("startMN1");
    }

    public void SubmitPressed()
    {
        if(!dialoguePlaying)
        {
            return;
        }

        ContinueOrExitStory();
    }

    private void EnterDialogue(string knotName)
    {
        // only ever enter dialogue once.
        if(dialoguePlaying)
        {
            return;
        }

        dialoguePlaying = true;

        if(!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("DialogueManager: EnterDialogue called with empty knotName.");
        }

        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if(story.canContinue)
        {
            string dialogueLine = story.Continue();

            // for testing
            Debug.Log(dialogueLine);
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        Debug.Log("DialogueManager: Exiting dialogue.");
        
        dialoguePlaying = false;

        story.ResetState();
    }
}
