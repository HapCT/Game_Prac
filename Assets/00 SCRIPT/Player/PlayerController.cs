using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed, _force, _isSlopeAngle = 0;
    Rigidbody2D _rigi;
    State.PlayerState _playerState = State.PlayerState.idle;
    [SerializeField] bool _isGrounded, _isEnemy, _isDead, _isSlope;
    AnimationController _ani;

    BoxCollider2D _box;
    [SerializeField] PhysicsMaterial2D _frictionMaterial;
    [SerializeField] PhysicsMaterial2D _noFrictionMaterial;
    void Start()
    {
        _rigi = this.GetComponent<Rigidbody2D>();
        _ani = this.GetComponentInChildren<AnimationController>();
        _box = this.GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        if (_isDead == true)
        {
            return;
        }
        CheckGround();
        Jump();
        MovingChar();
        UpdateAni();

        Debug.DrawRay(new Vector2(_box.bounds.center.x, _box.bounds.min.y), Vector2.down * 0.3f, Color.red);
        _ani.UpdateAnimation(_playerState);

    }
    public void ResetPlayer()
    {
        CancelInvoke(nameof(ShowOver));
        _isDead = false;
        _rigi.linearVelocity = Vector2.zero;
        _box.enabled = true;
    }
    void MovingChar()
    {
        Vector2 movement = _rigi.linearVelocity;

        if (!_isSlope)
        {
            movement.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }
        else
        {
            float input = Input.GetAxisRaw("Horizontal");

            movement.x = Mathf.Cos(_isSlopeAngle * Mathf.Deg2Rad) * moveSpeed * input;

            if (input != 0)
            {
                movement.y = Mathf.Sin(_isSlopeAngle * Mathf.Deg2Rad) * moveSpeed * input;
            }

        }

        _rigi.linearVelocity = movement;



        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Vector2 scale = this.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            this.transform.localScale = scale;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            Vector2 scale = this.transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            this.transform.localScale = scale;
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigi.AddForce(new Vector2(0, _force));
            _isGrounded = false;

        }
    }
    void UpdateAni()
    {
        if (_isGrounded == false)
        {
            _playerState = State.PlayerState.jump;
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                _playerState = State.PlayerState.run;
                _rigi.sharedMaterial = _noFrictionMaterial;
            }
            else
            {
                _playerState = State.PlayerState.idle;

                if (_isSlope)
                {
                    _rigi.sharedMaterial = _frictionMaterial;
                }
                else
                {
                    _rigi.sharedMaterial = _frictionMaterial;
                }
            }

        }
    }
    void ShowOver()
    {
        gameObject.SetActive(false);

    }
    void Die()
    {

        _isDead = true;
        _box.enabled = false;
        _rigi.AddForce(new Vector2(0, _force));
        Invoke(nameof(ShowOver), 2f);
        SceneManager.LoadScene("Restart", LoadSceneMode.Additive);


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("MovingPlatform"))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Debug để kiểm tra giá trị normal thực tế
            Debug.Log($"Contact normal: {contact.normal}");

            if (contact.normal.y > 0.01f && Mathf.Abs(contact.normal.x) > 0.01f)
            {
                _isSlope = true;
                _isSlopeAngle = Mathf.Atan2(contact.normal.x, contact.normal.y) * Mathf.Rad2Deg;
                return;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                return;
            }

            Die();
            return;
        }
    }


    void CheckGround()
    {
        
        Vector2 origin = new Vector2(_box.bounds.center.x, _box.bounds.min.y);
        RaycastHit2D[] hit = new RaycastHit2D[1];
        _box.Cast(Vector2.down, hit, 0.3f);
        foreach (RaycastHit2D hits in hit)
        {

            Debug.DrawRay(hits.point, hits.normal, Color.yellow);
            if (hits.collider == null)
            {
                _isGrounded = false;
                _isSlope = false;
                _isSlopeAngle = 0;
                this.transform.SetParent(null);
                return;
            }
            if (hits.normal.y > 0.01 && Mathf.Abs(hits.normal.x) > 0.01)
            {
                _isSlope = true;
                _isSlopeAngle = Mathf.Atan2(-hits.normal.x, hits.normal.y) * Mathf.Rad2Deg;
            }   
            else
            {
                _isSlope = false;
                _isSlopeAngle = 0;
            }
            if (hits.collider.CompareTag("Ground"))
            {
                _isGrounded = true;
                transform.SetParent(null);

                return;
            }
            if (hits.collider.CompareTag("MovingPlatform"))
            {
                this.transform.SetParent(hits.collider.transform);
                _isGrounded = true;

                return;
            }

        }
        _isSlope = false;
        _isSlopeAngle = 0;
        _isGrounded = false;
        transform.SetParent(null);


    }

}
