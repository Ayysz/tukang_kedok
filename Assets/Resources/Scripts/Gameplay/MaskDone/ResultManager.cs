using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : UIManager
{
    public static ResultManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI clientNameText;
    [SerializeField] private TextMeshProUGUI maskScoreText;
    [SerializeField] private Transform maskPosition;
    [SerializeField] private GameObject[] objectsToHide;

    Action OnContinue;
    private bool isShown = false;
    private float rotateSpeed = 45f;

    private void Awake()
    {
        Instance = this;
    }
    public void Show(string clientName, int totalScore,MaskData maskData,Action action)
    {
        base.Show();
        foreach (Transform t in maskPosition)
        {
            Destroy(t.gameObject);
        }

        MaskDataSO maskDataSO = maskData.GetMaskDataSO();
        MaskDisplay maskPrefab = Instantiate(maskDataSO.maskDisplayPrefab, maskPosition.transform.position,maskPosition.transform.rotation,maskPosition);
        maskPrefab.DisplayMask(maskData.currentProgress);
        maskPosition.rotation = Quaternion.Euler(0, 165, 0);

        clientNameText.text = clientName;   
        maskScoreText.text = "Score: " + totalScore.ToString();
        foreach (GameObject go in objectsToHide)
        {
            go.SetActive(false);
        }
        OnContinue += action;
        isShown = true;
    }
    public void ContinueButton()
    {
        OnContinue?.Invoke();
        OnContinue = null;
        Hide();
    }
    private void Update()
    {
        if ( isShown)
        {
            maskPosition.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }
    private void OnDestroy()
    {
        OnContinue = null;
    }
    public override void Hide()
    {
        base.Hide();
        foreach (Transform t in maskPosition)
        {
            Destroy(t.gameObject);
        }
        foreach (GameObject go in objectsToHide)
        {
            go.SetActive(true);
        }
        isShown = false;

    }


}
