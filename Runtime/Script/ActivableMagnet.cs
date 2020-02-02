using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivableMagnet : MonoBehaviour
{

    public static List<ActivableMagnet> m_activesMagnetInScene = new List<ActivableMagnet>();
    public Transform m_linkedTransform;
    public float m_radius = 1;
    public LayerMask m_affectable = -1;
    public bool m_snapToParent;
    public float m_minDistanceofOtherToSnap = 0.003f;


    public void OnEnable()
    {
        m_activesMagnetInScene.Add(this);
    }
    public void OnDisable()
    {
        m_activesMagnetInScene.Remove(this);
    }

    public bool SnapToFixMagnetAroundWithDefaultParameters()
    {
        return SnapToFixMagnetAtRangeWithDefaultParameters(m_radius);
    }
    public bool SnapToFixMagnetAtRangeWithDefaultParameters(float radius)
    {
        List<MagnetFixAnchor> fixmags = MagnetAnchorUtility.CheckForFixMagnet(m_linkedTransform.position, radius, m_affectable);
        if (fixmags.Count > 0)
        {
            if (IsOnlyInRegion(m_minDistanceofOtherToSnap))
            {
                Snap(fixmags);
                return true;
            }
        }
        return false;
    }

    private bool IsOnlyInRegion(float radius)
    {
        List<ActivableMagnet> magnet = MagnetAnchorUtility.CheckForActivableMagnet(m_linkedTransform.position, radius);
        if (magnet.Count == 0)
            return true;
        if (magnet.Count == 1 && magnet[0] == this)
            return true;
        return false;

    }

    private int GetActiveMagnetAround(float radius)
    {
        return MagnetAnchorUtility.CheckForActivableMagnet(m_linkedTransform.position, radius).Count;
    }

    public void Unparent()
    {
        transform.parent = null;
    }

    private void Snap(List<MagnetFixAnchor> fixmags)
    {
        m_linkedTransform.position = fixmags[0].transform.position;
        m_linkedTransform.rotation = fixmags[0].transform.rotation;
        if (m_snapToParent)
            m_linkedTransform.parent = fixmags[0].transform;

    }

    private void Reset()
    {
        m_linkedTransform = this.transform;
    }

    public Vector3 GetPosition()
    {
        return m_linkedTransform.position;
    }



}

public class MagnetAnchorUtility
{
    public static List<MagnetFixAnchor> CheckForFixMagnet(Vector3 where, float radius, LayerMask layer)
    {
        List<MagnetFixAnchor> anchors = new List<MagnetFixAnchor>();
        Collider[] hits = Physics.OverlapSphere(where, radius, layer);
        for (int i = 0; i < hits.Length; i++)
        {
            MagnetFixAnchor anc = hits[i].GetComponent<MagnetFixAnchor>();
            if (anc != null)
                anchors.Add(anc);

        }
        anchors = anchors.OrderBy(k => Vector3.Distance(k.GetPosition(), where)).ToList();
        return anchors;
    }
    public static List<ActivableMagnet> CheckForActivableMagnet(Vector3 where, float radius)
    {
        List<ActivableMagnet> anchors = ActivableMagnet.m_activesMagnetInScene;
        anchors = anchors.Where(k => Vector3.Distance(where, k.GetPosition()) < radius).OrderBy(k => Vector3.Distance(k.GetPosition(), where)).ToList();
        return anchors;
    }

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}


