using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    private Destroyable _currentTarget;
    [SerializeField] private string _targetTag;
    [SerializeField] private float _coolDown;
    private List<Destroyable> _targets = new List<Destroyable>();
    private bool _isActive = true;
    private PlayAudioClips _clips;

    void Start()
    {
        _clips = GetComponent<PlayAudioClips>();
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
            _clips.PlaySound(0);
            yield return new WaitForSeconds(_coolDown);
        }
    }
    private void Shoot()
    {
        var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Target = _currentTarget;
    }
    public void SetActive(bool active)
    {
        _isActive = active;
    }
}

