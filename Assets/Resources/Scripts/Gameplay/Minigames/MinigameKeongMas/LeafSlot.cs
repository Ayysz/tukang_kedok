using UnityEngine;

public class LeafSlot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk adalah Leaf
        //if (!other.CompareTag("Leaf")) return;

        //Leaf leaf = other.GetComponent<Leaf>();
        //if (leaf == null) return;

        //leaf.cancelDrag();

        //// Parent ke SLOT (ini prefab INSTANCE, aman)
        //other.transform.SetParent(transform);

        //// Snap posisi & rotasi
        //other.transform.localPosition = Vector3.zero;
        //other.transform.localRotation = Quaternion.identity;
    }
}
