using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;


public class launch_eye_track : MonoBehaviour
{
    // Declare Variables for UDP
    Thread receiveThread; 
    UdpClient client; 
    int port;

    public string text;

    // For launching python program
    public Process p = new Process();

    void Start()
    {
        port = 5065;

        ProcessStartInfo info = new ProcessStartInfo("ping");
        info.FileName = "cmd.exe";
        info.RedirectStandardInput = true;
        info.UseShellExecute = false;

        p.StartInfo = info;
        p.Start();

        using (StreamWriter sw = p.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine("cd python_eye");
                sw.WriteLine("conda activate env_dlib");
                sw.WriteLine("python realtime_eye_pos_position.py");
            }
        }

        InitUDP();
    }

    private void InitUDP()
    {
        print("UDP Initialized");

        receiveThread = new Thread(new ThreadStart(ReceiveData)); 
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
                byte[] data = client.Receive(ref anyIP);

                text = Encoding.UTF8.GetString(data);
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    void OnApplicationQuit()
    {
        foreach (var q in Process.GetProcessesByName("cmd"))
        {
            q.Kill();
        }
        foreach (var q in Process.GetProcessesByName("python"))
        {
            q.Kill();
        }
    }
}
