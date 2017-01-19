using UnityEngine;
using System.Collections;

/*

//public Camera camera;
private void CaptureCamera(Camera camera, Rect rect,string fileName) {
	// 創建一個RenderTexture對象
	RenderTexture rt = newRenderTexture((int)rect.width, (int)rect.height,0);

	// 臨時設置相關相機的targetTexture為rt, 並手動渲染該相機
	camera.targetTexture = rt; camera.Render;

	// 激活這個rt, 並從中中讀取像素。
	RenderTexture.active = rt;

	//一定要激活，不然讀取不到像素的
	Texture2D screenShot =newTexture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false); screenShot.ReadPixels(rect,00);

	// 這個時候，它是從RenderTexture.active中讀取像素
	screenShot.Apply; 
	GameObject.Destroy(rt);

	// 重置相關參數，以使camera繼續在螢幕上顯示
	camera.targetTexture =null; 
	RenderTexture.active =null;

	// 最後將這些紋理數據轉換成一個png圖片文件並保存
	byte bytes = screenShot.EncodeToPNG; 
	Destroy (screenShot); 
	System.IO.File.WriteAllBytes(fileName, bytes); 
	Debug.Log("截屏了一張圖片:"+fileName); }

	*/