using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float damping;
    private float originalZoom;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        originalZoom = GetComponent<Camera>().orthographicSize;
    }

    void FixedUpdate()
    {
        Vector3 movePosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }

    public void ChangeZoom(float zoom)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothZoom(zoom));
    }

    public void RevertZoom()
    {
        ChangeZoom(originalZoom);
    }

    private IEnumerator SmoothZoom(float zoom)
    {
        
        float lerpZoom = GetComponent<Camera>().orthographicSize;
        float difference = Mathf.Abs(lerpZoom - zoom);
        float count = 0;
        float increment = (lerpZoom - zoom < 0) ? 0.01f : -0.01f;

        while (count <= difference)
        {
            lerpZoom += increment;
            count += Mathf.Abs(increment);
            GetComponent<Camera>().orthographicSize = lerpZoom;
            yield return new WaitForSeconds(0.01f);
        }

    }
}
