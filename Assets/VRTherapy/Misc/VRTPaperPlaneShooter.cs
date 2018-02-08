using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTPaperPlaneShooter : MonoBehaviour {

	private const float MIN_TIMEOUT = 0.36f;
private float timeOut = MIN_TIMEOUT;
private Vector3 startScale;
private Vector3 startLocalPosition;
public GameObject rigidPaperPlane;


	// Use this for initialization
	void Start () {
			startScale = transform.localScale;
		startLocalPosition = transform.localPosition;

	}
	
	// Update is called once per frame
	void Update () {

		if (GvrControllerInput.ClickButtonDown && timeOut >= MIN_TIMEOUT)
			{
				GameObject newPaperPlane = Instantiate(rigidPaperPlane);
newPaperPlane.transform.position = transform.position;
				newPaperPlane.transform.rotation = transform.rotation;
				Rigidbody rigidBody = newPaperPlane.GetComponent<Rigidbody>();
rigidBody.velocity =  transform.rotation* Vector3.forward * 20.0f;
timeOut = 0.0f;
			}

			float scale = Mathf.Clamp(2.0f * timeOut / MIN_TIMEOUT - 1.0f, 0.0f, 1.0f);
transform.localScale = startScale* scale;
transform.localPosition = startLocalPosition + Vector3.forward* (1.0f - scale) * 0.1f;
			timeOut += Time.deltaTime;
		
	}
}
