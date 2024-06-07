using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurbineDetection : MonoBehaviour
{
    public AudioClip wiresVoiceM;
    public AudioClip wiresVoiceF;
    public AudioClip bossClipGlitched, fixM, fixF;

    public bool entered;
    public bool exited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && !entered)
        {
            GameManager.instance.PlayVoice(wiresVoiceM, wiresVoiceF, 1);
            Tablet.instance.UpdateTask("The light seems to be off. Figure out what is wrong with the switch. Check the wiring.", "The lights are off. Take a look at the wires");
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() && Clockwork.instance != null && !Clockwork.instance.interactable && !exited)
        {
            Keypad.instance.interactable = true;
            GameManager.instance.PlayVoice(bossClipGlitched, bossClipGlitched, 0);
            GameManager.instance.PlayVoice(fixM, fixF, 15);
            exited = true;
            Tablet.instance.UpdateTask("Climb on top of the turbine and find what's wrong with the antenna.", "Climb on top of the antenna.");
        }
    }
}
