using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

public class GameEntry : MonoBehaviour
{
    private float speed = 5f;

    private void Start()
    {
        GameObject cubeObject = GameObject.Find("Cube");

        if (cubeObject != null)
        {
            CubeRotation cubeRotation = cubeObject.AddComponent<CubeRotation>();
            cubeRotation.rotationSpeed = 10f;
        }
        else
        {
            Debug.LogError("Cube GameObject not found!");
        }
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        MovePlayer(moveHorizontal, moveVertical);

        Debug.LogFormat("Horizontal: {0}, Vertical: {1}", moveHorizontal, moveVertical);
    }

    private void MovePlayer(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
