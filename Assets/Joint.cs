using UnityEngine;

public class Joint : MonoBehaviour
{
    [SerializeField] private bool followMouse;
    [SerializeField] private Camera camera;
    
    [Header("X Axis")]
    [SerializeField] private bool enableRotationX;
    [SerializeField] private float speedX;
    [SerializeField] [Range(-360, 360)] private float minRotationX;
    [SerializeField] [Range(-360, 360)] private float maxRotationX;
    
    [Header("Y Axis")]
    [SerializeField] private bool enableRotationY;
    [SerializeField] private float speedY;
    [SerializeField] [Range(-360, 360)] private float minRotationY;
    [SerializeField] [Range(-360, 360)] private float maxRotationY;
    
    [Header("Z Axis")]
    [SerializeField] private bool enableRotationZ;
    [SerializeField] private float speedZ;
    [SerializeField] [Range(-360, 360)] private float minRotationZ;
    [SerializeField] [Range(-360, 360)] private float maxRotationZ;

    private float xAngle;
    private float yAngle;
    private float zAngle;

    private readonly Vector3 xAxis = new Vector3(1, 0, 0);
    private readonly Vector3 yAxis = new Vector3(0, 1, 0);
    private readonly Vector3 zAxis = new Vector3(0, 0, 1);
    
    private Vector3 mouseWorldPosition;

    private void Start()
    {
        xAngle = (minRotationX + maxRotationX) / 2;
        yAngle = (minRotationY + maxRotationY) / 2;
        zAngle = (minRotationZ + maxRotationZ) / 2;
    }

    private void Update()
    {
        var tRotation = transform.rotation;
        
        var rotation = enableRotationX
            ? RotateOnAxis(ref xAngle, minRotationX, maxRotationX, ref speedX, xAxis)
            : Quaternion.AngleAxis(tRotation.x, xAxis);

        rotation *= enableRotationY
            ? RotateOnAxis(ref yAngle, minRotationY, maxRotationY, ref speedY, yAxis)
            : Quaternion.AngleAxis(tRotation.y, yAxis);

        rotation *= enableRotationZ
            ? RotateOnAxis(ref zAngle, minRotationZ, maxRotationZ, ref speedZ, zAxis)
            : Quaternion.AngleAxis(tRotation.z, zAxis);

        if (followMouse)
        {
            var mousePosition = Input.mousePosition;
            var pos = transform.position;
            
            mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(
                mousePosition.x,
                mousePosition.y,
                Vector3.Distance(camera.transform.position, pos) - 2)
            );


            var backward = pos - mouseWorldPosition;
            transform.localRotation = rotation * Quaternion.LookRotation(backward);
        }
        else
            transform.localRotation = rotation;
    }
    
    private static Quaternion RotateOnAxis(ref float angle, float minRotation, float maxRotation, ref float speed,
        Vector3 axis)
    {
        angle += speed * Time.deltaTime;

        if (angle > maxRotation)
        {
            speed *= -1;
            angle -= angle - maxRotation;
        }

        if (angle < minRotation)
        {
            speed *= -1;
            angle -= angle - minRotation;
        }

        return Quaternion.AngleAxis(angle, axis);
    }
}
