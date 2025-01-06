using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial; 

    public event Action<Cube> touched;

    private Renderer _renderer;
    private bool _isTouchet;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _renderer.material = _defaultMaterial;
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _isTouchet = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouchet)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out Platform platform))
        {
            _isTouchet = true;
            _renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            touched?.Invoke(this);
        }
    }

}
