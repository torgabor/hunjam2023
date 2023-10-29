using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantConsumer : MonoBehaviour
{
    private List<Upgradeable> _food = new List<Upgradeable>();
    [SerializeField] private int _eatingRate;
    private CircleCollider2D collider;
    private CastleDestroyable _castle;
    private AudioSource _damageAudioSource;
    [SerializeField] private List<AudioClip> _damageClips;

    private void Start()
    {
        collider= GetComponent<CircleCollider2D>();
        if(_damageClips.Count>0 )
        {
            AddAudioSource(_damageAudioSource, _damageClips, 1, (0.75f, 1.25f));
        }
        StartCoroutine(EatCoroutine());
    }
    private void AddAudioSource(AudioSource audioSource, List<AudioClip> clips, float volume, (float, float) pitch)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
        audioSource.volume = volume;
        audioSource.pitch = UnityEngine.Random.Range(pitch.Item1, pitch.Item2);
    }
    private void Update()
    {
        _food.Clear();
        var colliders = Physics2D.OverlapCircleAll(transform.position, collider.radius);
        if(_damageAudioSource!=null && colliders.Length>0 && !_damageAudioSource.isPlaying)
        {
            _damageAudioSource.Play();
        }
        foreach (var col in colliders)
        {
            if (col.CompareTag("Crops"))
            {
                var dest = col.GetComponent<Upgradeable>();
                if (dest != null)
                {
                    _food.Add(dest);
                }
            }
            else if(col.CompareTag("Castle") && _castle==null)
            {
                _castle=col.GetComponent<CastleDestroyable>();
                StartCoroutine(EatCastleCoroutine());
            }
        }
    }
    private IEnumerator EatCastleCoroutine()
    {
        while(true)
        {
            _castle.Hp -= _eatingRate;
            yield return new WaitForSeconds(1);
        }
    }
  
    private IEnumerator EatCoroutine()
    {
        while(true)
        {
            foreach(var item in _food)
            {
                item.CurrentProgress -= _eatingRate;
            }
            yield return new WaitForSeconds(1);
        }
       
    }
}
