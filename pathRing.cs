using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class pathRing : MonoBehaviour
{
    public Image blueRdProgressTracker;
    public Animator animator;
    public Transform mesh;
    private float previousFillAmount = 0f;
    enum Csacsitype {
        red,
        blue,
        none
    }
    List<Csacsitype> objectsInArea = new List<Csacsitype>();
        [SerializeField] private AudioClip audioClip;
    private bool canPlayedSound = false;

    void Start()
    {
       blueRdProgressTracker.fillAmount = 0.5f;
    }




private bool hasPlayedZero = false;
private bool hasPlayedOne = false;

void Update()
{
    updateBar();
    CheckPlaySound();

    if (blueRdProgressTracker.fillAmount <= 0.001f || 
        blueRdProgressTracker.fillAmount >= 0.999f)
    {
        animator.SetBool("isErect", true);
        loadTexture();
    }
    else
    {
        animator.SetBool("isErect", false);
        loadTexture();
    }
}

void CheckPlaySound()
{

    if (blueRdProgressTracker.fillAmount <= 0.001f && !hasPlayedZero)
    {
        SoundFXManager.instance.PlaySoundFXClip(audioClip, transform, 20f);
        hasPlayedZero = true;
        hasPlayedOne = false; 
    }


    if (blueRdProgressTracker.fillAmount >= 0.999f && !hasPlayedOne)
    {
        SoundFXManager.instance.PlaySoundFXClip(audioClip, transform, 20f);
        hasPlayedOne = true;
        hasPlayedZero = false; 
    }
}




    void updateBar()
    {
        if(objectsInArea.Count == 0 || objectsInArea.Contains(Csacsitype.blue)&& objectsInArea.Contains(Csacsitype.red))return;

        if (objectsInArea.Contains(Csacsitype.blue))
        {
            if (blueRdProgressTracker.fillAmount > 0)
            {
                blueRdProgressTracker.fillAmount -= 0.5f *Time.deltaTime;
            }
            
        }
        if (objectsInArea.Contains(Csacsitype.red))
        {
            if (blueRdProgressTracker.fillAmount < 1f)
            {
                blueRdProgressTracker.fillAmount += 0.5f *Time.deltaTime;
            }
        }
    }


    void PlaySound()
    {
        if(!canPlayedSound)return;
        canPlayedSound = false;
        SoundFXManager.instance.PlaySoundFXClip(audioClip,transform, 20f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "BlueCsacsi")
        {
            objectsInArea.Clear();
            objectsInArea.Add(Csacsitype.blue);
            
        }
        if (other.name == "RedCsacsi")
        {
            objectsInArea.Clear();
            objectsInArea.Add(Csacsitype.red);
        }
    }
    void OnTriggerExit(Collider other)
    {

            if (other.name == "BlueCsacsi")
            {
                objectsInArea.Remove(Csacsitype.blue);
            }
           if (other.name == "RedCsacsi")
            {
                objectsInArea.Remove(Csacsitype.red);
            }
        
    }

    private void loadTexture()
    {
                Color teamColor = (blueRdProgressTracker.fillAmount == 0)
            ? new Color32(120, 200, 255, 255)
            : new Color32(230, 60, 60, 255);
        Renderer r = mesh.GetComponent<Renderer>();
        
        Material[] mats = r.materials;

            for (int i = 0; i < mats.Length; i++)
            {
            if (mats[i].name.Contains("Material.001"))
            {
                mats[i] .color = teamColor;
            }
        }

        r.materials = mats;
    }
}
