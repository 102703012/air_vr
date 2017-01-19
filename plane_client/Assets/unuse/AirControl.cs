using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;


public class AirControl : MonoBehaviour
{
	SerialPort serial = new SerialPort ("/dev/cu.usbmodem1461", 9600);

	/*以上六軸感測器設定*/
	public GameObject cube;
	private Transform m_transform;          //保存transform實例 
	public float speed = 50f;      //飛機的飛行速度
	private float rotationz = 0.0f;         //繞Ｚ軸的旋轉量
	private float rotationx = 0.0f;
	public float rotateSpeed_AxisX = 45f;
	public float rotateSpeed_AxisZ = 45f;   //绕Z轴的旋轉速度
	public float rotateSpeed_AxisY = 20f;   //绕Y轴的旋轉速度

	private int flag=0;

	public GameObject bullet;
	public Transform bullet_pos;

	// Use this for initialization
	void Start()
	{
		m_transform = this.transform;       //赋值，減少外部代碼的調用       
		this.gameObject.GetComponent<Rigidbody>().useGravity = false; //默認不受重力影響

		/*以下讀取設定*/
		serial.Open();
		serial.ReadTimeout = 2000;
		//鎖定使用者
		transform.LookAt(cube.transform.position);
	}

	// Update is called once per frame
	void Update()
	{
		
		/*cube.transform.position.y = m_transform.position.y;
		transform.LookAt(cube.transform);*/

		if (!serial.IsOpen)

			serial.Open();



		serial.Write("a");
		float AcX = float.Parse(serial.ReadLine());
		/*serial.Write("b");
		float AcY = float.Parse(serial.ReadLine());*/
		serial.Write("c");
		float AcZ = float.Parse(serial.ReadLine());
		/*serial.Write("d");
		float GyX = float.Parse(serial.ReadLine());
		serial.Write("e");
		float GyY = float.Parse(serial.ReadLine());
		serial.Write("f");
		float GyZ = float.Parse(serial.ReadLine());*/
		serial.Write("g");
		int button = int.Parse(serial.ReadLine());
		serial.Write ("h");
		int cam = int.Parse(serial.ReadLine());
		/*以上六軸讀取*/
		//m_transform.Translate(new Vector3(0, 0, speed / 20 * Time.deltaTime));//向前移動
		// 尋找到名稱為“propeller”的對象並使其繞y軸旋轉
		GameObject.Find("propeller").transform.Rotate(new Vector3(0, 300f, 0));



		// 獲取飛機對象繞Ｘ軸的旋轉量
		rotationz = this.transform.eulerAngles.z;
		rotationx = this.transform.eulerAngles.x;

		//拍照按鈕
		if (cam == 1) {
		}

		if (button >= 400) {
			m_transform.Translate (new Vector3 (0, speed / 5 * Time.deltaTime, 0));
			//Instantiate(bullet,bullet_pos.position,transform.rotation);//根據父物件的角度
			//if (button == 1) {Instantiate(bullet,bullet_pos.position,Quaternion.identity);
		} else if (button <= 260) {
			m_transform.Translate (new Vector3 (0, speed / -5 * Time.deltaTime, 0));


		}	if (serial.IsOpen) {
			if (AcX > 0.3 || AcX < -0.3) {
				if (AcX > 0.3) {
					if ((rotationz <= 45 || rotationz >= 315)) {
						// 飛機向左傾斜
						m_transform.Rotate (new Vector3 (0, 0, (Time.deltaTime * rotateSpeed_AxisZ)), Space.Self);
					}
					m_transform.RotateAround (cube.transform.position,new Vector3(0,Time.deltaTime*-30,0),100*Time.deltaTime);// 飛機左轉
					//m_transform.Rotate (new Vector3 (0, -Time.deltaTime * 30, 0), Space.World);
				} else if (AcX < -0.3) {
					if ((rotationz <= 45 || rotationz >= 315)) {
						// 飛機向右傾斜
						m_transform.Rotate (new Vector3 (0, 0, (Time.deltaTime * -rotateSpeed_AxisZ)), Space.Self);
					}
					// 飛機右轉
					m_transform.RotateAround (cube.transform.position,new Vector3(0,Time.deltaTime*30,0),100*Time.deltaTime);
				}	
			} else {
				BackToBlance();}
			if (AcZ > 0.4 || AcZ < -0.4) {
				
				if (AcZ < -0.4&& flag == 0) {//靠近
						
					m_transform.Translate(new Vector3(0, 0, speed /8 * Time.deltaTime));

				
				} else if (AcZ > 0.4 ) {//遠離
					
					m_transform.Translate(new Vector3(0, 0, speed / -8 * Time.deltaTime));
					flag = 0;
				} 
			} else {
				BackToBlance ();
			}
		}
	}

	void OnCollisionEnter (Collision aaa){
		if (aaa.gameObject.name == "Capsule") {
			Debug.Log ("發生撞擊,flag=1");
			flag = 1;
		}
	}

	void BackToBlance()                 //恢复平衡方法
	{
		if ((rotationz <= 180)) {       //當飛機為右傾斜
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
		if ((rotationx <= 180)) {       //當飛機上旋
			if (rotationx - 0 <= 2) {
				m_transform.Rotate (Time.deltaTime * -1, 0, 0);
			} else {
				m_transform.Rotate (Time.deltaTime * -20, 0, 0);
			}
		}

		if ((rotationx > 180)) {        //當飛機下旋
			if (360 - rotationx <= 2) {
				m_transform.Rotate (Time.deltaTime * 1, 0, 0);
			} else {
			                 
				m_transform.Rotate (Time.deltaTime * 20, 0, 0);
			}
		}
	}
}
