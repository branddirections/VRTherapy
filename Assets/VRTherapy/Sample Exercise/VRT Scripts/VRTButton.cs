using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTButton : MonoBehaviour {

	    public int exerciseId;
		public TextMesh textMesh;
		private static bool started = false;
 

		void Start(){
		}

		void Update(){
		if (started){  textMesh.text = "Stop Exercise";	}
		else {  textMesh.text = "Play without App";}

		// LASER POINTER
			if (GvrControllerInput.ClickButtonUp && GvrPointerInputModule.CurrentRaycastResult.isValid){
        	if (GvrPointerInputModule.CurrentRaycastResult.gameObject == gameObject){
			Debug.Log("CLICKED ON EXPLODABLE OBJECT, at point: " + GvrPointerInputModule.CurrentRaycastResult.worldPosition);
			buttonPressed();
			}
			}
			
		}

		void OnCollisionEnter(Collision collision){
			buttonPressed();
		}

		void buttonPressed(){
			VRTConnect VRTConnect = (VRTConnect)GameObject.Find("VRT Connect").GetComponent(typeof(VRTConnect));
			
			if(!VRTConnect.isChangingGameState){
		if (started){ 
			VRTConnect.StopWithoutApp(exerciseId);
			started = false;
		}else{
			VRTConnect.PlayWithoutApp(exerciseId);
			started = true;
		}
		}
		}
}
