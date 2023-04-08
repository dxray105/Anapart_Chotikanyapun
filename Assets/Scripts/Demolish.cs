using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class Demolish : MonoBehaviour
{
    [SerializeField] private GameObject colliding;
    public GameObject Colliding { get { return colliding; } }

 

    private void OnTriggerStay(Collider other)
    {
        colliding = other.gameObject;
    }
}
