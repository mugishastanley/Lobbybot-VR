using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Advertisements.Editor;
using UnityEngine.UI;

public class KdFindClosest : MonoBehaviour
{
   public GameObject WhitePrefab;
    public GameObject BlackPrefab;
    public Transform CalTracker;
    public GameObject RobotProp;
    
    private float _threshold = 0.3f;
    private Vector3 _gazevector;

    [SerializeField]
    private GameObject[] points;
    [SerializeField]
    private GameObject[] safepoints;

    private Vector3 nearobpostion;
    private Quaternion nearobrot;

    Camera cam;

    protected KdTree<SpawnedPoint> PointsInCar = new KdTree<SpawnedPoint>();
    protected KdTree<SpawnedPoint> Hands = new KdTree<SpawnedPoint>();
    protected List<SpawnedPoint> Safepoints = new List<SpawnedPoint>();
    protected List<SpawnedPoint> Allpoints = new List<SpawnedPoint>();

    private SpawnedPoint First;
    private bool _trackhome = true;

    public Vector3 Nearobpos { get; set; }
    public Vector3 Colorpose { get; set; }
    public string Idtoros { get; set; }

    void Start()
    {
        init();
    }
    
    public void init()
    {
        _gazevector = new Vector3(0, 0, 0);
        PointsInCar.UpdatePositions();
        cam = Camera.main;
        for (int i = 0; i < points.Length; i++)
        {
            var num=i+1;
            GameObject point = (Instantiate(BlackPrefab, points[i].transform.position, 
                points[i].transform.rotation, 
                CalTracker.transform));
            point.GetComponent<SpawnedPoint>().Id = num.ToString();
            PointsInCar.Add((point).GetComponent<SpawnedPoint>());
        }
        
        for (int i = 0; i < safepoints.Length; i++)
        {
            
            var num=1+i;
            //initialise the points of interest  
            GameObject point = (Instantiate(BlackPrefab, safepoints[i].transform.position, 
                safepoints[i].transform.rotation, 
                CalTracker.transform));
            point.GetComponent<SpawnedPoint>().Id = "SP"+num;
            Safepoints.Add((point).GetComponent<SpawnedPoint>());
            Allpoints.Add(point.GetComponent<SpawnedPoint>());
            //Debug.Log("Safepoint" + num + point.transform.localPosition.ToString("F4"));
            //Debug.Log("Safepoint" + num + point.transform.position.ToString("F4"));

        }
    
        Hands.Add(Instantiate(WhitePrefab).GetComponent<SpawnedPoint>());
        First = Safepoints[0];
    }

    // Fixed update is used,  Eye gaze tracking is resource hungry.
    private void FixedUpdate()
    {
        _gazevector = FindObjectOfType<SRanipal_GazeRay>().GazeDirection;
        WithHead_Handthreshold_Homepose2();
        Debug.Log("Id to ros"+ Idtoros);
    }

    
     private void WithHead_Handthreshold_Homepose2()
    {
        /* This function selects safe point based on safe point based on association with a given point
         * and not the head gaze.
         * 
         */
        
        foreach (var hand in Hands)
        {
            SpawnedPoint nearestObj = PointsInCar.FindClosest(hand.transform.position); 
            //nearestObj.tag = "nearestpoint";
            //First.tag = "nearestpoint";
            //pts = GameObject.FindGameObjectWithTag("nearestpoint");
            //renderers = pts.GetComponents<Renderer>();
            //nearestObj 
            
            //List<SpawnedPoint> pts2 = new List<SpawnedPoint>();
            //if(IsVisible(nearestObj.GetComponent<Renderer>()))
            //    pts2.Add(nearestObj);
           // if(IsVisible(second.GetComponent<Renderer>()))
             //   pts2.Add(second);
            //var point = Use_Angles(pts2, _lambda);

            //if ((Vector3.Distance(nearestObj.transform.position, hand.transform.position) < _threshold) && IsVisible(nearestObj.GetComponent<Renderer>()))
                if ((Vector3.Distance(nearestObj.transform.position, hand.transform.position) < _threshold))
                {
                //smallestf = dist;
                nearestObj = nearestObj;
                _trackhome = false;
            }
            else
            {
                _trackhome = true;
                //nearestObj = MovetoSafePoint();
                nearestObj = MovetoSafePoint_eye();

            }
            var position = nearestObj.transform.position;
            Debug.DrawLine(hand.transform.position, position, Color.red);
            //nearestObj.Id = "15";
            //var face = 0;
            //int result = Int32.Parse(nearestObj.Id);
            
            //Added to test prop rotation
            if (nearestObj.Id == "15")
            {
                int result = Int32.Parse(nearestObj.Id);
                int face = FindObjectOfType<LineRenderSettings>().Facenum;
                int newres = result+face;
                Idtoros = newres.ToString();
            }
            else
                Idtoros = nearestObj.Id;

            Nearobpos = nearestObj.transform.localPosition;
            Colorpose = nearestObj.transform.position;
            nearobrot = nearestObj.transform.localRotation;
            if (First != nearestObj)
                First = nearestObj;
        }
    }
    
    
     private void KdWithoutHead1()
     {
         foreach (var hand in Hands)
         {
             SpawnedPoint nearestObj = PointsInCar.FindClosest(hand.transform.position);
             Debug.DrawLine(hand.transform.position, nearestObj.transform.position, Color.red);
             Idtoros = nearestObj.Id;
             Nearobpos = nearestObj.transform.localPosition;
             Colorpose = nearestObj.transform.position;
             nearobrot = nearestObj.transform.localRotation;
         }
     }


