using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TerminalTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float delay = 0.1f;
   
    public Boolean title = false;
    private string terminalText = "Welcome to the old terminal.\nType commands below:";
    private string terminalTextTitle = 
               "....................................................................\n"
               + "ASSEMBLY LANGUAGE LOADER (v2.3.1)\n"
               + "..................................................................\n";

    private string terminalTextSubtitle = 
            "Searching for vulnerable devices...\n"
            + "*Drive A: Not secure\n"
            + "*Drive B: Not secure\n"
            + "*Drive C: Vulnerable hard disk detected\n"
            + "*Injecting malicious kernel\n"
            + "*Memory check...Bypassing security\n"
            + "*Hijacking system devices...\n"
            + "*Hacking core services...\n"
            + "*Malicious Kernel v2 loaded...\n"
            + "*Compromising user environment...\n"
            + "*Hacker Login: Root Success  _"; // Simulated blinking cursor

    private string terminalTextSubtitle2 =
             "*Encrypting sensitive data...\n"
            + "*Disabling antivirus protection...\n"
            + "*Initiating remote access...\n"
            + "*Downloading user credentials...\n"
            + "*Injecting backdoor into security...\n"
            + "*Executing ransomware payload...\n"
            + "*Broadcasting fake system alerts...\n"
            + "*Covering tracks...\n"
            + "*Preparing to exfiltrate data...\n"
            + "*Hacker Logout: Done"; // Simulated blinking cursor
    private float clearDelay = 2f; // Delay before clearing text


    private bool showCursor = true;

    void Start()
    {
        StartCoroutine(TypeText());
       // StartCoroutine(BlinkCursor());
    }

    IEnumerator TypeText()
    {
        terminalText = terminalTextSubtitle;
        if (title) {
            terminalText = terminalTextTitle;
        }else
        {
            yield return new WaitForSeconds(2); // Delay for 2 seconds
        }
      
        textMeshPro.text = "";
       
        foreach (char letter in terminalText.ToCharArray())
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(delay);
        }
      //  textMeshPro.text += "_"; // initial cursor

        if (!title) {
            ClearText();
            StartCoroutine(BlinkCursor()); 
            yield return new WaitForSeconds(clearDelay);
            terminalText = terminalTextSubtitle2;
            foreach (char letter in terminalText.ToCharArray())
            {
                textMeshPro.text += letter;
                yield return new WaitForSeconds(delay);
            }
              // textMeshPro.text += "_"; // initial cursor
        } // Delay for 2 seconds
        if (!title) {
            yield return new WaitForSeconds(2); // Delay for 2 seconds
            StartCoroutine(TypeText());
        }
       
    }


    void ClearText()
    {
        textMeshPro.text = ""; // Clear all text at once
    }

    IEnumerator BlinkCursor()
    {
        float blinkDuration = 1.5f; // Total duration for blinking effect
        float timer = 0f;
        float cursorBlinkRate = 0.5f;
        while (timer < blinkDuration)
        {
            textMeshPro.text = "_"; // Show the cursor
            yield return new WaitForSeconds(cursorBlinkRate);
            textMeshPro.text = ""; // Hide the cursor
            yield return new WaitForSeconds(cursorBlinkRate);
            timer += cursorBlinkRate * 2; // Each blink cycle takes cursorBlinkRate seconds
        }
    }
}