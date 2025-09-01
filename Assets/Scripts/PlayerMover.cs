using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rb;
    private Vector2 _startPosition;
    private bool _isWalk = false;
    private bool _isDown = false;

    public event Action<bool> Walk;
    public event Action<bool> DirectionWalk;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }
    private void FixedUpdate()
    {
        Vector2 impulse = InputController.Instance.Move;
        DefinitionMove(impulse);

        _rb.position = _rb.position + _speed * (impulse * Time.fixedDeltaTime);
    }

    private void DefinitionMove(Vector2 move) {
        if (move == Vector2.zero) {
            if (_isWalk) {
                _isWalk = false;
                Walk?.Invoke(_isWalk);
            }
            return;
        }
        if (move.y > 0)
        {
            if (_isDown)
            {
                _isDown = false;
                DirectionWalk?.Invoke(_isDown);
            }
        }
        else {
            if (!_isDown)
            {
                _isDown = true;
                DirectionWalk?.Invoke(_isDown);
            }
        }
    }

    public void Reset()
    {
        _rb.position = _startPosition;
    }
}