using UnityEngine;
using System.Collections;
using System.Net;

public class Debugger {

    public static bool enableLog = true;
    public static string ip = "192.168.0.255";
    public static ushort port = 18000;

    private static UDPClient udpClient;

    public static void Log(string messsage,object sender)
    {
        if (enableLog)
        {
            if (Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.LogFormat("contex: {0} message:{1}", sender.ToString(), messsage);
            }
            else
            {
                if (udpClient == null)
                {
                    udpClient = new UDPClient(port,ip, 1024, null);
                }

                string s = string.Format("contex: {0} message:{1}", sender.ToString(), messsage);
                byte[] bfs = System.Text.Encoding.Default.GetBytes(s);
                udpClient.UDPSend(bfs);
            }
        }
    }

    public static void LogError(string messsage, object sender)
    {
        if (enableLog)
        {
            if (Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.LogErrorFormat("contex: {0} message:{1}", sender.ToString(), messsage);
            }
            else
            {
                if (udpClient == null)
                {
                    udpClient = new UDPClient(port,ip, 1024, null);
                }

                string s = string.Format("contex: {0} message:{1}", sender.ToString(), messsage);
                byte[] bfs = System.Text.Encoding.Default.GetBytes(s);
                udpClient.UDPSend(bfs);
            }
        }
    }

}
