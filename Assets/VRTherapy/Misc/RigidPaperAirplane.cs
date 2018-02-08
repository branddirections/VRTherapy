

using UnityEngine;


  public class RigidPaperAirplane : MonoBehaviour {
    private const float killHeight = -10.0f;

    private Rigidbody rigidBody;
    public bool isSpinning = false;
  
    void Start() {
      rigidBody = GetComponent<Rigidbody>();
      rigidBody.maxAngularVelocity = 0.0f;
      rigidBody.freezeRotation = true;
    }

    void Update () {
      // Remove game object when it falls under the floor.
      if (transform.position.y < killHeight) {
        Destroy(gameObject);
      }

      // Always point in the direction of motion.
      if (!isSpinning) {
        Vector3 forward = transform.rotation * Vector3.forward;
        Quaternion rotation = Quaternion.FromToRotation(forward, rigidBody.velocity);
        transform.rotation = rotation * transform.rotation;
      }
    }

    public void Spin() {
      if (!isSpinning) {
        rigidBody.freezeRotation = false;
        rigidBody.maxAngularVelocity = 8.0f;
        rigidBody.angularVelocity = Random.onUnitSphere * 8.0f;
        isSpinning = true;
      }
    }
}

