using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonContinusRotate : MonoBehaviour
{
    public Vector3 m_rotationEuler;
  
    void Update()
    {
        transform.Rotate(m_rotationEuler * Time.deltaTime);
    }
}
