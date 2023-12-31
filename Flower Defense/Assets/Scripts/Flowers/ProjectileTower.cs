using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    private Destroyable _currentTarget;
    [SerializeField] private string _targetTag;
    public float CoolDown;
    public float ProjectileSpeed;
    public int ProjectileDamage;
    private List<Destroyable> _targets = new List<Destroyable>();
    private bool _isActive = true;
    private Upgradeable _watering;
    [SerializeField] private List<AudioClip> _shootClips;
    private AudioSource _audioSource;
    private bool isFiring = false;

    void Start()
    {
        _watering = GetComponentInChildren<Upgradeable>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var destroyable = collision.GetComponent<Destroyable>();
        if (destroyable != null && collision.CompareTag(_targetTag))
        {
            _targets.Add(destroyable);
            if (!isFiring)
            {
                StartCoroutine(ShootCoroutine());
                isFiring = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var destroyable = collision.GetComponent<Destroyable>();
        if (destroyable != null && collision.CompareTag(_targetTag))
        {
            _targets.Remove(destroyable);
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (_isActive && _targets.Count > 0)
            {
                _currentTarget = _targets.FirstOrDefault();
                Shoot();
            }
            yield return new WaitForSeconds(CoolDown);
        }
    }
    private void Shoot()
    {
        var lookAtTarget = _currentTarget.transform.position - transform.position;
        var lookRotation = Quaternion.LookRotation(lookAtTarget, new Vector3(0, 1, 0));
        var projectile = Instantiate(_projectilePrefab, transform.position, lookRotation);
        var projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.Damage = ProjectileDamage;
        projectileComponent.Speed = ProjectileSpeed;
        projectile.GetComponent<Projectile>().Target = _currentTarget;
        _audioSource.PlayOneShot(_shootClips[_watering.CurrentLvl], 0.3f * (1 + _watering.CurrentLvl));
    }
    public void SetActive(bool active)
    {
        _isActive = active;
    }
}

