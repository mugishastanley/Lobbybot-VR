using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trackerdata : MonoBehaviour
{
    private Matrix4x4 Tmat;

    private Vector3 Pos;
    private Quaternion Rot;
    private Vector3 Sc;


    public System.Numerics.Matrix4x4 T1,T2,T3,T4,T5,T6,T7,T8,T9,T10;

    // Start is called before the first frame update
    void Start()
    {
        Tmat = new Matrix4x4();
        Pos = new Vector3(1, 1, 1);
        Sc = new Vector3(1, 1, 1);
        T1 = new System.Numerics.Matrix4x4();


        T1.M11 = 0.70320f;
        T1.M12 = -0.41325f;
        T1.M13 = -0.58013f;
        T1.M14 = -0.22176f;
        T1.M21 = -0.49762f;
        T1.M22 = 0.29516f;
        T1.M23 = - 0.81563f;
   
    }

    // Update is called once per frame
    void Update()
    {
        Pos = this.transform.position;
        Rot = this.transform.rotation;
        Tmat = Matrix4x4.TRS(Pos, Rot, Sc);
        Debug.Log("Transform is "+Tmat);
    }
}
