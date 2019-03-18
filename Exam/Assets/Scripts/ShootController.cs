using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPosition;

    [SerializeField]
    private float interactDistance;

    void Update()
    {
        Ray ray = new Ray(cameraPosition.position, cameraPosition.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Bom");
            }
        }
    }
}
