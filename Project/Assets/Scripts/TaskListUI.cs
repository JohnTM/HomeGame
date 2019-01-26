using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
            icon.Text.text = tb.Task.Name;
            icon.transform.SetParent(transform);
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
        TaskIcon icon = m_taskIconMap[tb];
        if (icon)
        {
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
        TaskIcon icon = m_taskIconMap[tb];
        if (icon)
        {
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
