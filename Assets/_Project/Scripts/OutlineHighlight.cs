using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OutlineHighlight : MonoBehaviour
{
    [SerializeField] private Shader outlineShader;
    [SerializeField] private float outlineWidth = 0.02f;

    private GameObject outlineObject;
    private Material outlineMaterialInstance;

    private void Awake()
    {
        MeshFilter sourceMeshFilter = GetComponent<MeshFilter>();

        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform, false);

        MeshFilter mf = outlineObject.AddComponent<MeshFilter>();
        mf.sharedMesh = sourceMeshFilter.sharedMesh;

        MeshRenderer mr = outlineObject.AddComponent<MeshRenderer>();
        outlineMaterialInstance = new Material(outlineShader);
        outlineMaterialInstance.SetFloat("_OutlineWidth", outlineWidth);
        mr.material = outlineMaterialInstance;

        outlineObject.SetActive(false);
    }

    public void Show(Color color)
    {
        outlineMaterialInstance.SetColor("_Color", color);
        outlineObject.SetActive(true);
    }

    public void Hide()
    {
        outlineObject.SetActive(false);
    }
}