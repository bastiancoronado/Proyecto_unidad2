using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class Configuration : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Control;
    public GameObject obj;



    public Dropdown Port, BounRate;
    public string[] serialPorts;
    string[] boudrate = { "9600", "19200", "38400", "115200" };
    void Start()
    {
        List<string> Ports = new List<string>();
        List<string> BoundRates = new List<string>();

        serialPorts = SerialPort.GetPortNames();

        for (ushort i = 0; i < serialPorts.Length; i++) Ports.Add(serialPorts[i]);

        Port.AddOptions(Ports);

        for (uint i = 0; i < boudrate.Length; i++) BoundRates.Add(boudrate[i]);

        BounRate.AddOptions(BoundRates);
    }

    void Update()
    {
        
    }

    public void Desactivar()
    {
        Control.SetActive(true);
        obj.SetActive(true);
        Canvas.SetActive(false);
    }
}
