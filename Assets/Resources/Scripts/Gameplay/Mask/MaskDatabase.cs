using System.Collections.Generic;
using UnityEngine;

public class MaskDatabase : MonoBehaviour
{
    public static MaskDatabase Instance;

    public List<MaskDataSO> masks = new List<MaskDataSO>();

    public Dictionary<int, MaskDataSO> maskDictionary = new Dictionary<int, MaskDataSO>();
    void Awake()
    {
        Instance = this;
        Initialize();
    }
    public void Initialize()
    {
        for (int i = 0; i < masks.Count; i++)
        {
            maskDictionary.Add(masks[i].id, masks[i]);
        }
    }
    public MaskDataSO GetClient(int id)
    {
        return maskDictionary[id];
    }
}
