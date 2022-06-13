using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.IO;


public class TrackingdataLog : MonoBehaviour
{
    [Header("Data")]
    string serializedData;
    private string RightControllerPos = @"c:\temp\RightContPos.txt";
    private string handPos = @"c:\temp\HandPos.txt";
    private string RightControllerRot = @"c:\temp\RightContRot.txt";
    private string RightControllerVel = @"c:\temp\RightContVel.txt";
    private string RightControllerAcc = @"c:\temp\RightContAcc.txt";
    private string HeadRotation = @"c:\temp\HeadRot.txt";
    private string HeadRotationEuler = @"c:\temp\HeadRotEuler.txt";
    private string HeadPosition = @"c:\temp\HeadPos.txt";
    private string EETransform = @"c:\temp\EETransform.txt";
    private string Sentdata = @"c:\temp\SentDataTransform.txt";

    private string _robotproptrans = @"c:\temp\Robotproptransform.txt";
    private string _robotpropxyz = @"c:\temp\Robotpropxyz.txt";
    private string _robotbasetrans = @"c:\temp\Robotbasetransform.txt";
    private string _robotbasexyz = @"c:\temp\Robotbasexyz.txt";
    private string _propwrtbasetra = @"c:\temp\_propwrtbasetra.txt";
    

    public GameObject Robotproptracker;
    public GameObject RobotBasetracker;

        
    public void Start()
    {
        inDevices();
    }

    void Update()
    {
        //trackerinfo();
        trackerdata();
    }

    #region controller and headata
    private void trackerinfo()
    {
        /*Gets information of devices available */
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        UnityEngine.XR.InputTracking.GetNodeStates(nodeStates);

        //get the state of each node
        foreach (XRNodeState nodestate in nodeStates)
        {
            Vector3 right_vel, right_pos, head_pos, right_acc;
            Quaternion head_rot, right_rot;
            Matrix4x4 Tmat;

            if (nodestate.nodeType == XRNode.RightHand)
            {
                //get pos of the right hand
                if (nodestate.TryGetPosition(out right_pos))
                {
                    //Debug.Log("Right cont Pos is" + right_pos.ToString("F4"));
                        using (StreamWriter sw = File.AppendText(RightControllerPos))
                    {
                        sw.WriteLine(System.DateTime.Now + "," + Time.time + "," + right_pos.ToString("F4"));//time in seconds since start of game
                        //sw.WriteLine("Extra line");                      
                    }
                }
                //get velocity of the right hand
                if (nodestate.TryGetVelocity(out right_vel))
                {
                    using (StreamWriter sw = File.AppendText(RightControllerVel))
                    {
                        sw.WriteLine(System.DateTime.Now.Second + "," + Time.time + "," + right_vel.ToString("F4"));
                        //sw.WriteLine("Extra line");                      
                    }
                }
                //get acc
                if (nodestate.TryGetAcceleration(out right_acc))
                {
                    //Debug.Log("Right cont ACC is" + right_pos.ToString("F4"));
                    using (StreamWriter sw = File.AppendText(RightControllerAcc))
                    {
                        sw.WriteLine(System.DateTime.Now + "," + Time.time + "," + right_acc.ToString("F4"));//time in seconds since start of game
                        //sw.WriteLine("Extra line");                      
                    }
                }

                if (nodestate.TryGetRotation(out right_rot))
                {
                    using (StreamWriter sw = File.AppendText(RightControllerRot))
                    {
                        sw.WriteLine(System.DateTime.Now + "," + Time.time + "," + right_rot.ToString("F4"));
                        //sw.WriteLine("Extra line");                      
                    }
                }
            }
            //get rotation and pos of the head
            if (nodestate.nodeType == XRNode.Head)
            {
                if (nodestate.TryGetRotation(out head_rot))
                {
                    using (StreamWriter sw = File.AppendText(HeadRotation))
                    {
                        sw.WriteLine(System.DateTime.Now + "," + Time.time + "," + head_rot.ToString("F4"));
                        //sw.WriteLine("Extra line");                      
                    }
                }

                if (nodestate.TryGetRotation(out Quaternion headroteuler))
                {
                    using (StreamWriter sw = File.AppendText(HeadRotationEuler))
                    {
                        sw.WriteLine(System.DateTime.Now + "," + Time.time + "," + headroteuler.eulerAngles.ToString("F4"));
                        //sw.WriteLine("Extra line");                      
                    }
                }

                if (nodestate.TryGetPosition(out head_pos))
                {
                    using (StreamWriter sw3 = File.AppendText(HeadPosition))
                    {
                        sw3.WriteLine(System.DateTime.Now + "," + Time.time + "," + head_pos.ToString("F4"));
                        //sw.WriteLine("Extra line");                      
                    }
                }
            }
            //get transform matrix of End Effector
        }

        //Debug.Log("Delta time duration " + Time.deltaTime);

    }
    #endregion

    #region Tracker data
    private void trackerdata()
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        UnityEngine.XR.InputTracking.GetNodeStates(nodeStates);

        //get the state of each node
        foreach (XRNodeState nodestate in nodeStates)
        {

            if (nodestate.nodeType == XRNode.Head)
            {
                using (StreamWriter sw3 = File.AppendText(_robotproptrans))
                {
                    sw3.WriteLine(System.DateTime.Now.Second + "," + Time.time + "," + Robotproptracker.transform.localToWorldMatrix.ToString("F4"));
                    Debug.Log("Robotproptracker.transform.localToWorldMatrix" + Robotproptracker.transform.localToWorldMatrix.ToString("F4"));
                }
                using (StreamWriter sw3 = File.AppendText(_robotpropxyz))
                {
                    sw3.WriteLine(System.DateTime.Now.Second + "," + Time.time + "," + Robotproptracker.transform.position.ToString("F4")+ "," 
                        + Robotproptracker.transform.eulerAngles.ToString("F4"));
                    Debug.Log("Robotproptracker.transform.position" + Robotproptracker.transform.position.ToString("F4") + ","
                        + Robotproptracker.transform.eulerAngles.ToString("F4"));
                }

                using (StreamWriter sw3 = File.AppendText(_propwrtbasetra))
                {
                    sw3.WriteLine(System.DateTime.Now.Second + "," + Time.time + "," + Robotproptracker.transform.worldToLocalMatrix.ToString("F4"));
                    Debug.Log("Robotproptracker.transform.worldToLocalMatrix" + Robotproptracker.transform.worldToLocalMatrix.ToString("F4"));
                }

                using (StreamWriter sw3 = File.AppendText(_robotbasetrans))
                {
                    sw3.WriteLine(System.DateTime.Now.Second + "," + Time.time + "," + RobotBasetracker.transform.localToWorldMatrix.ToString("F4"));
                    Debug.Log("RobotBasetracker.transform.localToWorldMatrix" + RobotBasetracker.transform.localToWorldMatrix.ToString("F4"));
                }

                using (StreamWriter sw3 = File.AppendText(_robotbasexyz))
                {
                    sw3.WriteLine(System.DateTime.Now.Second + "," + Time.time + "," + RobotBasetracker.transform.position.ToString("F4") + "," 
                        + RobotBasetracker.transform.eulerAngles.ToString("F4"));
                    Debug.Log("RobotBasetracker.transform.position"+ RobotBasetracker.transform.position.ToString("F4") + ","
                        + RobotBasetracker.transform.eulerAngles.ToString("F4"));
                }
            }
        }
    }
    #endregion 


    void inDevices()
    {
        //works fine as at 3/6/2020 6:02
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'",
                      device.name, device.role.ToString()));

        }
    }
}