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
    [SerializeField] float moveSpeed, _force;
    Rigidbody2D _rigi;
    State.PlayerState _playerState = State.PlayerState.idle;
    [SerializeField] bool _isGrounded, _isEnemy, _isDead;
    AnimationController _ani;
    Collider2D _colli;
    BoxCollider2D _box;
    [SerializeField] PhysicsMaterial2D _frictionMaterial;
    [SerializeField] PhysicsMaterial2D _noFrictionMaterial;
    void Start()
    {
        _rigi = this.GetComponent<Rigidbody2D>();
        _ani = this.GetComponentInChildren<AnimationController>();
        _colli = this.GetComponent<Collider2D>();
        _box = this.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(_isDead == true)
        {
            return;
        }
        MovingChar();
        Jump();
        CheckGround();
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
        _rigi.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, _rigi.linearVelocity.y);
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);

        }
    }
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
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
            }
            else
            {
                _playerState = State.PlayerState.idle;

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
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!collision.gameObject.CompareTag("Enemy")){

            return;
        }
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x < -0.5f)
            {
                Die();
                return;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) < 0.5f)
            {
                _colli.sharedMaterial = _noFrictionMaterial;
            }
        }
    }
    void CheckGround() {
        Vector2 origin = new Vector2(
            _box.bounds.center.x,
            _box.bounds.min.y);
        RaycastHit2D[] hit = new RaycastHit2D[1]; 
        _colli.Cast(Vector2.down, hit, 0.3f); 
        foreach (RaycastHit2D hits in hit) 
        { 
            if (hits.collider != null && hits.collider.CompareTag("Ground"))
            { 
                _isGrounded = true;
                _colli.sharedMaterial = _frictionMaterial;
                return; 
            } 
        } 
        _isGrounded = false;
        _colli.sharedMaterial =_noFrictionMaterial;

    }

}
