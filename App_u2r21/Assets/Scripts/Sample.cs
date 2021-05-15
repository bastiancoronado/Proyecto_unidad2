using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Sample : MonoBehaviour
{
    public Controller serialController;
    private float timer = 0.0f;
    private float waitTime = 0.05f;

    private Transform objTransform;
    private Vector3 scaleChange;


    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<Controller>();
        objTransform = GetComponent<Transform>();
        scaleChange = new Vector3(0f, 0f, 0f);
    }

    // Executed each frame
    void Update()
    {

        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //Debug.Log("Get data 0x73 ");
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

        float x = ((float)System.BitConverter.ToUInt16(message, 0)) / 500F;
        float y = ((float)System.BitConverter.ToUInt16(message, 2)) / 500F;
        float z = ((float)System.BitConverter.ToUInt16(message, 4)) / 500F;
        scaleChange.Set(x, y, z);

        objTransform.localScale = scaleChange;

        /*         StringBuilder sb = new StringBuilder();
                sb.Append("Packet: ");
                foreach (byte data in message)
                {
                    sb.Append(data.ToString("X2") + " ");
                }
                Debug.Log(sb); */
    }
}