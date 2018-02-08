using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;


namespace VRTherapy {
public class VRTExplodableElement : MonoBehaviour //, IPointerClickHandler
	{



public bool searchBalloons = true;
public bool searchCandies = false; 

public GameObject soundObjectSelection;

/// Amplitude of floating motion for the balloon (meters).
private const float FLOATING_AMPLITUDE = 0.4f;

/// Period of floating motion for the balloon (seconds).
private const float FLOATING_PERIOD = 12.0f;

public bool ApplyFloatingAnimation = false;
public bool ApplyRotationAnimation = false;

/// Amount of time to grow the balloon before popping (seconds).
private const float POP_TIMER = 0.2f;

/// Growth of the balloon in scale per second.
private const float POP_SCALE = 0.8f;

/// Number of exploded quads to create
private const int NUM_EXPLODED_QUADS = 100;

/// Number of seconds for a balloon to fully appear.
private const float APPEARING_TIME = 0.5f;

private Vector3 startScale;
private Vector3 startPosition;
private float startTime;
//private ColorUtil.Type type;
private float appearingTimer = 0.0f;

private bool isAppearing = true;

public VRTSearchObjectSpawner spawner;
public int balloonIx;

public string SelectionType = "point";

public bool isExplodable = true;

public bool explodeObject = true;
public bool explodeMoveToArm = false;
public bool explodeDisappear = false;

public Color objectColor = Color.yellow;


private VRTConnect VRTConnect;

		public GameObject explodedQuad;
		public bool isSelected = false;
		public GameObject popSound;

		void Start()
		{

		  GameObject SocketIO = GameObject.Find("VRT Connect");
		  VRTConnect = (VRTConnect)SocketIO.GetComponent(typeof(VRTConnect));
						
			startPosition = transform.localPosition;
			startScale = transform.localScale;
			startTime = Time.realtimeSinceStartup + Random.Range(0.0f, FLOATING_PERIOD);
			transform.localScale = Vector3.zero;
			float randAngle = Random.Range(0.0f, 360.0f);
			transform.localRotation = Quaternion.Euler(0.0f, randAngle, 0.0f);
		
		}


		private void ApplyFloating()
		{
			float t = startTime - Time.realtimeSinceStartup;
			float w = (2 * Mathf.PI / FLOATING_PERIOD);
			float delta = Mathf.Sin(t * w) * FLOATING_AMPLITUDE;
			transform.localPosition = startPosition + Vector3.up * delta;
		}

		private void ApplyRotation()
		{

			float speed = 10;
			transform.Rotate(Vector3.back * Time.deltaTime * speed); 
			//transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);

		}


		bool lookTimerIsRunning = false; 
		float lookTime = 0f;
	
