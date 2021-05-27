﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Surface { VeryRough, Rough, Neutral, Smooth, VerySmooth };
    float rotation;
    private float Thetaside = 63.44f;
    private float ThetaFront = 90.00f;
    public Surface mySurface;
    private Matrix4x4 T3;

    bool activateside;

    void Start()
    {
        mySurface = Surface.VerySmooth;
    }


    private void Update()
    {
        ChangeSurface();
    }

    
    public Matrix4x4 ChangeSurface()
    {
        switch (mySurface)
        {
            case Surface.VeryRough:
                T3=TransformationChangeface(0.0f);
                break;
            case Surface.Rough:
                T3 = TransformationChangeface(72.0f);
                break;
            case Surface.Neutral:
                T3 = TransformationChangeface(144.0f);
                break;
            case Surface.Smooth:
                T3 = TransformationChangeface(216.0f);
                break;
            case Surface.VerySmooth:
                T3 = TransformationChangeface(288.0f);
                break;
        }
        return T3;
    }

   static Matrix4x4 TransformationChangeface(float Theta)
    {/*ROTATION OF PROP ABOUT Y TO CHANGE FACE*/

        Matrix4x4 T3 = new Matrix4x4();
        T3[0, 0] = Mathf.Cos(Theta);    T3[0, 1] = 0;               T3[0, 2] = Mathf.Sin(Theta);           T3[0, 3] = 0f;
        T3[1, 0] = 0;                   T3[1, 1] = 1.0f;            T3[1, 2] = 0.0f;                       T3[1, 3] = 0;
        T3[2, 0] = -Mathf.Sin(Theta);   T3[2, 1] = 0f;              T3[2, 2] = Mathf.Cos(Theta);           T3[2, 3] = 0f;
        T3[3, 0] = 0f;                  T3[3, 1] = 0f;              T3[3, 2] = 0f;                         T3[3, 3] = 1.0f;
        return T3;
    }

    static Matrix4x4 InvTransformationChangeface(float Theta)
    {/*ROTATION OF PROP ABOUT Y TO CHANGE FACE*/

        Matrix4x4 T3 = new Matrix4x4();
        T3[0, 0] = Mathf.Cos(Theta); T3[0, 1] = 0; T3[0, 2] = -Mathf.Sin(Theta); T3[0, 3] = 0f;
        T3[1, 0] = 0; T3[1, 1] = 1.0f; T3[1, 2] = 0.0f; T3[1, 3] = 0;
        T3[2, 0] = Mathf.Sin(Theta); T3[2, 1] = 0f; T3[2, 2] = Mathf.Cos(Theta); T3[2, 3] = 0f;
        T3[3, 0] = 0f; T3[3, 1] = 0f; T3[3, 2] = 0f; T3[3, 3] = 1.0f;
        return T3;
    }

    Matrix4x4 Transformside(float Thetaside)
    {/*Inverse of T2*/ //to reach the side surfaces of the prop
        Matrix4x4 T4 = new Matrix4x4();
        Thetaside = Thetaside * Mathf.PI / 180;
        T4[0, 0] = 1.0f; T4[0, 1] = 0.0f; T4[0, 2] = 0.0f; T4[0, 3] = 0.0f;
        T4[1, 0] = 0.0f; T4[1, 1] = Mathf.Cos(Thetaside); T4[1, 2] = Mathf.Sin(Thetaside); T4[1, 3] = -0.0285f * Mathf.Cos(Thetaside) - 0.0775f;
        T4[2, 0] = 0.0f; T4[2, 1] = -Mathf.Sin(Thetaside); T4[2, 2] = Mathf.Cos(Thetaside); T4[2, 3] = 0.0285f * Mathf.Sin(Thetaside);
        T4[3, 0] = 0.0f; T4[3, 1] = 0.0f; T4[3, 2] = 0.0f; T4[3, 3] = 1.0f;
        return T4;
    }

    Matrix4x4 TransformFront()
    {/*Inverse of T1*/
        T3[0, 0] = 1.0f; T3[0, 1] = 0.0f; T3[0, 2] = 0.0f; T3[0, 3] = 0.0f;
        T3[1, 0] = 0.0f; T3[1, 1] = 1.0f; T3[1, 2] = 0.0f; T3[1, 3] = -0.116f;
        T3[2, 0] = 0.0f; T3[2, 1] = 0.0f; T3[2, 2] = 1.0f; T3[2, 3] = 0.0f;
        T3[3, 0] = 0.0f; T3[3, 1] = 0.0f; T3[3, 2] = 0.0f; T3[3, 3] = 1.0f;
        return T3;
    }



}
