using UnityEngine;
using Yarn.Unity;

public class QuestGiver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    [YarnCommand("QuestComplete")]
    public void QuestComplete()
    {
        Debug.Log("Quest completed! You can add your quest completion logic here.");
        // You can add additional logic here, such as giving rewards, updating quest status, etc.
    }
}