		void Update(){

			if (isAppearing)
			{
				float scale = Mathf.Min(appearingTimer / APPEARING_TIME, 1.0f);
				appearingTimer += Time.deltaTime;
				transform.localScale = startScale* scale;
				if (scale >= 1.0f){	isAppearing = false;}
				if (ApplyFloatingAnimation){ ApplyFloating();	}
				if (ApplyRotationAnimation){ ApplyRotation();}

			}else if (isSelected){

  			
				if (explodeObject){
                    Destroy(gameObject);
                    CreateExplosion();
				}else if (explodeMoveToArm) {
					if(searchBalloons){
					gameObject.GetComponent<Renderer>().material.DOColor(Color.green, 0.5f);
					}
					// FALL TO FLOOR
					Rigidbody RigidBody = gameObject.GetComponent<Rigidbody>();
					RigidBody.useGravity = true;
					RigidBody.isKinematic = false;
				}else {
					gameObject.transform.DOScale(0f, 0.5f);
				}
			}else {
				if (ApplyFloatingAnimation)	{ ApplyFloating();}
				if (ApplyRotationAnimation){  ApplyRotation();}
			}

			if (isExplodable){
				// LASER POINTER
				if (VRTConnect.selectedShootMode == 1){
				if (GvrControllerInput.ClickButtonDown && GvrPointerInputModule.CurrentRaycastResult.isValid){
	        		if (GvrPointerInputModule.CurrentRaycastResult.gameObject == gameObject){ ObjectSelected(); }
				}
				}

// HOVER OVER OBJECT WITH LASER POINTER
		/*		if (GvrPointerInputModule.CurrentRaycastResult.isValid) { // GvrControllerInput.ClickButtonDown &&  {
			Vector3 endPosition = GvrPointerInputModule.CurrentRaycastResult.worldPosition;
			Debug.Log ("GOT RAYCAST.");

			if (GvrPointerInputModule.CurrentRaycastResult.gameObject.tag == "ExplodableElement"){
			Debug.Log("Interactable object clicked, at point: " + GvrPointerInputModule.CurrentRaycastResult.worldPosition);
			}else{
			Debug.Log("Non-interactable object hovered, at point: " + GvrPointerInputModule.CurrentRaycastResult.gameObject.tag);
				// GvrPointerInputModule.CurrentRaycastResult.worldPosition +
			}
        } */
				
				// 3 SECONDS LOOK AT OBJECT
				if (VRTConnect.selectedShootMode == 2){
					 RaycastHit hit;
	 				if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity)) {
	         			if (hit.collider.gameObject == gameObject){
	         				if (lookTimerIsRunning) {
									if (lookTime <= 3f){ lookTime += Time.deltaTime;
									}else{ lookTime = 0f; lookTimerIsRunning = false; ObjectSelected();  }
							}else{
								lookTimerIsRunning = true;
								if(searchBalloons){ gameObject.GetComponent<Renderer>().material.DOColor(Color.green, 0.5f); }
							}
						}
	 				}else{
	 					if (lookTimerIsRunning) {
							lookTime = 0f; lookTimerIsRunning = false;
							if(searchBalloons){ gameObject.GetComponent<Renderer>().material.DOColor(objectColor, 0.5f); }
						}
						
	 				}
				}
			}
		}

		private void ObjectSelected(){
			if (isExplodable){
		    	VRTConnect.IncreaseScoreBy(1);
		    	
		    	if (VRTConnect.soundOn) {
		    	soundObjectSelection.transform.position = transform.position;
		    	soundObjectSelection.transform.rotation = Quaternion.identity;
		    	AudioSource audio = soundObjectSelection.GetComponent<AudioSource>();
        		audio.Play();
		    }

		    isSelected = true;
			}
		}

		private void CreateExplosion(){
			SphereCollider collider = GetComponent<SphereCollider>();
			Vector3 center = transform.localToWorldMatrix.MultiplyPoint3x4(collider.center);
			for (int i = 0; i < NUM_EXPLODED_QUADS; ++i){
				GameObject quad = Instantiate(explodedQuad);
				Vector3 delta = Random.onUnitSphere * 1.5f;
				float sx = Random.Range(0.1f, 0.5f);
				float sy = Random.Range(0.1f, 0.5f);
				quad.transform.position = center + delta;
				quad.transform.rotation = Quaternion.FromToRotation(Vector3.forward, delta);
				quad.transform.localScale = new Vector3(sx, sy, 1.0f);
				Rigidbody rigidBody = quad.GetComponent<Rigidbody>();
				float ax = Random.Range(-1.0f, 1.0f);
				float ay = Random.Range(-1.0f, 1.0f);
				float az = Random.Range(-1.0f, 1.0f);
				rigidBody.angularVelocity = new Vector3(ax, ay, az);
				rigidBody.velocity = delta * 3.0f;
		    }
		}

		void OnCollisionEnter(Collision collision){
			if (collision.gameObject.tag == "Shooter") {
				//Destroy(collision.gameObject);
				collision.gameObject.tag = "NotShootable";
                ObjectSelected(); 
      		 }
		}

	}
}
