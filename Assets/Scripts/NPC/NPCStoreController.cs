using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStoreController : MonoBehaviour, Interactable
{
    public GameObject panelNPCStore;
    void Start(){
        ClosePanel();
    }
    public void Interact()
    {
        panelNPCStore.SetActive(true);
        Debug.Log("Interacting with NPC store");
    }

    public void ClosePanel(){
        panelNPCStore.SetActive(false);
    }
}
