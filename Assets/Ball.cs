using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Collider col;
    public MeshRenderer mesh;
    private void Start()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
    }

    public IEnumerator Regenerate()
    {
        mesh.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(4);
        mesh.enabled = true;
        col.enabled = true;
    }
}
