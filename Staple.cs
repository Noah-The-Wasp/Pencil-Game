using UnityEngine;

public class Staple : MonoBehaviour
{

    Vector3 AimPoint;
    public float Step = 0.5f;
    public AudioSource Shoot;

    //Gets the end location of the shot when the staple is fired so it doesnt chase the player around the game

    void Start()
    {

        Shoot.Play(0);

        AimPoint = PlayerMovement.TipTrack;

    }

    //The staple gradually moves towards its target, when it reaches the target it is destroyed

    private void FixedUpdate()
    {

        transform.LookAt(AimPoint);

        transform.position = Vector3.MoveTowards(transform.position, AimPoint, Step);

        if (transform.position == AimPoint)
        {

            Destroy(gameObject);

        }

    }


}
