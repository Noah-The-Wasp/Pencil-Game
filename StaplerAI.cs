using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaplerAI : MonoBehaviour
{

    public Vector3 StaplerPosition;
    public GameObject StaplePrefab;

    //Sets StaplerPosition to be the correct origin location for the firing of projectiles 

    void Start()
    {

        StaplerPosition = new Vector3(transform.position.x, transform.position.y + 8f, transform.position.z);

        StartCoroutine(TimeBetweenShots());

    }

    //Waits 5 seconds between shots, this coroutine is called recursively so the staplers will constantly shoot at the player

    IEnumerator TimeBetweenShots()
    {

        yield return new WaitForSeconds(5);

        Shoot();

    }

    //Shoot instantiates a new staple which fires at the player using its own script

    void Shoot()

    {

        Instantiate(StaplePrefab, StaplerPosition, StaplePrefab.transform.rotation);

        StartCoroutine(TimeBetweenShots());

    }

}
