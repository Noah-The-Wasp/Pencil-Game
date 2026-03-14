using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{

    public NavMeshAgent RubberAgent;

    public Transform Enemy;

    public GameObject EnemyGO;
    public Material Damage;

    Vector3 MovePoint;
    public float Speed;

    public static bool StartMove = false;
    public bool BeginDestroy = false;

    public GameObject ParticlePrefab;
    GameObject ParticleInstance;

    public AudioSource Squelch;

    void FixedUpdate()
    {

        MovePoint = new Vector3(PlayerMovement.TipTrack.x, Enemy.transform.position.y, PlayerMovement.TipTrack.z);

        transform.LookAt(PlayerMovement.TipTrack);

        if (StartMove == true)
        {

            //All the enemies use NavMesh to follow the player and avoid obstacles

            if (PlayerMovement.TipTrack.y > -1f)
            {

                RubberAgent.SetDestination(MovePoint);

                RubberAgent.updateRotation = false;

            }

        }

        //If the enemy is encircled then this code is triggered which destroys the death particle prefab after 50 frames

        if (BeginDestroy == true)
        {

            int FrameCount = 0;

            if (FrameCount < 50)
            {

                FrameCount++;

            }
            else
            {

                Destroy(ParticleInstance);

            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        //If the enemy is encircled then the death sound effect is played and a coroutine which makes sure the enemy is only destroyed after the sound associated with it stops playing 

        if (other.gameObject.tag == "Circle")
        {

            Squelch.Play(0);

            if (PlayerMovement.MaxStamina != 250)
            {

                PlayerMovement.MaxStamina += 10;

            }

            StartCoroutine(DestoyAfterSound());

        }

    }

    //This coroutine initiates the death particle effect and then the destruction of the enemy game object when it is called

    IEnumerator DestoyAfterSound()
    {

        while (Squelch.isPlaying)
        {

            yield return null;

        }

        Combo.ComboCount++;
        Combo.DeathPos = EnemyGO.transform.position;
        Combo.ScoreAdd = 100;

        ParticleInstance = Instantiate(ParticlePrefab);
        ParticleInstance.transform.position = new Vector3(EnemyGO.transform.position.x, (EnemyGO.transform.position.y + 2), EnemyGO.transform.position.z);

        BeginDestroy = true;

        Destroy(EnemyGO);

    }

}
