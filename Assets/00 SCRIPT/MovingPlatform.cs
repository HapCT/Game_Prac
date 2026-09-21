using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector2 start, end;
    [SerializeField] float speed;
    Vector2 target;
   Vector2[] _points;

    void Start()
    {
       target = start;
    }

    void Update()
    {
        this.transform.position =Vector2.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(this.transform.position, target) <= 0.5f)
        {
            target =target.Equals(start) ? end : start;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);
    }
}