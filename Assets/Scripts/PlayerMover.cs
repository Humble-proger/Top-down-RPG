using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rb;
    private Vector2 _startPosition;
    private bool _isWalk = false;
    private DirectionMove _oldDirection;

    public event Action<bool> Walk;
    public event Action<bool> DirectionWalkY;
    public event Action<bool> DirectionWalkX;

    public enum DirectionMove {
        None,
        Up,
        Down,
        Right,
        Left
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }
    private void FixedUpdate()
    {
        Vector2 impulse = InputController.Instance.Move;
        ReportMove(DefinitionMove(impulse));

        _rb.position = _rb.position + _speed * (impulse * Time.fixedDeltaTime);
    }

    private DirectionMove DefinitionMove(Vector2 move) {
        DirectionMove typeMove = DirectionMove.None;
        if (move.y > 0)
        {
            typeMove = DirectionMove.Up;
        }
        else if (move.y < 0)
        {
            typeMove = DirectionMove.Down;
        }
        if (move.x > 0)
        {
            typeMove= DirectionMove.Right;
        }
        else if (move.x < 0)
        {
            typeMove = DirectionMove.Left;
        }
        return typeMove;
    }

    private void ReportMove(DirectionMove typeMove) 
    {
        if (DirectionMove.None == typeMove)
        {
            if (_isWalk)
            {
                _isWalk = false;
                Walk?.Invoke(_isWalk);
            }
            return;
        }
        else {
            if (!_isWalk) {
                _isWalk= true;
                Walk?.Invoke(_isWalk);
                ReportMoveXY(typeMove);
                _oldDirection = typeMove;
                return;
            }
        }

        if (_oldDirection == typeMove) return;
        ReportMoveXY(typeMove);
        _oldDirection = typeMove;
    }

    private void ReportMoveXY(DirectionMove typeMove)
    {
        switch (typeMove)
        {
            case DirectionMove.Up:
                DirectionWalkY?.Invoke(true);
                break;
            case DirectionMove.Down:
                DirectionWalkY?.Invoke(false);
                break;
            case DirectionMove.Left:
                DirectionWalkX?.Invoke(false);
                break;
            case DirectionMove.Right:
                DirectionWalkX?.Invoke(true);
                break;
            default:
                break;
        }
    }
    public void Reset()
    {
        _rb.position = _startPosition;
    }
}