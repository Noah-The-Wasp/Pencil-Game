using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static float XMove;
    public static float ZMove;

    private float Speed = 15;
    public static float MaxStamina = 250;

    public static Vector3 TipTrack;

    public GameObject Tip;
    public GameObject Pencil;
    public GameObject Rubber;

    public float Sprint = 230f;
    public bool CanSprint = true;
    public bool Dashed = false;

    public bool IsSprinting = false;
    public bool IsMoving = true;

    public Vector3 Jump;
    public float JumpHeight;
    public bool OnGround;
    public Rigidbody PencilRB;
    private Vector3 StartLocation;

    public AudioSource PencilMove;
    public AudioSource PencilSprint;

    public Vector3 CurrentFramePos;
    public Vector3 LastFramePos;

    //Deals with setting up variables before movement can occur, such as muting the move sound effects

    void Start()
    {

        OnGround = true;
        StartLocation = Tip.transform.position;

        LastFramePos = Tip.transform.position;

        PencilMove.mute = true;
        PencilSprint.mute = true;

    }

    //Checks if the player is grounded and thus if they can jump

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Ground")
        {

            OnGround = true;

        }

    }

    //Reduces the player's maximum stamina if they take damage from scissors

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Scissors")
        {

            MaxStamina -= 10;

        }

    }

    void FixedUpdate()
    {

        //Tip Track is the point which the pencil follows, this code deals with movement, sprinting and the ability to dash

        TipTrack = Tip.transform.position;

        if ((Input.GetKeyDown(KeyCode.Space)) && OnGround == true)
        {

            OnGround = false;

            PencilRB.AddForce(Jump * JumpHeight, ForceMode.Impulse);

        }

        if (Sprint > MaxStamina - 1)
        {

            CanSprint = true;

        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && CanSprint == true)
        {

            Speed = 35;

            IsSprinting = true;

        }

        if (Input.GetKeyDown(KeyCode.E) && CanSprint == true)
        {

            Speed = 100;
            Dashed = true;

        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || CanSprint == false)
        {

            Speed = 25;

            IsSprinting = false;

        }

        XMove = Input.GetAxis("Horizontal");
        ZMove = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Speed * Time.deltaTime * ZMove);
        transform.Translate(Vector3.right * Speed * Time.deltaTime * XMove);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {

            IsMoving = true;

        }
        else
        {

            IsMoving = false;

        }

        if (IsMoving == true && OnGround == true)
        {

            if (IsSprinting == true)
            {

                PencilMove.mute = true;
                PencilSprint.mute = false;

            }
            else
            {

                PencilSprint.mute = true;
                PencilMove.mute = false;

            }

        }
        else
        {

            PencilMove.mute = true;
            PencilSprint.mute = true;

        }

        Pencil.transform.position = Tip.transform.position;

        if (Tip.transform.position != StartLocation)
        {

            EnemyAI.StartMove = true;

        }

        if (Speed == 35)
        {

            if (Sprint > 5)
            {

                Sprint -= 0.1f;

            }
            else if (Sprint < 5)
            {

                CanSprint = false;

            }

        }
        else if (Speed == 25)
        {

            if (Sprint < MaxStamina)
            {

                Sprint += 0.08f;

            }

        }
        else if (Speed == 100)
        {

            if ((Sprint > 5) && (Dashed == true))
            {

                Sprint -= 2.5f;

            }
            else if ((Sprint < 5) && (Dashed = true))
            {

                Dashed = false;
                CanSprint = false;

            }

        }

        //Reduces the size of the rubber when the player sprints or dashes

        Rubber.transform.localScale = new Vector3(Rubber.transform.localScale.x, Rubber.transform.localScale.y, (Sprint / 200));

    }

    public void MuteAudio()
    {

        PencilMove.mute = true;
        PencilSprint.mute = true;

    }

}
