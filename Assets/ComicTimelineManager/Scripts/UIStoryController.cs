using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIStoryController : MonoBehaviour
{
    [SerializeField] private StorySnippetsModel snippetsModel;
    [SerializeField] private TimelineController timelineController;
    [Space, Header("Configuration")]
    [SerializeField] private KeyCode keyCodeToSkipTimeline = KeyCode.Escape;
    [SerializeField] private bool isActiveDebug = false;

    private List<StoryEntity> _storyEntities;
    private Queue<StorySnippet> unlockedStoriesQueue;
    private UIStoryView view;
    private CanvasGroup canvasGroup;
    private Coroutine skipCheckCoroutine;

    private bool isPlayedUnlockedStory = false;
    private bool hasSkipped = false;

    private void Awake()
    {
        view = GetComponentInChildren<UIStoryView>();

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void AddListeners()
    {
        timelineController.TimelineFinished += OnTimelineFinished;

        if (view != null)
            view.PlayedTimeline += OnPlayedTimeline;
    }

    private void RemoveListeners()
    {
        timelineController.TimelineFinished -= OnTimelineFinished;

        if (view != null)
            view.PlayedTimeline -= OnPlayedTimeline;
    }

    public void Initialize(List<StoryEntity> storytEntities)
    {
        _storyEntities = storytEntities;

        unlockedStoriesQueue = new Queue<StorySnippet>();

        timelineController.Initialize();

        if (!isActiveDebug)
            LoadStoryState();

        UnlockStories(_storyEntities);

        if (view != null)
            view.Initialize(snippetsModel.GetStory());

        if (unlockedStoriesQueue.Count > 0)
        {
            SetActiveGallery(false);

            PlayNextStory();
        }
        else
        {
            SetActiveGallery(true);

            if (view != null)
                view.PlayAnimations();
        }

        AddListeners();
    }

    private void UnlockStories(List<StoryEntity> achievementEntities)
    {
        var snippetsByAchievementType = snippetsModel.GetStory()
            .GroupBy(snippet => snippet.StoryType)
            .ToDictionary(g => g.Key, g => g.ToList());

        var allStories = achievementEntities
            .Where(e => snippetsByAchievementType.ContainsKey(e.StorysType))
            .SelectMany(e => snippetsByAchievementType[e.StorysType].Select(s => (story: s, entity: e)));

        foreach (var (story, achievementEntity) in allStories)
        {
            story.SetStorySnippetUnlocked(story.IsUnlocked, achievementEntity);
            story.SetStorySnippetNotified(story.IsNotified, achievementEntity);

            if (achievementEntity.IsCompleted && !story.IsUnlocked)
            {
                story.SetStorySnippetUnlocked(true, achievementEntity);
                story.SetStorySnippetNotified(true, achievementEntity);

                unlockedStoriesQueue.Enqueue(story);
            }

            SaveStoryState(story);
        }
    }

    private void PlayNextStory()
    {
        if (unlockedStoriesQueue.Count > 0)
        {
            isPlayedUnlockedStory = true;

            var nextSnippet = unlockedStoriesQueue.Dequeue();

            timelineController.PlayStory(nextSnippet);

            SetActiveGallery(false);
        }
        else
        {
            SetActiveGallery(true);

            if (isPlayedUnlockedStory && view != null)
                view.PlayAnimations();

            isPlayedUnlockedStory = false;
        }
    }

    private void OnPlayedTimeline(StorySnippet snippet)
    {
        isPlayedUnlockedStory = false;

        timelineController.PlayStory(snippet);

        SetActiveGallery(false);

        if (skipCheckCoroutine == null)
            skipCheckCoroutine = StartCoroutine(CheckForSkip());
    }

    private void OnTimelineFinished()
    {
        PlayNextStory();
    }

    private void SetActiveGallery(bool active)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = active ? 1 : 0;
    }

    private void SaveStoryState(StorySnippet story)
    {
        PlayerPrefs.SetInt($"Story_{story.StoryType}_Unlocked", story.IsUnlocked ? 1 : 0);

        PlayerPrefs.SetInt($"Story_{story.StoryType}_Notified", story.IsNotified ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void LoadStoryState()
    {
        foreach (StorySnippet story in snippetsModel.GetStory())
        {
            story.SetStorySnippetUnlocked(PlayerPrefs.GetInt($"Story_{story.StoryType}_Unlocked", 0) == 1, story.Story);

            story.SetStorySnippetNotified(PlayerPrefs.GetInt($"Story_{story.StoryType}_Notified", 0) == 1, story.Story);
        }
    }

    private IEnumerator CheckForSkip()
    {
        hasSkipped = false;

        while (!hasSkipped)
        {
            if (Input.GetKeyDown(keyCodeToSkipTimeline))
            {
                hasSkipped = true;

                timelineController.SkipTimeline();
            }

            yield return null;
        }

        skipCheckCoroutine = null;
    }


    public void Conclude()
    {
        RemoveListeners();

        if (skipCheckCoroutine != null)
            StopCoroutine(skipCheckCoroutine);

        if (view != null)
            view.Conclude();
    }
}
