using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
  	public class VRTExercise : MonoBehaviour {
 
		private bool explodeObject = true;
		private bool explodeMoveToArm = false;
		private bool explodeDisappear = false;
  
		public bool reloadExerciseFlag = false;
  		public VRTConnect VRTConnect;
		public GameObject[] exerciseObjects;
     	
  		void Start(){
 			reloadExercise();
 		}  	

		public void reloadExercise(){
			reloadExerciseFlag = true;
		}

		void Update(){

			explodeObject = VRTConnect.explodeObject;
			explodeMoveToArm = VRTConnect.explodeMoveToArm;
			explodeDisappear = VRTConnect.explodeDisappear;

  			if (reloadExerciseFlag) {
 				reloadExerciseFlag = false;
  				loadExercise();
 			}   		  		 	
		}

		private void loadExercise(){
			
			/// DESTROY/ REMOVE ALL CURRENT OBJECTS             
			resetExercise();
			
			/// IMPORTANT: ADD ALL CREATED OBJECT FOR THIS EXERCISE INTO THE ARRAY exerciseObjects in order to remove them when the exercise will be reloaded by the app.
			// 
   		
      	}   

		public void resetExercise() {
  			if (exerciseObjects != null) {
 				for (int i = 0;i<exerciseObjects.Length;++i) {
  					Destroy(exerciseObjects[i]);
  				}   
			}  			
			exerciseObjects = null;
   		}

		  
	}
