using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbit : MonoBehaviour {

    public Vector3 target = Vector3.zero;

    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float zSpeed = 120.0f;

    public float keySpeed = 0.5f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start() {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null) {
            rigidbody.freezeRotation = true;
        }
    }

    void LateUpdate() {
        // Don't move unless asked to.

        float moveX = 0f, moveY = 0f;
        if (Input.GetMouseButton(1)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            moveX = Input.GetAxis("Mouse X");
            moveY = Input.GetAxis("Mouse Y");
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (Input.GetKey(KeyCode.W)) {
                moveY = -keySpeed;
            }
            if (Input.GetKey(KeyCode.A)) {
                moveX = keySpeed;
            }
            if (Input.GetKey(KeyCode.S)) {
                moveY = keySpeed;
            }
            if (Input.GetKey(KeyCode.D)) {
                moveX = -keySpeed;
            }

            if (moveX != 0f && moveY != 0f) {
                Vector2 clamps = new Vector2(moveX, moveY);
                Vector2 magnitude = Vector2.ClampMagnitude(clamps, keySpeed);
                moveX = magnitude.x;
                moveY = magnitude.y;
            }

        }

        x += moveX * xSpeed * distance * Time.deltaTime;
        y -= moveY * ySpeed * distance * Time.deltaTime;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zSpeed, distanceMin, distanceMax);

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
