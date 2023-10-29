using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringDevice : MonoBehaviour
{
    [SerializeField] private int _waterRate;
    [SerializeField] private int _fillRate;
    [SerializeField] private float _fillInterval=1f;
    private int _currentFill;
    [SerializeField] private int _capacity;
    private bool _inLake = false;
    private bool _isWatering = false;
    private Upgradeable _currentlyWatered;
    private AudioSource _spillSource;
    private AudioSource _fillSource;
    [SerializeField] private AudioClip _spillClip;
    [SerializeField] private AudioClip _fillClip;
    public int CurrentPercentage { get { return _currentFill; } set { _currentFill = Math.Clamp(value, 0, _capacity); } }
    // Start is called before the first frame update
    void Start()
    {
        _spillSource = gameObject.AddComponent<AudioSource>();
        _spillSource.clip = _spillClip;
        _spillSource.volume = 0.1f;
        _fillSource = gameObject.AddComponent<AudioSource>();
        _fillSource.clip = _fillClip;
        _fillSource.volume = 0.8f;
        _spillSource.loop = true;
        _currentFill = _capacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWatering && !_spillSource.isPlaying)
        {
            _spillSource.Play();
        }
        else if (!_isWatering && _spillSource.isPlaying)
        {
            _spillSource.Stop();
        }

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
        if (Input.GetMouseButton(0)) //todo maybe buttondown?
        {
            Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(ray, ray);

            var newWatered = hit.collider ? hit.collider.gameObject.GetComponent<Upgradeable>() : null;

            if (_currentlyWatered != null && newWatered != _currentlyWatered)
            {
                _currentlyWatered.IsBeingWatered = false;
            }
            _currentlyWatered = newWatered;


            if (!_inLake)
            {
                if (hit.collider != null && hit.collider.CompareTag("Lake"))
                {
                    _inLake = true;
                    _fillSource.Play();
                    StartCoroutine(FillCoroutine());
                }
                else if (!_isWatering && (_currentlyWatered == null || !_currentlyWatered.IsBeingWatered))
                {
                    StartCoroutine(WaterCoroutine());
                }
            }
            else if (hit.collider == null || !hit.collider.CompareTag("Lake"))
            {
                _inLake = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _inLake = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lake"))
        {
            _inLake = false;
        }
    }
    private IEnumerator FillCoroutine()
    {
        _isWatering = false;
        while (_inLake && CurrentPercentage < _capacity)
        {
            CurrentPercentage += _fillRate;
            yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator WaterCoroutine()
    {
        _isWatering = true;
        while (Input.GetMouseButton(0) && _isWatering)
        {
            if (_currentlyWatered != null)
            {
                if (!_currentlyWatered.IsBeingWatered)
                {
                    _currentlyWatered.IsBeingWatered = true;
                }
                _currentlyWatered.Water(_waterRate);
            }
            CurrentPercentage -= _waterRate;
            yield return new WaitForSeconds(_fillInterval);
        }
        if (_currentlyWatered != null)
        {
            _currentlyWatered.IsBeingWatered = false;
        }
        _isWatering = false;
    }
}
