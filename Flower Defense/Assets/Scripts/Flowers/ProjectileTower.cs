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
    private PlayAudioClips _clips;
    private Upgradeable _watering;

    void Start()
    {
        _clips = GetComponent<PlayAudioClips>();
        _watering = GetComponentInChildren<Upgradeable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_targets.Count > 0 && _currentTarget == null && _isActive)
        {
            _currentTarget = _targets.FirstOrDefault();
            _targets.Remove(_currentTarget);
            StartCoroutine(ShootCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var destroyable = collision.GetComponent<Destroyable>();
        if (destroyable != null && collision.CompareTag(_targetTag))
        {
            _targets.Add(destroyable);
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
        while (_currentTarget != null && _isActive)
        {
            Shoot();
            _clips.PlaySound(_watering.CurrentLvl, 0.5f);
            yield return new WaitForSeconds(CoolDown);
        }
    }
    private void Shoot()
    {
        var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        var projectileComponent= projectile.GetComponent<Projectile> ();
        projectileComponent.Damage = ProjectileDamage;
        projectileComponent.Speed= ProjectileSpeed;
        projectile.GetComponent<Projectile>().Target = _currentTarget;
    }
    public void SetActive(bool active)
    {
        _isActive = active;
    }
}

