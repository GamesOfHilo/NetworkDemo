using UnityEngine;
using System.Collections;

public class PlayerLocal : MonoBehaviour
{
    private Vector3 p;
    private Quaternion r;
    private int m = 0;

    void Start()
    {
        networkView.observed = this;
    }

    void OnSerializeNetworkView(BitStream stream)
    {
        p = rigidbody.position;
        r = rigidbody.rotation;
        m = 0;
        stream.Serialize(ref p);
        stream.Serialize(ref r);
        stream.Serialize(ref m);
    }
}
