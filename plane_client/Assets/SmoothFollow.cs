using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public GameObject cube;
	private GameObject target;          //所要跟隨目標的對象
	public float distance = -2.0f;      //與目標對象的距離
	public float height = 0.0f;         //與目標對象的高度差
	public float heightDamping = 2.0f;  //高度變化中的阻尼參數
	public float rotationDamping = 3.0f;//繞y軸的旋轉中的阻尼參數

	void Start()
	{
		// 尋找標籤為“airplane”的遊戲對象並設置為要跟隨的目標對象
		target = GameObject.FindWithTag("AirPlane");
	}
	void LateUpdate()
	{ 	// 如果目標不在對象內跳出方法
		if (!target)
			return;

		// 攝影機期望的旋轉角度計高度
		float wantedRotationAngle = target.transform.eulerAngles.y;
		float wantedHeight = target.transform.position.y + height;

		// 攝影機當前的旋轉角度及高度
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		// 計算攝影機繞y軸變化的阻尼
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// 計算攝影機高度變化中的阻尼
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// 轉換成旋轉角度
		var currentRotation =Quaternion.Euler(0, currentRotationAngle, 0) ;

		// 攝影機距離目標背後的距離
		transform.position = target.transform.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		// 設置攝影機的高度
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);;

		// 攝影機一直注視目標
		transform.LookAt(cube.transform.position);
	}
}
