using System.Linq;
using UnityEngine;

public class Sample : MonoBehaviour
{
    MeshRenderer[] renderers;
    [SerializeField] private MeshRenderer[] HideRenders;

    void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>(true);
    }

    private void Start()
    {
        //HideAll();
    }

    public void HideAll()
    {
        foreach (var r in renderers)
        {
            if (HideRenders != null && HideRenders.Contains(r))
                r.enabled = false;
            else
                r.enabled = false;
        }
    }

    public void ShowAll()
    {
        foreach (var r in renderers)
        {
            r.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk adalah Leaf
        //if (!other.CompareTag("Leaf")) return;
        //Debug.Log(other.tag);

        //Leaf leaf = other.GetComponent<Leaf>();
        //if (leaf == null) return;

        //// Parent leaf ke slot
        //other.transform.SetParent(transform);
        //other.transform.localPosition = Vector3.zero;
        //other.transform.localRotation = Quaternion.identity;
    }
}
