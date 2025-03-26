using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "StorySnippetsSO", menuName = "Story/Story Snippets", order = 1)]
public class StorySnippetsModel : ScriptableObject
{
    [SerializeField, NonReorderable] private List<StorySnippet> snippets = new();

    public List<StorySnippet> GetStory() { return snippets; }
}

[Serializable]
public class StorySnippet
{
    [SerializeField] private StorysTypes storyType;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool isNotified = false;
    [SerializeField] private AssetReferenceGameObject playableDirectorPrefab;
    [SerializeField] private Sprite coverImage;
    [SerializeField, TextArea(1, 5)] private string description;
    private StoryEntity story;

    public StorysTypes StoryType { get { return storyType; } }
    public bool IsUnlocked { get { return isUnlocked; } }
    public bool IsNotified { get { return isNotified; } }
    public StoryEntity Story { get { return story; } }
    public AssetReferenceGameObject PlayableDirectorPrefab { get { return playableDirectorPrefab; } }
    public Sprite CoverImage { get { return coverImage; } }
    public string Description { get { return description; } }

    public void SetStorySnippetUnlocked(bool _isUnlocked, StoryEntity storyEntity)
    {
        story = storyEntity;
        isUnlocked = _isUnlocked;
    }

    public void SetStorySnippetNotified(bool _isNotified, StoryEntity storyEntity)
    {
        story = storyEntity;
        isNotified = _isNotified;
    }
}
