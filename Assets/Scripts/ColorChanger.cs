using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(NetworkView))]
public class ColorChanger : MonoBehaviour
{
    Material material;

    // Use this for initialization
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 100, 20, 90, Screen.height - 25));
        GUILayout.Label("R:");
        var r = GUILayout.HorizontalSlider(material.color.r, 0, 1, null);
        GUILayout.Label("G:");
        var g = GUILayout.HorizontalSlider(material.color.g, 0, 1, null);
        GUILayout.Label("B:");
        var b = GUILayout.HorizontalSlider(material.color.b, 0, 1, null);
        GUILayout.Space(20);

        Texture2D t = new Texture2D(1, 1);
        t.SetPixel(0, 0, new Color(r, g, b));
        t.wrapMode = TextureWrapMode.Repeat;
        t.Apply();
        GUIStyle s = new GUIStyle();
        s.normal.background = t;
        GUILayout.Label("", s);


        GUILayout.EndArea();
        material.color = new Color(r, g, b);
    }
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            var color = material == null ? Color.white : material.color;
            var value = new Vector3(color.r, color.g, color.b);
            stream.Serialize(ref value);
        }
        else
        {
            var value = new Vector3();
            stream.Serialize(ref value);
            info.networkView.GetComponent<Renderer>().material.color = new Color(value.x, value.y, value.z);
        }
    }
}
