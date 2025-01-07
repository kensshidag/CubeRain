using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private float _minTime = 2f;
    [SerializeField] private float _maxTime = 5f;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _isTouchet;

    public event Action<Cube> Touched;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
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
            StartCoroutine(WaitForSeconds());
        }
    }

    public void ResetVelocity()
    {
        if (_rigidbody.velocity != Vector3.zero)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minTime, _maxTime));
        Touched?.Invoke(this);
    }
}
