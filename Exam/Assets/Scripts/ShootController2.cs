using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController2 : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPosition;

    [SerializeField]
    private float interactDistance;

    [SerializeField]
    private GameObject bulletTrack;

    [SerializeField]
    private Gun gun;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(cameraPosition.position, cameraPosition.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Wall")
                {
                    Vector3 contact = hit.point;
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                    GameObject instantiatedBulletHole = Instantiate(bulletTrack, contact, rotation) as GameObject;
                    instantiatedBulletHole.transform.parent = hit.transform;
                }

                if (hit.collider.tag == "Obstacles")
                {
                    var s = hit.transform.GetComponent<Obstacles>();

                    Debug.Log(s);

                    s.DamageToObstacles(gun.Damage());

                    Vector3 contact = hit.point;
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                    GameObject instantiatedBulletHole = Instantiate(bulletTrack, contact, rotation) as GameObject;
                    instantiatedBulletHole.transform.parent = hit.transform;
                }

                if (hit.collider.tag == "Enemy")
                {
                    var s = hit.transform.GetComponent<Enemy>();

                    s.DamageToObstacles(gun.Damage());

                    Vector3 contact = hit.point;
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                    GameObject instantiatedBulletHole = Instantiate(bulletTrack, contact, rotation) as GameObject;
                    instantiatedBulletHole.transform.parent = hit.transform;
                }                
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cameraPosition.position, cameraPosition.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                if (hit.collider.tag == "Hill")
                {
                    Destroy(hit.transform.gameObject);
                    StatPlayer.hp += 100;
                }
            }
        }


    }
}
