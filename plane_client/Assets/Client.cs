using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;


public class Client : MonoBehaviour
{
	public GameObject cube;
	private ClientThread ct;
	private bool isSend;
	private bool isReceive;

	public GameObject cam1, cam2;

	private Transform m_transform;
	public float speed = 50f; 
	private float rotationz = 0.0f;         //繞Ｚ軸的旋轉量
	private float rotationx = 0.0f;
	public float rotateSpeed_AxisZ = 45f;   //绕Z轴的旋轉速度
	private int flag1 = 0;//Joystick1 上下
	private int flag2 = 0;//AcX
	private int flag3 = 0;//AcZ
	private int flag4 = 0;//joy左右
	private int flag5 = 0;//拍照狀況
	private int flag6 = 0;//初始狀況
	private int flag =0;//撞擊判斷
	Vector3 v;//初始化位置
	Quaternion v2;//機頭方向
	private void Start()
	{
		m_transform = this.transform; 
		ct = new ClientThread(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "192.168.104.31", 8000);
		ct.StartConnect();
		isSend = true;


	
		//鎖定使用者
		transform.LookAt(cube.transform.position);
		v  = this.transform.position;
		v2 = this.transform.rotation;

	}

	private void Update()
	{
		GameObject.Find("propeller").transform.Rotate(new Vector3(0, 100f, 0));
		rotationz = this.transform.eulerAngles.z;
		rotationx = this.transform.eulerAngles.x;


		if (flag1 == 1) {
			m_transform.Translate (new Vector3 (0, speed / 15 * Time.deltaTime, 0));
		} else if (flag1 == 2) {
			m_transform.Translate (new Vector3 (0, speed / -15 * Time.deltaTime, 0));
		} else {m_transform.Translate (new Vector3 (0, 0, 0));
		}

		if (flag2 == 1) {
			if (rotationz <= 45 || rotationz >= 315) {
				// 飛機向左傾斜
				m_transform.Rotate (new Vector3 (0, 0, (Time.deltaTime * -rotateSpeed_AxisZ)), Space.Self);

			}
			m_transform.RotateAround (cube.transform.position,new Vector3(0,Time.deltaTime*-3,0),10*Time.deltaTime);// 飛機左繞


		} else if (flag2 == 2) {
			if ((rotationz <= 45 || rotationz >= 315)) {
				// 飛機向右傾斜
				m_transform.Rotate (new Vector3 (0, 0, (Time.deltaTime * rotateSpeed_AxisZ)), Space.Self);
			}
			// 飛機右轉
			m_transform.RotateAround (cube.transform.position,new Vector3(0,Time.deltaTime*3,0),10*Time.deltaTime);//飛機右繞
		} else {m_transform.RotateAround (cube.transform.position,new Vector3(0,0,0),0);
			BackToBlance();   //恢復平衡
			}

		if (flag3 == 1 && flag==0) {
			m_transform.Translate (new Vector3 (0, 0, speed / 8 * Time.deltaTime));//靠近
		} else if (flag3 == 2) {
			m_transform.Translate (new Vector3 (0, 0, speed / -8 * Time.deltaTime));
			flag = 0;
		} else {m_transform.Translate (new Vector3 (0, 0, 0));
		}

		if (flag4 == 1) {
			m_transform.RotateAround (cube.transform.position,new Vector3(0,Time.deltaTime*3,0),10*Time.deltaTime);//飛機加速右繞
		} else if (flag4 == 2) {
			m_transform.RotateAround (cube.transform.position,new Vector3(0,Time.deltaTime*-3,0),10*Time.deltaTime);//飛機加速左繞
		} else {m_transform.Translate (new Vector3 (0, 0, 0));
		}

		if (flag5 == 1)
		{
			//若是按下鍵盤的z則切換成第二部攝影機
			cam1.SetActive(false);

			cam2.SetActive(true);
		
		}
		else if (flag5 == 0)
		{
			//若是按下鍵盤的x則切換成第一部攝影機
			cam2.SetActive(false);

			cam1.SetActive(true);
	
		}
		if (flag6 == 1) {
		//初始化位置
		
			this.transform.position = v;
			this.transform.rotation = v2;
		
		}

	
		float joy1,AcX,AcZ,joy2;
		float button,z;
		if (ct.receiveMessage != null)//處理數據
		{	
			Debug.Log("Server:" + ct.receiveMessage);

			char[] delimiterChars = {','}; 
			string[] words = ct.receiveMessage.Split(delimiterChars);

			joy1 = float.Parse(words[0]);
			if (joy1 >= 485) {//上升
				flag1 = 1;
			} else if (joy1 <= 165) {
				flag1 = 2;
			} else {
				flag1 = 0;
			}

			AcX = float.Parse (words [1]);
			if (AcX > 0.3 || AcX < -0.3) {
				if (AcX > 0.3) {
					flag2 = 1;//向右轉 向左傾斜
				} else if (AcX < -0.3) {
					flag2=2;
				}	
			} else {flag2 = 0;
		}

			AcZ = float.Parse (words [2]);
			if (AcZ > 0.4 || AcZ < -0.4) {
				if (AcZ < -0.4) {
						
					flag3 = 1;

				} else if (AcZ > 0.4) {
					flag3 = 2;
				} 
			} else {flag3 = 0;
				
			}
			joy2 = float.Parse(words[3]);
			if (joy2 >= 485) {//上升
				flag4 = 1;
			} else if (joy2 <= 165) {
				flag4 = 2;
			} else {
				flag4 = 0;
			}

			button = float.Parse(words[4]);
			if (button==1) {
				flag5 = 1;//有按鈕按壓
			}  else {
				flag5 = 0;
			}

			z = float.Parse(words[5]);
			if (z==1) {
				flag6 = 1;//有初始按鈕按壓
			}  else {
				flag6 = 0;
			}

			ct.receiveMessage = null;
		}
		if (isSend == true)
			StartCoroutine(delaySend());

		ct.Receive();


	}
	void OnCollisionEnter (Collision aaa){
		if (aaa.gameObject.name == "Capsule") {
			Debug.Log ("發生撞擊,flag=1");
			flag = 1;
		}
	}
	private IEnumerator delaySend()
	{
		isSend = false;
		yield return new WaitForSeconds(1);
		ct.Send("Hello~ My name is Client");
		isSend = true;
	}

