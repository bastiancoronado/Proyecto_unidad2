using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

using System.Text;

public class Protocol : AbstractSerialThread
{
    // Buffer where a single message must fit
    private byte[] buffer = new byte[1024];
    private int bufferUsed = 0;

    public Protocol(string portName,
                                      int baudRate,
                                      int delayBeforeReconnecting,
                                      int maxUnreadMessages)
        : base(portName, baudRate, delayBeforeReconnecting, maxUnreadMessages, false)
    {

    }

    protected override void SendToWire(object message, SerialPort serialPort)
    {
        byte[] binaryMessage = (byte[])message;
        serialPort.Write(binaryMessage, 0, binaryMessage.Length);
    }

    protected override object ReadFromWire(SerialPort serialPort)
    {
        if (serialPort.BytesToRead > 0)
        {
            bufferUsed = 0;
            // wait for the rest of data
            while (bufferUsed < (buffer[0] + 1))
            {
                bufferUsed = bufferUsed + serialPort.Read(buffer, bufferUsed, 12);
            }
            // send the package to the application
            byte[] returnBuffer = new byte[bufferUsed + 1];
            System.Array.Copy(buffer, returnBuffer, bufferUsed);
            bufferUsed = 0;
            return returnBuffer;
        }
        else
        {
            return null;
        }
    }

}