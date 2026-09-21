using UnityEngine;
public class AnimationController : AnimationBase
{
    Animator _ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ani = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void UpdateAnimation(State.PlayerState playerState)
    {
        for(int i = 0; i <= (int)State.PlayerState.jump; i++)
        {
            string st = ((State.PlayerState)i).ToString();
            if (playerState == (State.PlayerState)i)
            {
                _ani.SetBool(st, true);
            }
            else {
                _ani.SetBool(st, false);

            }

        }
    }
    public override void UpdateAnimationEnemy(State.EnemyState enemyState)
    {
        for (int i = 0; i <= (int)State.EnemyState.idle; i++)
        {
            string st = ((State.EnemyState)i).ToString();
            if (enemyState == (State.EnemyState)i)
            {
                _ani.SetBool(st, true);
            }
            else
            {
                _ani.SetBool(st, false);

            }

        }
    }
}
