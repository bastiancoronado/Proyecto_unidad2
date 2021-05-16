using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class Sample : MonoBehaviour
{
    public Controller serialController;
    //private float timer = 0.0f;
    //private float waitTime = 0.05f;

    byte[] Rx = new byte[8];
    string[] dayWeek = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};



    void Start()
    {
        //serialController = GameObject.Find("SerialController").GetComponent<Controller>();

        string[] current = DateTime.Now.ToString().Split(' ');
        string[][] date = { current[0].Trim().Split('/'), current[1].Trim().Split(':'), current[2].Trim().Split('.') };
        Debug.Log(current[1] + " " + current[2] + current[3] + " " + DateTime.Now.DayOfWeek + " " + current[0]);

        Rx[0] = (byte)int.Parse(date[1][2]);
        Rx[1] = (byte)int.Parse(date[1][1]);
        Rx[2] = (byte)int.Parse(date[1][0]);
        Rx[3] = date[2][0].Equals("p") ? (byte)1 : (byte)2;
        Rx[4] = (byte)(1 + Array.IndexOf(dayWeek, DateTime.Now.DayOfWeek.ToString().Trim()));
        Rx[5] = (byte)int.Parse(date[0][0]);
        Rx[6] = (byte)int.Parse(date[0][1]);
        Rx[7] = (byte)(int.Parse(date[0][2]) - 2000);

        Debug.Log(Rx[0] + ", " + Rx[1] + ", " + Rx[2] + ", " + Rx[3] + ", " + Rx[4] + ", " + Rx[5] + ", " + Rx[6] + ", " + Rx[7]);
        

    }


    void Update()
    {

        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------
        /*
        if (Input.GetKeyUp(KeyCode.Q))
        {
            serialController.SendSerialMessage(new byte[] { 0x73 });
        }

        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            timer = timer - waitTime;

            serialController.SendSerialMessage(new byte[] { 0x73 });
        }

        

        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        byte[] message = serialController.ReadSerialMessage();

        if (message == null)
            return;
        */
        /*
        float x = ((float)System.BitConverter.ToUInt16(message, 0)) / 500F;
        float y = ((float)System.BitConverter.ToUInt16(message, 2)) / 500F;
        float z = ((float)System.BitConverter.ToUInt16(message, 4)) / 500F;
        */
    }

    static byte stringToint(string chain, int pos1, int pos2)
    {
        byte bt = (byte)int.Parse(chain.Substring(pos1, pos2));
        return 0;
    }

}