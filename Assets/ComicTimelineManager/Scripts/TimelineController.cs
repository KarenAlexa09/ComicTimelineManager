using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TimelineController : MonoBehaviour
{
    public Action TimelineFinished;

    private Dictionary<string, PlayableDirector> instantiatedDirectors = new();
    private PlayableDirector currentDirector;

    private bool isPlaying = false;
    private bool isSkipped = false;

    public void Initialize()
    {
        isPlaying = false;
        isSkipped = false;
    }

    public void PlayStory(StorySnippet storySnippet)
    {
        if (storySnippet == null || storySnippet.PlayableDirectorPrefab == null)
        {
            Debug.LogWarning("No PlayableDirector prefab available for this snippet.");
            return;
        }

        if (instantiatedDirectors.TryGetValue(storySnippet.StoryType.ToString(), out PlayableDirector director))
        {
            currentDirector = director;
            currentDirector.gameObject.SetActive(true);
            StartTimeline();
        }
        else
        {
            storySnippet.PlayableDirectorPrefab.InstantiateAsync(transform).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject directorInstance = handle.Result;
                    currentDirector = directorInstance.GetComponent<PlayableDirector>();

                    if (currentDirector != null)
                    {
                        var identifier = currentDirector.GetComponent<PlayableDirectorIdentifier>();

                        if (identifier == null)
                        {
                            identifier = currentDirector.gameObject.AddComponent<PlayableDirectorIdentifier>();
                        }

                        identifier.Initialize(storySnippet.StoryType.ToString());

                        instantiatedDirectors[storySnippet.StoryType.ToString()] = currentDirector;
                        StartTimeline();
                    }
                    else
                    {
                        Debug.LogWarning("Prefab does not contain a PlayableDirector.");
                    }
                }
                else
                {
                    Debug.LogError("Error loading PlayableDirector prefab from Addressables.");
                }
            };
        }
    }

    private void StartTimeline()
    {
        currentDirector.Play();
        currentDirector.stopped += OnTimelineFinished;
        isPlaying = true;
        isSkipped = false;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        isPlaying = false;

        if (director != null)
        {
            director.stopped -= OnTimelineFinished;
            director.gameObject.SetActive(false);
        }

        TimelineFinished?.Invoke();
    }

    public void SkipTimeline()
    {
        if (isPlaying && currentDirector != null && !isSkipped)
        {
            currentDirector.time = currentDirector.duration;
            currentDirector.Evaluate();

            isSkipped = true;
        }
    }
}
