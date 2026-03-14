using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EraserPrefab;
    public GameObject ScissorPrefab;
    public GameObject SpawnArea;

    bool InstantiatePrefab = true;

    void FixedUpdate()
    {
        
        //Calls a coroutine to wait for 5 seonds every 5 seconds to control enemy spawn rates

        if (InstantiatePrefab == true)
        {

            StartCoroutine(WaitSomeSeconds());

            InstantiatePrefab = false;

        }

    }

    //Chooses a random number from 1 to 10, giving scissors a 1 in 10 chance to spawn, it then waits 5 seconds and resets using the code in update

    IEnumerator WaitSomeSeconds()
    {

        int RandNum = Random.Range(1, 10);

        if (RandNum == 5)
        {

            Instantiate(ScissorPrefab, SpawnArea.transform.position, ScissorPrefab.transform.rotation);

        }
        else
        {

            Instantiate(EraserPrefab, SpawnArea.transform.position, EraserPrefab.transform.rotation);

        }

        yield return new WaitForSeconds(5);

        InstantiatePrefab = true;

    }

}
