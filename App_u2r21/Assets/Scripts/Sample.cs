using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    public Controller serialController;
    public GameObject clk;
    public Text tx_clk;
    public Text tx_hmd;

    float t = 0;
    float lastt = 0;
    static string[] dayWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    static string[] mounth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "Octuber", "November", "December" };
    byte[] Rx;
    byte[] bt = new byte[] { 0x73 };
    string[] states = { "Init", "Send", "Waith", "Signal" };
    string mode;

    static string[] fecha = new string[8];
    static float real = 0;
    byte[] message;

    void Start()
    {
        
        mode = states[0];
        serialController = GameObject.Find("SerialController").GetComponent<Controller>();
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
                    clk.SetActive(true);
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
                    rt.Append(data.ToString() + "-");
                }

                if (message != null)
                {
                    string[] date = rt.ToString().Split('-');
                    for (uint i = 0; i < fecha.Length; i++) fecha[i] = date[i];                    

                    byte[] realfloat = new byte[4];

                    for (ushort i = 0; i < realfloat.Length; i++) realfloat[i] = message[8 + i];

                    if (!BitConverter.IsLittleEndian) Array.Reverse(realfloat);

                    real = System.BitConverter.ToSingle(realfloat, 0);

                    string[] d = getNow();

                    tx_clk.text = d[2] + ":" + d[1] + ":" + d[0] + " " + d[3] + "\n" + d[4] + ", " + d[5] + " " + d[6] + " " + d[7];
                    tx_hmd.text = "Humidity: " + real + "f.";
                    mode = states[1];
                }

                break;

        }

    }

    static byte[] getTime()
    {
        
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


    public static string[] getNow()
    {
        fecha[3] = fecha[3] == "1" ? "Pm" : "Am"; 
        fecha[4] = dayWeek[int.Parse(fecha[4]) - 1];
        fecha[6] = mounth[int.Parse(fecha[6])];
        fecha[7] = "20" + fecha[7];
        return fecha;
    }

}