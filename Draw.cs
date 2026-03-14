using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{

    public Vector3 RoundedStart;
    public Vector3 RoundedEnd;
    public Vector3 LastPos;

    public int Gap;

    public GameObject DrawPoint;
    public Vector3 DrawPosition;
    public Vector3 TransferLine;

    List<Vector3> LineLocations;
    List<Vector3> MidPointLocator;
    public static List<Vector3> LineLocTransfer;

    public LineRenderer DrawLine;
    public int StupidFrameCount = 1;

    public bool IAmCircular = false;

    public GameObject MiddleCheck;
    public GameObject AreaArea;
    public int CurrentFindingIndex;

    public AudioSource CircleDing;

    void Start()
    {

        LineLocations = new List<Vector3>();
        LastPos = DrawPoint.transform.position;

        CircleDing.playOnAwake = false;

    }

    //Deals with the drawing of the line which constantly follows the player

    void FixedUpdate()
    {

        DrawPosition = DrawPoint.transform.position;

        //Checks if the pencil hasnt moved since the last frame

        if (LastPos != DrawPosition)
        {

            //Adds the current pencil position to a list of pencil positions 

            LastPos = DrawPoint.transform.position;
            LineLocations.Add(DrawPosition);

            //Removes the last element from the list of pencil positions so the line is constantly chasing the player

            if (LineLocations.Count > 1000)
            {

                TransferLine = LineLocations[LineLocations.Count - 1001];

                LineLocations.Remove(TransferLine);

            }

            DrawLine.positionCount = LineLocations.Count;
            DrawLine.SetPositions(LineLocations.ToArray());

            IAmCircular = CircleFinder(DrawPosition);

            LineLocTransfer = LineLocations;

            StupidFrameCount += 1;

        }

    }

    //Finds if the player has drawn a circle

    bool CircleFinder(Vector3 CurrentDrawPos)
    {

        bool IsCircle = false;

        //Rounds the location of current point to the nearest 2 decimal places

        RoundedEnd = new Vector3(((Mathf.Floor((CurrentDrawPos.x) * 10)) / 10), ((Mathf.Floor((CurrentDrawPos.y) * 10)) / 10), ((Mathf.Floor((CurrentDrawPos.z) * 10)) / 10));
        List<Vector3> TempList = new List<Vector3>();

        //Rounds each location in the list to 2 decimal places and checks it against the current location

        for (CurrentFindingIndex = 0; CurrentFindingIndex < (LineLocations.Count - 1); CurrentFindingIndex++)
        {

            RoundedStart = new Vector3(((Mathf.Floor((LineLocations[CurrentFindingIndex].x) * 10)) / 10), ((Mathf.Floor((LineLocations[CurrentFindingIndex].y) * 10)) / 10), ((Mathf.Floor((LineLocations[CurrentFindingIndex].z) * 10)) / 10));

            if (RoundedStart.x == RoundedEnd.x)
            {

                if (RoundedStart.z == RoundedEnd.z)
                {

                    //If a match is found the gap between the current position and the last time that position was logged is found

                    Vector3 GapValue = LineLocations[CurrentFindingIndex];

                    //If the distance is larger than a pre-specified gap then the shape is declared a 'Circle' or really any closed shape

                    if (((LineLocations.IndexOf(CurrentDrawPos)) - (LineLocations.IndexOf(GapValue))) < Gap)
                    {

                        IsCircle = false;

                    }
                    else
                    {

                        IsCircle = true;

                        MiddleCheck.transform.position = (MidPointGetter(LineLocations, CurrentFindingIndex));

                    }

                }

            }

        }

        return IsCircle;

    }

    //Finds the middle point of any cirlces the player draws

    Vector3 MidPointGetter(List<Vector3> MidPointSearch, int CurrentValue)
    {

        Vector3 TempVecOne;
        Vector3 TempVecTwo;
        Vector3 TempVecThree;

        if (MidPointSearch.Count > 0)
        {

            //This code gets 3 unique, random points from the list of values which make up the circle

            List<int> RandomNumbs = new List<int>();
            int TempRand;
            int TempTempRand;
            int TempTempTempRand;
            bool RandFull = false;

            TempTempTempRand = Random.Range(CurrentValue, MidPointSearch.Count);

            TempVecOne = MidPointSearch[TempTempTempRand];

            TempTempRand = Random.Range(CurrentValue, MidPointSearch.Count);

            while (RandFull == false)
            {

                if (TempTempRand != TempTempTempRand)
                {

                    RandFull = true;

                }
                else
                {

                    TempTempRand = Random.Range(CurrentValue, MidPointSearch.Count);

                }

            }

            TempVecTwo = MidPointSearch[TempTempRand];

            RandFull = false;

            TempRand = Random.Range(CurrentValue, MidPointSearch.Count);

            while (RandFull == false)
            {

                if (TempRand != TempTempTempRand && TempRand != TempTempRand)
                {

                    RandFull = true;

                }
                else
                {

                    TempRand = Random.Range(CurrentValue, MidPointSearch.Count);

                }

            }

            //This code uses the 3 unique random values to mathematically calculate the midpoint of the circle they all fall on

            TempVecThree = MidPointSearch[TempRand];

            float XMiddle = (TempVecOne.x + TempVecTwo.x + TempVecThree.x) / 3;
            float ZMiddle = (TempVecOne.z + TempVecTwo.z + TempVecThree.z) / 3;

            float LocalAreaCalc = (AreaFinder(new Vector3(XMiddle, 2, ZMiddle), TempVecTwo));
            Vector3 TempScale = new Vector3(0, 2, 0);

            AreaArea.transform.position = new Vector3(XMiddle, 2f, ZMiddle);
            TempScale.x += Mathf.Abs(LocalAreaCalc - 1);
            TempScale.z += Mathf.Abs(LocalAreaCalc - 1);

            AreaArea.transform.localScale = TempScale;

            if (LocalAreaCalc > 2)
            {

                CircleDing.Play(0);

            }

            return new Vector3(XMiddle, 2f, ZMiddle);

        }
        else
        {

            return new Vector3(0, 0, 0);

        }

    }

    //This code finds the area of the found circle

    float AreaFinder(Vector3 MidPoint, Vector3 EdgePoint)
    {

        float DistanceX = (EdgePoint.x - MidPoint.x);
        float DistanceZ = (EdgePoint.z - MidPoint.z);

        float Distance = Mathf.Sqrt((DistanceX * DistanceX) + (DistanceZ * DistanceZ));

        float Area = Mathf.PI * (Distance * Distance);

        return (Distance * 2);

    }

}
