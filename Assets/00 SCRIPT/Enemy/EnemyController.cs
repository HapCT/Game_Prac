
using NUnit;
using NUnit.Framework;
using UnityEngine;
using static State;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Vector2 _start, _end;
    [SerializeField] float _speed, _force;
    Rigidbody2D _rigi;
    Vector2[] _points;
    int _direction = 1;
    int _currentPoint = 0;
    AnimationController _ani;
    BoxCollider2D _box;
    State.EnemyState enemy = State.EnemyState.idle;
    bool _enemy, _isDead;

    void Start()
    {
        _ani = this.GetComponentInChildren<AnimationController>();
        _rigi = this.GetComponent<Rigidbody2D>();
        _box = this.GetComponent<BoxCollider2D>();
        _points = new Vector2[]
         {
             _start,
             _end
         };
            
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead == true)
        { return; }
            
        this.transform.position = Vector2.MoveTowards(this.transform.position, _points[_currentPoint], _speed * Time.deltaTime);
        if(Vector2.Distance(this.transform.position, _points[_currentPoint]) < 0.5f)
        {
            _currentPoint += _direction;
            if (_currentPoint == _points.Length - 1)
            {
                _direction = -1;
                this.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_currentPoint == 0)
            {
                _direction = 1;
                this.transform.localScale = new Vector3(1, 1, 1);

            }
        }
        UpdateAni();
        _ani.UpdateAnimationEnemy(enemy);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_start, _end);
    }
    void UpdateAni()
    {

        if (_currentPoint != _points.Length)
        {
            enemy = State.EnemyState.run;
        }
        else
        {
            enemy = State.EnemyState.idle;
        }

        
    }
    void DieJump()
    {
        _rigi.linearVelocity = new Vector2(0, _force);
        _rigi.freezeRotation = false ;
        _rigi.angularVelocity = 500f;

        Invoke(nameof(ShowOver), 2f);
    }
    
    void Die()
    {
        _isDead = true;
        _box.enabled = false;
        DieJump();

    }
    void ShowOver()
    {
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                Die();
                return;
            }    
        }
    }
}
