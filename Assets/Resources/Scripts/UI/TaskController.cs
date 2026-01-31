using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public Transform parent;
    public GameObject taskObject;
    public TaskLine TaskLinePrefab;
    public void Show()
    {
        taskObject.SetActive(true);
    }
    public void Hide()
    {
        taskObject.SetActive(false);
    }
    public void SetTaskLines(List<CraftingType> craftingTypes, int progress)
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < craftingTypes.Count; i++)
        {
            TaskLine tl = Instantiate(TaskLinePrefab, parent);
            bool prog = false;
            if (progress > i)
            {
                prog = true;
            }
            else {
                prog = false;
            }
            tl.SetTask(craftingTypes[i], prog);
        }
    }
}
