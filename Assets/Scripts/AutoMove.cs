using UnityEngine;
using System.Collections;

public enum Direction
{
    Left = 0,
    Top,
    Right,
    Bottom,
    Front,
    Back       
};

public class AutoMove : MonoBehaviour 
{
    public float moveSpeed = 2.0f;
    public Direction direction = Direction.Top;

    void Update()
    {
        Vector3 v = transform.position;
        switch (direction)
        {
            case Direction.Left:
                v.x += moveSpeed * Time.deltaTime;
                break;
            case Direction.Top:
                v.y += moveSpeed * Time.deltaTime;
                break;
            case Direction.Right:
                v.x -= moveSpeed * Time.deltaTime;
                break;
            case Direction.Bottom:
                v.y -= moveSpeed * Time.deltaTime;
                break;
            case Direction.Front:
                v.z += moveSpeed * Time.deltaTime;
                break;
            case Direction.Back:
                v.z -= moveSpeed * Time.deltaTime;
                break;
        }
        transform.position = v; 
    }

}