    public void KdWithoutHead2()
    {
        foreach (var hand in Hands)
        {
            SpawnedPoint nearestObj = PointsInCar.FindClosest(hand.transform.position);
         if (Vector3.Distance(nearestObj.transform.position, hand.transform.position) < 0.3f)
            {
                Debug.DrawLine(hand.transform.position, nearestObj.transform.position, Color.red);
                Nearobpos = nearestObj.transform.localPosition;
                nearobrot = nearestObj.transform.localRotation;
            }
        }
    }
    
    public Vector3 getclosestobjectposition()
    {
        return nearobpostion;
    }

    public Quaternion getclosestobjectrotation()
    {
        return nearobrot;
    }
    private bool IsVisible(Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return true;
        else
            return false;
    }
    
    Vector3 ClosestPointOnLineSegment(Vector3 segmentStart, Vector3 segmentEnd, Vector3 point) {
        // Shift the problem to the origin to simplify the math.    
        var wander = point - segmentStart;
        var span = segmentEnd - segmentStart;

        // Compute how far along the line is the closest approach to our point.
        float t = Vector3.Dot(wander, span) / span.sqrMagnitude;

        // Restrict this point to within the line segment from start to end.
        t = Mathf.Clamp01(t);

        // Return this point.
        return segmentStart + t * span;
    }
    
    
    SpawnedPoint MovetoSafePoint_eye()
    {
        /*This function returns closest object based on the angle from eye gaze direction
         * Input : point 1, point2, head ray
         * Output: point star
         *steps.
         * Draw ray from cam.
         * l1 from point from cam,
         * l2 from ray
         * 
         */
        var position = cam.transform.position;
        SpawnedPoint nearest = Safepoints[0];
        var minAng = 300.0f;
        for (int i = 0; i < Safepoints.Count; i++)
        {
            var tt= ClosestPointOnLineSegment(
                position, position + _gazevector * 10f, Safepoints[i].transform.position);
            //Debug.DrawLine(position,cam.transform.forward * 10, Color.green);
            var l1 = Vector3.Distance(position, tt);
            var l2 = Vector3.Distance(Safepoints[i].transform.position, tt);
            var ang = (float)(Math.Atan2(l2, l1)*180/3.14162);

            if ((ang < minAng))
            {
                minAng = ang;
                nearest = Safepoints[i];
            }
        }
        //Debug.DrawRay(position,position+_gazevector * 10, Color.green);
        Debug.DrawLine(position,nearest.transform.position, Color.red);
       // Debug.DrawLine(position,nearest.transform.position, Color.red);
        return nearest;
    }
}
