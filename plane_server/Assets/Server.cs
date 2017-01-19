using UnityEngine;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class Server : MonoBehaviour
{
	SerialPort serial = new SerialPort ("/dev/cu.usbmodem1461", 9600);

	private ServerThread st;
	private bool isSend;//儲存是否發送訊息完畢

	private void Start()
	{
		//開始連線，設定使用網路、串流、TCP
		st = new ServerThread(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "192.168.104.31",8000);
		st.Listen();//讓Server socket開始監聽連線
		st.StartConnect();//開啟Server socket
		isSend = true;
	}

	private void Update()
	{

		if (st.receiveMessage != null)
		{
			Debug.Log("Client:" + st.receiveMessage);
			st.receiveMessage = null;
		}
		if (isSend == true)
			//st.Send("Hello~ My name is Server");
			StartCoroutine(delaySend());//延遲發送訊息

		st.Receive();
	}

	private IEnumerator delaySend()
	{
		//設定感測器數據
		if (!serial.IsOpen)
			serial.Open();
		serial.Write("g");
		float joy1 = float.Parse(serial.ReadLine());
		string s1 = joy1.ToString("R");
		//設定感測器數據
		serial.Write("a");
		float AcX = float.Parse(serial.ReadLine());
		string s2 = AcX.ToString("R");

		serial.Write("c");
		float AcZ = float.Parse(serial.ReadLine());
		string s3 = AcZ.ToString("R");

		serial.Write ("i");
		float joy2 = float.Parse(serial.ReadLine());
		string s4 = joy2.ToString("R");

		serial.Write ("h");
		float button = float.Parse(serial.ReadLine());
		string s5 = button.ToString("R");

		float z=0;
		if (Input.GetKey ("z") == true) {
			z = 1;
		}

		string s6 = z.ToString("R");;

		isSend = false;
		yield return new WaitForSeconds(0);//延遲1秒後才發送
		//st.Send("Hello~ My name is Server");
		st.Send(s1+','+s2+','+s3+','+s4+','+s5+','+s6);
		isSend = true;

	}

	private void OnApplicationQuit()//應用程式結束時自動關閉連線
	{
		st.StopConnect();
	}
}

