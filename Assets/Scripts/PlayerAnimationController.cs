using UnityEngine;

public class PlayerAnimationController : MonoBehaviour 
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMover _mover;
    
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly int _down = Animator.StringToHash("Down");
    private readonly int _up = Animator.StringToHash("Up");
    private readonly int _right = Animator.StringToHash("Right");
    private readonly int _left = Animator.StringToHash("Left");


    private void OnEnable()
    {
        _mover.Walk += OnAnimationWalk;
        _mover.DirectionWalkX += ChangeMoveX;
        _mover.DirectionWalkY += ChangeMoveY;
    }
    private void OnDisable()
    {
        _mover.Walk -= OnAnimationWalk;
        _mover.DirectionWalkX -= ChangeMoveX;
        _mover.DirectionWalkY -= ChangeMoveY;
    }

    private void OnAnimationWalk(bool value) => _animator.SetBool(_walk, value);

    private void ChangeMoveY(bool value)
    {
        if (value) OnUp();
        else OnDown();
    }

    private void ChangeMoveX(bool value)
    {
        if (value) OnRight();
        else OnLeft();
    }
    private void OnRight() => _animator.SetTrigger(_right);
    private void OnLeft() => _animator.SetTrigger(_left);
    private void OnUp() => _animator.SetTrigger(_up);
    private void OnDown() => _animator.SetTrigger(_down);
}
