using UnityEngine;

public class PlayerAnimationController : MonoBehaviour 
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMover _mover;
    
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly int _down = Animator.StringToHash("Down");

    private void OnEnable()
    {
        _mover.Walk.performed += OnAnimationWalk;
        _mover.DirectionWalk.performed += OnChangeDirectionWalk;
    }
    private void OnDisable()
    {
        _mover.Walk.performed -= OnAnimationWalk;
        _mover.DirectionWalk.performed -= OnChangeDirectionWalk;
    }

    private void OnAnimationWalk(bool value) 
    {
        _animator.SetBool(_walk, value);
    }

    private void OnChangeDirectionWalk(bool value)
    {
        _animator.SetBool(_down, value);
    }
}