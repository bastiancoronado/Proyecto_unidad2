using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class Sample : MonoBehaviour
{
    public Controller serialController;

    float t = 0;
    float lastt = 0;

    byte[] Rx;
    byte[] bt = new byte[] { 0x73 };
    string[] states = { "Init", "Send", "Waith", "Signal" };
    string mode;

    byte[] message;

    void Start()
    {
        mode = states[0];
        serialController = GameObject.Find("SerialController").GetComponent<Controller>();
        //Debug.Log(bt[0]);
        //Debug.Log(Rx[0] + ", " + Rx[1] + ", " + Rx[2] + ", " + Rx[3] + ", " + Rx[4] + ", " + Rx[5] + ", " + Rx[6] + ", " + Rx[7]);
    }


    void Update()
    {

        t += Time.deltaTime;


        switch (mode)
        {
            case "Init":
                if((lastt + 1) < t)
                {
                    lastt = t;
                    serialController.SendSerialMessage(bt);
                    Rx = getTime();
                    serialController.SendSerialMessage(Rx);
                    serialController.SendSerialMessage(Rx);
                    mode = states[2];
                }            
                break;
            case "Waith":
                message = serialController.ReadSerialMessage();
                if (message == null)
                    return;
                StringBuilder sb = new StringBuilder();                
                foreach (byte data in message)
                {
                    sb.Append(data.ToString("X2") + " ");
                }
                int p = int.Parse(sb.ToString().Substring(0,2));

                if (p == 49)
                {
                    Debug.Log(p);
                    mode = states[1];
                }
                

                break;
            case "Send":
                if ((lastt + 1) < t)
                {
                    lastt = t;
                    serialController.SendSerialMessage(new byte[] { 0x72 });
                    mode = states[3];
                }

                break;

            case "Signal":
                message = serialController.ReadSerialMessage();
                if (message == null)
                    return;
                StringBuilder rt = new StringBuilder();
                foreach (byte data in message)
                {
                    rt.Append(data.ToString() + " ");
                }

                if (message != null)
                {
                    Debug.Log(rt);
                    mode = states[1];
                }

                break;

        }



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

    static byte[] getTime()
    {
        string[] dayWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        byte[] Tx = new byte[8];
        string[] current = DateTime.Now.ToString().Split(' ');
        string[][] date = { current[0].Trim().Split('/'), current[1].Trim().Split(':'), current[2].Trim().Split('.') };

        Tx[0] = (byte)int.Parse(date[1][2]);
        Tx[1] = (byte)int.Parse(date[1][1]);
        Tx[2] = (byte)int.Parse(date[1][0]);
        Tx[3] = date[2][0].Equals("p") ? (byte)1 : (byte)2;
        Tx[4] = (byte)(1 + Array.IndexOf(dayWeek, DateTime.Now.DayOfWeek.ToString().Trim()));
        Tx[5] = (byte)int.Parse(date[0][0]);
        Tx[6] = (byte)int.Parse(date[0][1]);
        Tx[7] = (byte)(int.Parse(date[0][2]) - 2000);

        return Tx;
    }
}