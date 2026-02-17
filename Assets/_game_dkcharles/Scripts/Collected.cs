using UnityEngine;
using Yarn.Unity;

public class Collected : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IncrementCount();
            Destroy(gameObject); // Destroy the collectible after being collected
        }
    }

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
