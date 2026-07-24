using System.Collections;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float dropSpeed = 1.7f;
    private float stopY;
    private Coroutine disableCoroutine;
    private bool isFalling = true;

    void OnEnable()
    {
        stopY = Random.Range(-2f, 4f);

        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }

        // 7초 뒤 자동 비활성화 코루틴 시작
        disableCoroutine = StartCoroutine(AutoDisableRoutine(7f));
    }

    void Update()
    {
        if (transform.position.y > stopY && isFalling)
        {
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);
        }
    }

    IEnumerator AutoDisableRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void OnMouseDown()
    {
        SunManager.instance.AddSun(25);

        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        isFalling = false;
        StopAllCoroutines();
    }

    public void Initialize(float targetStopY,bool shouldFall)
    {
        stopY =targetStopY;
        isFalling = shouldFall;

        if (disableCoroutine != null) StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(AutoDisableRoutine(7f));
    }
}
