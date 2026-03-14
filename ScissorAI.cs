using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class ScissorAI : MonoBehaviour
{

    bool HasBeenHit = false;

    public Transform Enemy;

    public GameObject EnemyGO;

    Vector3 MovePoint;
    public float Speed;

    public static bool StartMove = false;
    public bool BeginDestroy = false;

    public GameObject ParticlePrefab;
    GameObject ParticleInstance;

    public AudioSource Squelch;

    public MeshFilter ScissorsMesh;
    public Mesh Damaged;

    bool DamageCoolDown = false;

    public NavMeshAgent ScissorsAgent;

    void Start()
    {

        Squelch.mute = true;

    }

    void FixedUpdate()
    {

        //Scissors navigate with NavMesh, their heigh if offset to avoid a glitch where they would float in the air

        MovePoint = new Vector3(PlayerMovement.TipTrack.x, Enemy.transform.position.y, PlayerMovement.TipTrack.z);

        transform.LookAt(PlayerMovement.TipTrack);

        ScissorsAgent.height = 1.1f;
        ScissorsAgent.baseOffset = 0;

        if (PlayerMovement.TipTrack.y > -1f)
        {

            ScissorsAgent.SetDestination(MovePoint);

            ScissorsAgent.updateRotation = false;

        }

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

        //Makes sure the scissors havent already been hit once in the last 5 seconds

        if (DamageCoolDown == false)
        {

            if (other.gameObject.tag == "Circle")
            {

                //If the scissors have 1 health they die, if they have 2 then their sprite is updated and their health reduced

                if (HasBeenHit == true)
                {

                    Squelch.Play(0);

                    StartCoroutine(DestoyAfterSound());

                }
                else
                {

                    ScissorsMesh.mesh = Damaged;

                    Squelch.mute = false;
                    Squelch.Play(0);

                    ParticleInstance = Instantiate(ParticlePrefab);
                    ParticleInstance.transform.position = new Vector3(EnemyGO.transform.position.x, (EnemyGO.transform.position.y + 2), EnemyGO.transform.position.z);

                    HasBeenHit = true;
                    DamageCoolDown = true;

                    StartCoroutine(DamageCoolDownWait());

                }

            }

        }

    }

    //Damage cooldown timer so the scissors cant be hit twice in the same frame

    IEnumerator DamageCoolDownWait()
    {

        Debug.Log("Waiting");

        yield return new WaitForSeconds(5);

        Debug.Log("Waited");

        DamageCoolDown = false;

    }

    //Makes sure the full sound effect plays before the scissor object is destroyed

    IEnumerator DestoyAfterSound()
    {

        while (Squelch.isPlaying)
        {

            yield return null;

        }

        Combo.ComboCount++;
        Combo.DeathPos = EnemyGO.transform.position;
        Combo.ScoreAdd = 300;

        ParticleInstance = Instantiate(ParticlePrefab);
        ParticleInstance.transform.position = new Vector3(EnemyGO.transform.position.x, (EnemyGO.transform.position.y + 2), EnemyGO.transform.position.z);

        Destroy(EnemyGO);

        BeginDestroy = true;

    }

}
