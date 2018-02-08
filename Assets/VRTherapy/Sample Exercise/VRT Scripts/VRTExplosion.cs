using UnityEngine;

public class VRTExplosion : MonoBehaviour {


		private const float LIFE_TIME = 2.0f;
		private const float GRAVITY_FORCE = 1.0f;

		private Rigidbody rigidBody;
		private float startTime;

		void Awake()
		{
			rigidBody = GetComponent<Rigidbody>();
			startTime = Time.realtimeSinceStartup;
		}

		void Update()
		{
			rigidBody.AddForce(Vector3.down * GRAVITY_FORCE);
			float t = (Time.realtimeSinceStartup - startTime) / LIFE_TIME;
			transform.localScale *= 0.95f;
			if (t >= 1.0f)
			{
				Destroy(gameObject);
			}
		}

}
