using UnityEngine;
using Yarn.Unity;


public class Count_Contacts : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string nodeName;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered && !dialogueRunner.IsDialogueRunning)
        {
            hasTriggered = true; // Prevent re-triggering
            dialogueRunner.StartDialogue(nodeName);
        }
    }

    // Reset trigger when player leaves, allowing dialogue to trigger again if re-entering
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasTriggered = false;
        }
    }

    [YarnCommand("IncrementCount")]
    public void IncrementCount()
    {
        // Increment the count variable in Yarn
        if (dialogueRunner.VariableStorage != null)
        {
            object value;
            int currentCount = 0;
            if (dialogueRunner.VariableStorage.TryGetValue("$yarn_count", out value))
            {
                if (value is float f)
                {
                    currentCount = (int)f;
                }
                else if (value is int i)
                {
                    currentCount = i;
                }
            }
            dialogueRunner.VariableStorage.SetValue("$yarn_count", currentCount + 1);
            Debug.Log("Count incremented to: " + (currentCount + 1));
        }
    }
}