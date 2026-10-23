using Ink.Runtime;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;

    private int currentChoiceIndex = -1;

    private bool dialoguePlaying = false;

    private void Awake()
    {
        story = new Story(inkJson.text);
    }

    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        EventManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
        EventManager.instance.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
    }

    private void OnDisable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        EventManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
        EventManager.instance.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    public void SubmitPressed(InputEventContext context)
    {
        if (!context.Equals(InputEventContext.DIALOGUE))
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

        EventManager.instance.dialogueEvents.DialogueStarted();

        EventManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);

        if (!knotName.Equals(""))
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
        // make a choice, if applicable
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            // reset choice index for next time
            currentChoiceIndex = -1;
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();

            EventManager.instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);
        }
        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {        
        dialoguePlaying = false;

        EventManager.instance.dialogueEvents.DialogueFinished();

        EventManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);

        story.ResetState();
    }
}
