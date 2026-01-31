using System;
using System.Collections;
using UnityEngine;

public class MinigameSculptTarget : MonoBehaviour
{
    [SerializeField] private float circleDelay;
    [SerializeField] private float circleSpeed;
    [SerializeField] private float circleStartSize;
    [SerializeField] private float circlePerfectThreshold;
    [SerializeField] private float circleGreatThreshold;
    [SerializeField] private float targetSize;


    [SerializeField] private MinigameSculptCircle circlePrefab;

    public Action OnFailed;
    public Action OnPerfect;
    public Action OnGreat;
    public Action OnGood;

    public void SetAction(Action fail, Action perfect, Action great, Action good)
    {
        OnFailed += fail;
        OnPerfect += perfect;
        OnGreat += great;
        OnGood += good;
    }
    public void Set(float delay,float speed,float size,float greatThreshold,float perfectThreshold,float targetSize)
    { 
        circleDelay = delay;
        circleSpeed = speed;
        circleStartSize = size;
        circlePerfectThreshold = greatThreshold;
        circleGreatThreshold = perfectThreshold;
        this.targetSize = targetSize;
        StartCoroutine(SpawnDelay());
    }
    private void OnDestroy()
    {
        OnFailed = null;
        OnPerfect = null;
        OnGreat = null;
        OnGood = null;
    }
    public IEnumerator SpawnDelay()
    { 
        yield return new WaitForSeconds(circleDelay);
        MinigameSculptCircle c = Instantiate(circlePrefab, transform.position, Quaternion.identity,transform);
        c.OnFailed += OnFailed;
        c.OnPerfect += OnPerfect;
        c.OnGreat += OnGreat;
        c.OnGood += OnGood;
        c.Set(circleSpeed, circleStartSize, circleGreatThreshold, circlePerfectThreshold,targetSize);
      
    }
}
