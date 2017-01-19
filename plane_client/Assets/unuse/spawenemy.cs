using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawenemy : MonoBehaviour {
	public GameObject enemy;//裝敵人的模型
	Vector3 enemy_pos;//出現的位置
	float spawntime =5;
	float spawnspeed=3;
	// Use this for initialization
	void Start () {
		
		enemy_pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		enemy_pos.x = Random.Range (-15, 15);
		if (Time.time > spawntime) {
			Instantiate (enemy, enemy_pos,transform.rotation);
			spawnspeed = spawnspeed * 0.9f;
			spawntime = Time.time + spawnspeed;
		
		}
	}
}
