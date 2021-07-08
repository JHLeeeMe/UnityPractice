using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    [SerializeField] private float _velocity = 0.1f;
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _throwClip;

    private bool _isInvincible = false;
    private float _timeInvincible = 2.0f;
    private float _invincibleTimer;

    private int _currentHealth;
    public int health { get => _currentHealth; }

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _lookDirection = new Vector2(1, 0);

    private AudioSource _audioSource;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            _lookDirection.Set(move.x, move.y);
            _lookDirection.Normalize();
        }
        
        _animator.SetFloat("LookX", _lookDirection.x);
        _animator.SetFloat("LookY", _lookDirection.y);
        _animator.SetFloat("Speed", move.magnitude);

        Vector2 position = _rigidbody2D.position;
        position += move * _velocity * Time.deltaTime;
        _rigidbody2D.MovePosition(position);

        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0)
            {
                _isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = 
                Physics2D.Raycast(_rigidbody2D.position + (Vector2.up * 0.2f),
                                  _lookDirection,
                                  1.5f,
                                  LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        // Hit or Heal
        if (amount < 0)
        {
            if (_isInvincible) { return; }

            _animator.SetTrigger("Hit");
            _isInvincible = true;
            _invincibleTimer = _timeInvincible;

            PlaySound(_hitClip);
        }

        // Update currentHealth
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        if (_currentHealth == 0) { 
            Destroy(gameObject); 
        }

        // UI HealthBar
        UIHealthBar.instance.SetValue(_currentHealth / (float)_maxHealth);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    private void Launch()
    {
        GameObject projectileObject = 
            Instantiate(_projectilePrefab, _rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);

        _animator.SetTrigger("Launch");
        PlaySound(_throwClip);
    }
}
