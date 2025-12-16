using Unity.VisualScripting;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int i = 0;
    private float speed = 2f;
    void Update()
    {
        if (Vector2.Distance(transform.position, waypoints[i].transform.position) < 0.1)
        {
            if (i >= waypoints.Length-1)
            {
                i = 0;
            }
            else
            {
                i++;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[i].transform. position, Time.deltaTime * speed);
    }
}
