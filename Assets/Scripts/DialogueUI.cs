using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textDialog;

    [SerializeField]
    private float writingSpeed = 50f;

    public List<string> dialogs;

    private void Start()
    {
        StartCoroutine(StepThrowDialogs());
    }

    IEnumerator StepThrowDialogs() {
        foreach (string dialogue in dialogs) {
            yield return TypingText(dialogue);
            yield return new WaitUntil(() => Keyboard.current.leftShiftKey.wasPressedThisFrame);
        }
    }

    IEnumerator TypingText(string textToType) {

        textDialog.text = string.Empty;

        float t = 0f;

        int charIndex = 0;

        while (charIndex < textToType.Length) {
            t += Time.deltaTime * writingSpeed;

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textDialog.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        textDialog.text = textToType;
    }
}
