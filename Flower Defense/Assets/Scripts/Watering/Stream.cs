using System.Collections;
using UnityEngine;

public class Stream : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ParticleSystem splashParticle;
    public Vector3 targetPosition = Vector3.zero;
    private Coroutine pourRoutine = null;
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }
    void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPourCoroutine());
    }
    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }
    public IEnumerator BeginPourCoroutine()
    {
        while (gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();
            MoveToPosition(0, transform.position);
            AnimateToPosition(0, targetPosition);
            yield return null;
        }
    }
    private IEnumerator EndPour()
    {
        while (!HasReachedPosition(0, targetPosition))
        {
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }
        Destroy(gameObject);
    }
    private Vector3 FindEndPoint()
    {
        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);

        var ray = new Ray2D(transform.position,mousePos-transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, ray.direction, 2.0f);
        return hit.collider ? hit.point : ray.GetPoint(2.0f);
    }
    private void MoveToPosition(int idx, Vector3 target)
    {
        lineRenderer.SetPosition(idx, target);
    }
    private void AnimateToPosition(int idx, Vector3 target)
    {
        var currentPoint = lineRenderer.GetPosition(idx);
        var newPos = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(idx, newPos);
    }
    private bool HasReachedPosition(int idx, Vector3 target)
    {
        return lineRenderer.GetPosition(idx) == targetPosition;

    }
    private IEnumerator UpdateParticle()
    {
        while(gameObject.activeSelf)
        {
            splashParticle.transform.position = targetPosition;
            var isHitting = HasReachedPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);
            yield return null;
        }
    }

}