	private void OnApplicationQuit()
	{
		ct.StopConnect();
	}
	void BackToBlance()                 //恢复平衡方法
	{
		if ( rotationz <= 180) {       //當飛機為右傾斜
			if (rotationz - 0 <= 2) {   //飛機輕微晃動
				m_transform.Rotate (0, 0, Time.deltaTime * -1);
			} else {                      //快速恢復平衡
				m_transform.Rotate (0, 0, Time.deltaTime * -20);
			}
		}

		if ((rotationz > 180)) {        //當飛機為左傾斜
			if (360 - rotationz <= 2) { //飛機輕微晃動
				m_transform.Rotate(0, 0, Time.deltaTime * 1);
			}
			else {                      //快速恢復平衡
				m_transform.Rotate(0, 0, Time.deltaTime * 20);
			}
		}
		if ((rotationx <= 180)) {       //當飛機下旋
			if (rotationx - 0 <= 2) {
				m_transform.Rotate (Time.deltaTime * -1, 0, 0);
			} else {
				m_transform.Rotate (Time.deltaTime * -60, 0, 0);
			}
		}

		if ((rotationx > 180)) {        //當飛機上旋
			if (360 - rotationx <= 2) {
				m_transform.Rotate (Time.deltaTime * 1, 0, 0);
			} else {

				m_transform.Rotate (Time.deltaTime * 60, 0, 0);
			}
		}
	}
}