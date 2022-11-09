using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance //singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraMovement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("CameraMovement");
                    instance = instanceContainer.AddComponent<CameraMovement>();
                }
            }
            return instance;
        }
    }
    private static CameraMovement instance;




    public GameObject Player;
    public float offsetY = 45f;
    public float offsetZ = -40f;
    Vector3 cameraPoition;

    private void LateUpdate() 
    {
        cameraPoition.y = Player.transform.position.y + offsetY;
        cameraPoition.z = Player.transform.position.z  + offsetZ;

        transform.position = cameraPoition;
    }
    public void CameraNextRoom()
    {
        cameraPoition.x = Player.transform.position.x;
    }
}
