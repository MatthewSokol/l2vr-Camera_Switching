using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTouch : MonoBehaviour {

    public Camera firstPerson;
    public View activeView = View.THIRD;
    public int i = 0;
    private bool prevA;
    

    // Use this for initialization
    void Start ()
    {
        switchView(activeView);
    }

    // Update is called once per frame
    void Update ()
    {
        OVRInput.Update();
        //print("right touch update");
        //bool thumbstickDown = OVRInput.GetDown(OVRInput.RawButton.RThumbstick);
       // bool thumbstickUp = OVRInput.GetUp(OVRInput.RawButton.RThumbstick);
       // bool aDown = OVRInput.GetDown(OVRInput.Button.Three);
       // bool aUp = OVRInput.GetUp(OVRInput.Button.Three);
       // bool bDown = OVRInput.GetDown(OVRInput.Button.Four);
       // bool bUp = OVRInput.GetUp(OVRInput.Button.Four);

        bool a = OVRInput.Get(OVRInput.Button.Three);
        bool b = OVRInput.Get(OVRInput.Button.Four);
        bool thumbstick = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);

        //NOTE: touch controllers seem to be getting crossed

        //mimic for others...
        if (a != prevA && a)
        {

            switchCamera();
            prevA = a;
        }
        else if (a != prevA && !a)
        {

            prevA = a;
        }
        if (b)
        {

        }
        if (thumbstick)//NOT WORKING
        {

        }
        float triggerPressure = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        float gripPressure = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
        if (triggerPressure > 0)
        {

        }
        if (gripPressure > 0)
        {

        }

        // get input data from keyboard or controller
        Vector2 thumb = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 position = transform.position;
        position.x += thumb.x  * Time.deltaTime;
        position.z += thumb.y  * Time.deltaTime;
    }

    private bool switchCamera()
    {
        bool success = false;
        View nextView = activeView;
        while (!success)
        {
            nextView = getNextView(nextView);
            success = switchView(nextView);
        }
        return success;
    }

    private View getNextView(View currentView)
    {
        Array allViews = Enum.GetValues(typeof(View));
        int idOfView = 0;
        for (int i = 0; i < allViews.Length; i++)
        {
            idOfView = i;
            if (((View)allViews.GetValue(idOfView)) == currentView)
            {
                break;
            }
        }
        if (idOfView < allViews.Length - 1)
        {
            idOfView++;
        }
        else
        {
            idOfView = 0;
        }
        return (View) allViews.GetValue(idOfView);
    }

    private bool switchView(View newView)
    {
        bool retval = false;
        newView.Zoom(firstPerson);
         
        this.activeView = newView;
        retval = true;
        print("Switched to: " + this.activeView);
       
        return retval;
    }
    
}

public enum View
{
    FIRST,
    THIRD,
    LONG,
}

public static class ViewExtensions
{

    public static bool Zoom(this View v, Camera c)
    {
        bool success = true;
        float targetZoom = 1;
        float speed = 1;
        Transform playerTransform = GameObject.Find("/Player").transform;


        Vector3 pos = c.transform.position;
        Quaternion rot = c.transform.rotation;
        switch (v)
        {
            case View.THIRD:
                targetZoom = -3;
                pos.y = playerTransform.position.y + 5;
                pos.z = -5;
                rot.x = 35;
                RightTouch.print("New " + c.name + " camera position: " + pos);
                break;
            case View.LONG:
                targetZoom = -6;
                pos.y = playerTransform.position.y + 8;
                pos.z = -10;
                rot.x = 40;
                RightTouch.print("New " + c.name + " camera position: " + pos);

                break;
            case View.FIRST:
            default:
                pos.y = playerTransform.position.y;
                pos.z = playerTransform.position.z;
                rot.x = playerTransform.rotation.x;
                RightTouch.print("New " + c.name + " camera position: " + pos);
                break;
        }
        
        c.transform.position = pos;
        c.transform.rotation = rot;
        //Camera.current.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetZoom, Time.deltaTime * speed);
        return success;
    }
}