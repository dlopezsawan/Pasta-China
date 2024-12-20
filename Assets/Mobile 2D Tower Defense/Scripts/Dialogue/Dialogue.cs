using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    Button button;

    [SerializeField] GameObject dialoguePanel;

    void Start()
    {
        textComponent.text = string.Empty;
        button = GetComponentInChildren<Button>();
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        Time.timeScale = 0; // Pause the game
        index = 0;
        ShowLine();
    }

    void ShowLine()
    {
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed); // Use unscaled time
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        if(button != null) { button.interactable = true; }
        Debug.Log("stop");
        Time.timeScale = 1; // Resume the game
        if (dialoguePanel != null) { dialoguePanel.SetActive(false); } // Hide dialogue UI
    }
}
