using UnityEngine;

public class SecondCamView : MonoBehaviour
{

    public Camera MainCamera;
    public Camera SecondCamera;

    public GameObject FOVTrack;
    public GameObject PencilCenter;

    public static bool EnableActionCam;

    //Makes sure the main camera is set on and the secondary camera is off

    void Start()
    {

        MainCamera.enabled = true;
        SecondCamera.enabled = false;

    }

    void FixedUpdate()
    {

        //Causes the secondary camera to rotate around the player constantly even if it is not in use

        FOVTrack.transform.position = new Vector3(PencilCenter.transform.position.x, (PencilCenter.transform.position.y + 2), PencilCenter.transform.position.z);

        FOVTrack.transform.Rotate(0f, 0.5f, 0f, Space.Self);

        //If the camera is enabled due to a large combo then the player is shown the rotating view for around 250 frames

        if (EnableActionCam == true)
        {

            MainCamera.enabled = false;
            SecondCamera.enabled = true;

        }
        else
        {

            MainCamera.enabled = true;
            SecondCamera.enabled = false;

        }

    }

}
