using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableMagnet : MonoBehaviour
{

    public Transform m_linkedTransform;
    public float m_radius=1;
    public LayerMask m_affectable = -1;
    public bool m_snapToParent;


    public void SnapToFixMagnetAroundWithDefaultParameters()
    {
        List<MagnetFixAnchor> fixmags = MagnetAnchorUtility.CheckForFixMagnet(m_linkedTransform.position, m_radius, m_affectable);
        if (fixmags.Count > 0)
        {
            Snap(fixmags);
        }
    }
    public void SnapToFixMagnetAtRangeWithDefaultParameters(float radius)
    {
        List<MagnetFixAnchor> fixmags = MagnetAnchorUtility.CheckForFixMagnet(m_linkedTransform.position, radius, m_affectable);
        if (fixmags.Count > 0)
        {
            Snap(fixmags);
        }
    }
    public void Unparent() {
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
}

public class MagnetAnchorUtility {
    public static List<MagnetFixAnchor> CheckForFixMagnet(Vector3 where, float radius, LayerMask layer )
    {
        List<MagnetFixAnchor> anchors = new List<MagnetFixAnchor>();
        Collider [] hits =Physics.OverlapSphere(where, radius, layer);
        for (int i = 0; i < hits.Length; i++)
        {
            MagnetFixAnchor anc = hits[i].GetComponent<MagnetFixAnchor>();
            if (anc != null)
                anchors.Add(anc);

        }
        return anchors;
    }
}
