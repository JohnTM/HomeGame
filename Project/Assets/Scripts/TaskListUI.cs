using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class TaskListUI : MonoBehaviour {

    [SerializeField]
    private TaskIcon m_taskIconPrefab;

    [SerializeField]
    private RectTransform m_punishTickPrefab;

    [SerializeField]
    private RectTransform m_rewardTickPrefab;

    private Dictionary<TaskBroadcaster, TaskIcon> m_taskIconMap = new Dictionary<TaskBroadcaster, TaskIcon>();

    public void BroadcasterAdded(TaskBroadcaster tb)
    {
        if (!m_taskIconMap.ContainsKey(tb))
        {
            TaskIcon icon = Instantiate<TaskIcon>(m_taskIconPrefab);
            icon.TaskBroadcaster = tb;
            icon.Text.text = tb.Task.Name;
            icon.Image.sprite = tb.Task.Icon;
            icon.transform.SetParent(transform);
            icon.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().rect.width, 0);
            m_taskIconMap.Add(tb, icon);
        }
    }

    public void BroadcasterRemoved(TaskBroadcaster tb)
    {
        if (m_taskIconMap.ContainsKey(tb))
        {
            TaskIcon icon = m_taskIconMap[tb];            
            m_taskIconMap.Remove(tb);
            if (icon)
            {
                Destroy(icon.gameObject);
            }            
        }
    }

    public void SpawnPunishTick(TaskBroadcaster tb, EmotionalStateUI emotionUI)
    {
        
        if (m_taskIconMap.ContainsKey(tb))
        {
            TaskIcon icon = m_taskIconMap[tb];

            RectTransform punishTick = Instantiate<RectTransform>(m_punishTickPrefab, transform.parent);
            punishTick.position = icon.transform.position + Vector3.up * 30;

            Vector3 start = punishTick.position;
            Vector3 target = emotionUI.transform.position;
            Vector3 dir = (start - target + Vector3.up * 20).normalized;
            target += dir * 50.0f;

            Vector3[] path = new Vector3[] { start, Vector3.Lerp(start, target, 0.5f) + Vector3.up * 30.0f, target};

            var seq = DOTween.Sequence();

            seq.Append(punishTick.DOPath(path, 1.0f, PathType.CatmullRom).SetEase(Ease.InCubic));
            seq.Insert(0, punishTick.GetComponent<Image>().DOFillAmount(1.0f, 0.25f));
            seq.InsertCallback(0.9f, () => { emotionUI.Punish(1, tb); });
            Destroy(punishTick.gameObject, 1.0f);
        }

        //m_emotionalStateUI.Punish(amount, source);
    }

    public void SpawnRewardTick(int amount, TaskBroadcaster tb, EmotionalStateUI emotionUI)
    {
        if (m_taskIconMap.ContainsKey(tb))
        {
            TaskIcon icon = m_taskIconMap[tb];

            RectTransform punishTick = Instantiate<RectTransform>(m_rewardTickPrefab, transform.parent);
            punishTick.position = icon.transform.position + Vector3.up * 30;

            Vector3 start = punishTick.position;
            Vector3 target = emotionUI.transform.position;
            Vector3 dir = (start - target + Vector3.up * 20).normalized;
            target += dir * 50.0f;

            Vector3[] path = new Vector3[] { start, Vector3.Lerp(start, target, 0.5f) + Vector3.up * 30.0f, target };

            var seq = DOTween.Sequence();

            seq.Append(punishTick.DOPath(path, 1.0f, PathType.CatmullRom).SetEase(Ease.InCubic));
            seq.Insert(0, punishTick.GetComponent<Image>().DOFillAmount(1.0f, 0.25f));
            seq.InsertCallback(0.9f, () => { emotionUI.Reward(amount, tb); });
            Destroy(punishTick.gameObject, 1.0f);
        }

        //m_emotionalStateUI.Punish(amount, source);
    }

    // Use this for initialization
    void Start () {
		
	}

    public void SortChildren()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform tran in transform)
        {
            children.Add(tran);
        }
       
        children.Sort(Compare);
        for (int i = 0; i < children.Count; i++)
            children[i].SetSiblingIndex(i);
    }
    private int Compare(Transform lhs, Transform rhs)
    {
        if (lhs == rhs) return 0;
        var test = rhs.gameObject.activeInHierarchy.CompareTo(lhs.gameObject.activeInHierarchy);
        if (test != 0) return test;

        TaskBroadcaster t1 = lhs.GetComponent<TaskIcon>().TaskBroadcaster;
        TaskBroadcaster t2 = rhs.GetComponent<TaskIcon>().TaskBroadcaster;

        int c = t1.Task.Name.CompareTo(t2.Task.Name);

        if (c == 0)
        {
            return t2.TotalTime.CompareTo(t1.TotalTime);
        }

        return c;
    }

    // Update is called once per frame
    void Update () {

        SortChildren();

        float x = 5;
		for (int i = 0; i < transform.childCount; i++)
        {            
            RectTransform rt = (RectTransform)transform.GetChild(i);
            TaskIcon icon = rt.gameObject.GetComponent<TaskIcon>();
            TaskBroadcaster tb = icon.TaskBroadcaster;

            icon.TimerBar.fillAmount = Mathf.Clamp01(tb.TotalTime / tb.Task.PunishTime);

            Vector2 target = new Vector2(x, 0);

            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, target, 0.1f);

            if (i < transform.childCount-1)
            {
                RectTransform rt2 = (RectTransform)transform.GetChild(i + 1);
                TaskIcon icon2 = rt2.gameObject.GetComponent<TaskIcon>();
                TaskBroadcaster tb2 = icon2.TaskBroadcaster;

                if (tb.Task.Name == tb2.Task.Name && tb.Task.Grouped && tb2.Task.Grouped)
                {
                    x += 15;
                    continue;
                }
            }

            x += rt.rect.size.x + 5;
        }
	}
}
