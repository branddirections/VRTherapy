using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 namespace VRTherapy {
  	public class VRTSearchObjectSpawner : MonoBehaviour {
 
		public int AmountOfDistractionObjects = 20;
 		public int AmountOfExplodableObjects = 10;
 	 	
		public bool searchBalloons = true;
  		public bool searchCandies = false;

   		private bool explodeObject = true;
		private bool explodeMoveToArm = false;
		private bool explodeDisappear = false;

		public GameObject soundObjectSelection;
   		public GameObject soundObjectHover;
   		public GameObject soundObjectNotSelectable;

		public bool ApplyRotationAnimation = true;
  		public bool ApplyFloatingAnimation = true;
   		[Range(-45,180)] 		
 		public int LeftAngle = 150;
  		[Range(-45,180)] 		
		public int RightAngle = 150;
  		[Range(-45,180)] 		
		public int TopAngle = 70;
  		[Range(-45,180)] 		
		public int BottomAngle = 10;
 	 	[Range(5.0f, 25.0f)]     	
		public float MIN_SPAWN_RADIUS = 8.0f;
  		[Range(5.0f, 25.0f)]     	
		public float MAX_SPAWN_RADIUS = 16.0f;
     	 //public float MIN_SPAWN_HEIGHT = 0.0f;
     	//public  float MAX_SPAWN_HEIGHT = 10.0f;
     	public  float MIN_DIST_BETWEEN_OBJECTS = 1.5f;
     		public string SelectionType = "point";
 		public GameObject[] exerciseObjects;
     	public GameObject[] shootableElements;
     	public GameObject[] shootableElementsBalloons;
 		public GameObject[] shootableElementsCandies;
  		public bool reloadExerciseFlag = false;
		//public bool isReloadingExercise = false; 
  	
  		public GameObject QuadYellow;

		public GameObject QuadBlue;
 		public GameObject QuadMagenta;
 		public bool findMagentaObjects = true;
  	 	public bool findYellowObjects = true;
   		public bool findBlueObjects = true;
    		public VRTConnect VRTConnect;
  		
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
			//	isReloadingExercise = true; 
 				reloadExerciseFlag = false;
  				createExercise();
 			}   

/*
			if (!isReloadingExercise ) {
				

				int obj = 0;
				

				for (int i = 0;i < (AmountOfDistractionObjects+AmountOfExplodableObjects);++i){
				if (exerciseObjects[i]){
					VRTExplodableElement VRTExplodableElement = exerciseObjects[i].GetComponent(typeof(VRTExplodableElement)) as VRTExplodableElement;
					if (VRTExplodableElement.isExplodable == true){ obj += 1; }
				}}

				if (obj < 1){
				Debug.Log("No more shootable Elements. Stop Game.");
				VRTConnect.StopWithoutApp(VRTConnect.exerciseId);
				}	 
			}
			*/

	  		 	
		}

		public void resetExercise() {
  			/// DESTROY/ REMOVE ALL CURRENT OBJECTS 			
			if (exerciseObjects != null) {
 				for (int i = 0;i<exerciseObjects.Length;++i) {
  					Destroy(exerciseObjects[i]);
  				}   
			}  			
			exerciseObjects = null;
   		}

		private void createExercise(){
			/// DEFINE THE SHOOT ELEMENTS - BALLOONS OR CANDIES             
			if (searchCandies) { shootableElements = shootableElementsCandies; ApplyRotationAnimation = true;  }
			else { shootableElements = shootableElementsBalloons;	ApplyRotationAnimation = false; }

			/// DESTROY/ REMOVE ALL CURRENT OBJECTS             
			resetExercise();
			
   			/// CREATE ARRAY WITH SUM OF EMPTY DISTRACTINO AND EXPLODABLE OBJECTS 			
			int totalObjects = AmountOfExplodableObjects+AmountOfDistractionObjects;
 			exerciseObjects = new GameObject[totalObjects];
 			/// SPAWN OBJECTS 			
			for (int i = 0;i<totalObjects;++i) {
				if (i < AmountOfExplodableObjects){ SpawnObject(i, true);
				}else { SpawnObject(i, false);}
			}  	

			//isReloadingExercise = false; 


		}  		

			private bool IsTooClose(Vector3 position){
				for (int i = 0;i < (AmountOfDistractionObjects+AmountOfExplodableObjects);++i){
				if (exerciseObjects[i]){
					float dist = Vector3.Distance(exerciseObjects[i].transform.position, position);
					if (dist < MIN_DIST_BETWEEN_OBJECTS){ 
						return true; 
					}
					
				}}		
				return false;
    			}

   			
   			private Vector3 GetObjectPosition(){
   			  for (int i = 0;i < 100;++i){
   			  MIN_DIST_BETWEEN_OBJECTS = 1.5f;
   			  float spawnRadius = Random.Range(MIN_SPAWN_RADIUS, MAX_SPAWN_RADIUS);
 		      // float spawnHeight = Random.Range(MIN_SPAWN_HEIGHT, MAX_SPAWN_HEIGHT);
 			  float spawnAngleLeftRight = Random.Range((((RightAngle)*-1)+90)*Mathf.Deg2Rad, ((LeftAngle)+90)*Mathf.Deg2Rad);
 			  float spawnAngleTopBottom = Random.Range((BottomAngle*-1)*Mathf.Deg2Rad, TopAngle*Mathf.Deg2Rad);
 		      float spawnX = spawnRadius * Mathf.Cos(spawnAngleLeftRight);
 			  float spawnY = spawnRadius * Mathf.Sin(spawnAngleTopBottom);
 			  //float spawnZ = spawnRadius * Mathf.Sin(spawnHeight);
 			  float spawnZ = spawnRadius * Mathf.Sin(spawnAngleLeftRight);
 		      Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

 					if (!IsTooClose(spawnPosition)){
			        	return spawnPosition;
			        }

			        if (i == 30) {
			        	MIN_DIST_BETWEEN_OBJECTS -= 0.1f;
			        	LeftAngle += 2;
						RightAngle += 2;
				    }

				    if (i == 50) {
			        	MIN_DIST_BETWEEN_OBJECTS -= 0.1f;
			        	LeftAngle += 2;
						RightAngle += 2;
				    }

				    if (i == 70) {
			        	MIN_DIST_BETWEEN_OBJECTS -= 0.1f;
			        	LeftAngle += 2;
						RightAngle += 2;
				    }
				
			  }
			  
			  return new Vector3();

   			}

   	  		private void SpawnObject(int objectIx, bool isExplodable) {
   			  
   			Vector3 spawnPosition = GetObjectPosition();
 			// Define which color is explodable and which does not react and seperate between shootable object and distraction objects 			
 			List<GameObject> DistractionObjects = new List<GameObject>();
 			GameObject ShootableObject = shootableElements[0];
 
			    if (findBlueObjects) { ShootableObject = shootableElements[0]; } 
			    else { DistractionObjects.Add(shootableElements[0]); }
				if (findMagentaObjects) { ShootableObject = shootableElements[1]; } 
				else { DistractionObjects.Add(shootableElements[1]); }
				if (findYellowObjects) { ShootableObject = shootableElements[2];  } 
				else { DistractionObjects.Add(shootableElements[2]);}  			 

				if (!isExplodable){
				// Choose a random distraction object from the ones available in the array if it is not shootable
				ShootableObject = DistractionObjects[Random.Range(0, (DistractionObjects.Count))];
 				}

			// Create the Playable Game Object 			
			GameObject newObject = Instantiate(ShootableObject);
 			newObject.transform.position = spawnPosition;
 
			// Add the explodable element component to this object
			VRTExplodableElement VRTExplodableElement = newObject.AddComponent(typeof(VRTExplodableElement)) as VRTExplodableElement;

			if (findBlueObjects) { VRTExplodableElement.objectColor = Color.blue; } 
		    if (findMagentaObjects) { VRTExplodableElement.objectColor = Color.magenta;  } 
			if (findYellowObjects) { VRTExplodableElement.objectColor = Color.yellow;  } 
				

			VRTExplodableElement.searchBalloons = searchBalloons;
 			VRTExplodableElement.searchCandies = searchCandies;
 			 VRTExplodableElement.spawner = this;
 			VRTExplodableElement.balloonIx = objectIx;
 			VRTExplodableElement.ApplyFloatingAnimation = ApplyFloatingAnimation;
 			VRTExplodableElement.ApplyRotationAnimation = ApplyRotationAnimation;
 			newObject.transform.parent = gameObject.transform;
	  		Rigidbody gameObjectsRigidBody = newObject.AddComponent<Rigidbody>();
 			// Add the rigidbody. 				
			gameObjectsRigidBody.mass = 1;
 			// Set the GO's mass to 5 via the Rigidbody. 				
				gameObjectsRigidBody.drag = 0;
 				gameObjectsRigidBody.angularDrag = 0.05f;
 				gameObjectsRigidBody.useGravity = false;
 				gameObjectsRigidBody.isKinematic = true;
  				SphereCollider m_Collider = newObject.AddComponent<SphereCollider>();

				m_Collider.center = Vector3.zero;
 			// the center must be in local coordinates 				
				m_Collider.radius = 1;
 
				VRTExplodableElement.isExplodable = isExplodable;
				VRTExplodableElement.explodeObject = explodeObject;
				VRTExplodableElement.explodeDisappear = explodeDisappear;
				VRTExplodableElement.explodeMoveToArm = explodeMoveToArm;

				VRTExplodableElement.soundObjectSelection = Instantiate(soundObjectSelection);
   		
  				if (findBlueObjects) { VRTExplodableElement.explodedQuad = QuadYellow; } 				
				else if (findMagentaObjects) { VRTExplodableElement.explodedQuad = QuadMagenta; } 
				else { VRTExplodableElement.explodedQuad = QuadBlue; } 

 						
			exerciseObjects[objectIx] = newObject;

      	}     
	}
} 