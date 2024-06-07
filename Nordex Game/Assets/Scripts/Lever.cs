using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public ladder ladder;
    private Animator animator;
    public bool inrange;
    public BoxCollider boxCollider;
    public AudioClip PullSound, JamSound, stuckM, stuckF;
    public bool jammed;

    private float voiceCD;
    private AudioSource source;
    public bool Pulled;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (voiceCD > 0) voiceCD -= Time.deltaTime;

        if (inrange)
        {
            if (Input.GetButtonDown("Interact") && Pulled == false)
            {
                animator.SetTrigger("Pull");
                
                //adding Pull sound
                if (!jammed) 
                {
                    source.clip = PullSound;
                    Pulled = true;
                    source.Play();
                    ladder.GetComponent<Animator>().SetTrigger("GoDown");
                }
                else
                {
                    source.clip = JamSound;
                    if (voiceCD <= 0)
                    {
                        Tablet.instance.UpdateTask("The lever is stuck, take a look at the lever mechanism and think of a way to fix it.", "Find the correct pattern. Beware of malfunctions.");
                        GameManager.instance.PlayVoice(stuckM, stuckF, 1);
                        voiceCD = 5;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
            inrange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
            inrange = false;
    }
}
