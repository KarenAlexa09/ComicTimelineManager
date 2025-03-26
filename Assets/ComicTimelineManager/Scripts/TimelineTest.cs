using System.Collections.Generic;
using UnityEngine;

public class TimelineTest : MonoBehaviour
{
    [SerializeField] private UIStoryController uIStoryController;
    [Space, SerializeField] private List<StoryEntity> achievementEntities = new();

    private void Start()
    {
        if (uIStoryController != null)
            uIStoryController.Initialize(achievementEntities);
    }

    private void OnDisable()
    {
        uIStoryController.Conclude();
    }
}
