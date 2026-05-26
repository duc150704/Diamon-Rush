using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private Camera _camera;
    private Vector3 _mouPos;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        _camera = Camera.main;
    }

    public Vector3 GetMousePostion()
    {
        _mouPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _mouPos.z = 0;
        return _mouPos;
    }
}
