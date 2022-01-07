using System;
using System.Collections;
using UnityEngine;
public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;
    public Transform GetDestination(){
        return destination;
    }
}