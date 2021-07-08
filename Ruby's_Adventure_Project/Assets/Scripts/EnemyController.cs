using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _velocity = 2.0f;
    [SerializeField] private bool _isVertical;
    [SerializeField] private float _changeTime = 3.0f;
    [SerializeField] private ParticleSystem _smokeEffect;

    private Rigidbody2D _rigidbody2D;
    private float _timer;
    private int _direction = 1;
    private Animator _animator;
    private bool _isBroken = true;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _timer = _changeTime;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isBroken) { return; }

        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _direction *= -1;
            _timer = _changeTime;
        }

        Vector2 position = _rigidbody2D.position;
        if (_isVertical)
        {
            _animator.SetFloat("MoveX", 0);
            _animator.SetFloat("MoveY", _direction);
            position.y += _direction * _velocity * Time.deltaTime;
        }
        else
        {
            _animator.SetFloat("MoveX", _direction);
            _animator.SetFloat("MoveY", 0);
            position.x += _direction * _velocity * Time.deltaTime;
        }

        _rigidbody2D.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        RubyController ruby = other.gameObject.GetComponent<RubyController>();
        if (ruby == null) { return; }

        ruby.ChangeHealth(-1);
    }

    public void Fix()
    {
        _isBroken = false;
        _rigidbody2D.simulated = false;
        _animator.SetTrigger("Fixed");
        _smokeEffect.Stop();
    }
}
