using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float moveSpeed = 5f;

    public float radius;

    GameObject focus;
    Config config;

    Bounds bounds;
    bool lastMouseValid = false;
    Vector3 lastMouse;


	// Use this for initialization
	void Start () {
        focus = GameObject.FindObjectOfType<CubeOfCubes>().gameObject;
        config = GameObject.FindObjectOfType<Config>();

        float guessedSize = config.sizeX * config.tileSize;

        Vector3 newCameraPosition = (Vector3.left + Vector3.up + Vector3.back*.75f) * (guessedSize + 5f);
        Camera.main.transform.position = newCameraPosition;
        radius = newCameraPosition.magnitude;
        Camera.main.transform.LookAt(transform.position);

        moveSpeed *= config.sizeX;


        // find the center of the object we are looking at
        bounds = new Bounds();
        foreach (Renderer r in focus.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(r.bounds);
        }

	}

	// Update is called once per frame
	void Update () {
        Vector3 axis = Vector3.zero;
        float moveSpeed = this.moveSpeed;

        bool up, down, left, right;
        up = down = left = right = false;

        if (Input.GetKey(KeyCode.W))
        {
            up = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }


        if (Input.GetKey(KeyCode.S))
        {
            down = true;
        }


        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }

        if (Input.GetMouseButton(1))
        {
            if (lastMouseValid)
            {
                Vector3 mouseMovement = Input.mousePosition - lastMouse;
                Debug.Log(mouseMovement);

                if (mouseMovement.x > 0)
                {
                    right = true;
                }

                if (mouseMovement.x < 0)
                {
                    left = true;
                }

                if (mouseMovement.y < 0)
                {
                    down = true;
                }

                if (mouseMovement.y > 0)
                {
                    up = true;
                }

                moveSpeed = Vector3.Magnitude(mouseMovement);
            }
            lastMouse = Input.mousePosition;
            lastMouseValid = true;
        }
        else
        {
            lastMouseValid = false;
        }

        if (up)
        {
            axis += transform.TransformDirection(Vector3.right);
        }

        if (left)
        {
            axis += Vector3.up;
        }


        if (down)
        {
            axis += transform.TransformDirection(Vector3.left);
        }


        if (right)
        {
            axis += Vector3.down;
        }


        axis = Vector3.ClampMagnitude(axis, 1);

        transform.RotateAround(bounds.center, axis, moveSpeed * Time.deltaTime);

        Camera.main.transform.LookAt(bounds.center);
	}
}
