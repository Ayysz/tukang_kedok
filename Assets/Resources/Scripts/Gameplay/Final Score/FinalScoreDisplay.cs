using TMPro;
using UnityEngine;

public class FinalScoreDisplay : MonoBehaviour
{
    [SerializeField] private MaskData data;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private Transform maskParent;

    public void SetData(MaskData data)
    {
        this.data = data;
        MaskDataSO maskDataSO = MaskDatabase.Instance.GetMask(data.maskID);
        nameText.text = maskDataSO.maskName;
        scoreText.text = data.score.ToString();
        foreach (Transform t in maskParent)
        { 
            Destroy(t.gameObject);
        }
        MaskDisplay maskDisplay = Instantiate(maskDataSO.maskDisplayPrefab, maskParent);
        maskDisplay.transform.position = maskParent.position;
        maskDisplay.transform.rotation = maskParent.rotation;
        maskDisplay.DisplayMask(data.currentProgress);
    }
    private void Update()
    {
        maskParent.Rotate(0, 90f * Time.deltaTime, 0);
    }
}
