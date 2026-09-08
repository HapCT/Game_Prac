using UnityEngine;
using static State;
using PlayerState = State.PlayerState;
public abstract class AnimationBase : MonoBehaviour
{
    public abstract void UpdateAnimation(PlayerState _playerState);
    public abstract void UpdateAnimationEnemy(EnemyState _enemyState);

}
