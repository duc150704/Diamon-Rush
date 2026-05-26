using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject _pref;

    List<GameObject> a = new();

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                a.Add(Instantiate(_pref, new Vector3(i,j), Quaternion.identity));
            }
        }
    }
}