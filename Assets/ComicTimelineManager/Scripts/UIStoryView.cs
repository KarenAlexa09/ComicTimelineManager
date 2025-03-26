using DG.Tweening;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIStoryView : MonoBehaviour
{
    public Action<StorySnippet> PlayedTimeline;

    [Header("UI Elements")]
    [SerializeField] private Transform galleryContainer;
    [SerializeField] private StoryCardUIView snippetButtonPrefab;
    [SerializeField] private Button quitButton;

    [Header("Animation Configuration")]
    [SerializeField] private float delay = 0.5f;

    private List<StorySnippet> snippets;
    private List<StoryCardUIView> snippetCards;

    private const string DOTWEEN_ID = "UIStoryView";

    private void Awake()
    {
        snippetCards = new List<StoryCardUIView>();

        galleryContainer.gameObject.SetActive(false);
    }

    private void AddListeners()
    {
        foreach (var button in snippetCards)
        {
            button.SnippetOpened += OnSnippetOpen;
        }
    }

    private void RemoveListeners()
    {
        foreach (var button in snippetCards)
        {
            button.SnippetOpened -= OnSnippetOpen;
        }
    }

    public void Initialize(List<StorySnippet> _snippets)
    {
        snippets = _snippets;

        InitializeGallery();

        AddListeners();

        galleryContainer.gameObject.SetActive(true);
    }

    private void InitializeGallery()
    {
        ClearGallery();

        foreach (var snippet in snippets)
        {
            var snippetButton = Instantiate(snippetButtonPrefab, galleryContainer);
            snippetButton.Initialize(snippet);

            snippetCards.Add(snippetButton);
        }
    }

    public void ClearGallery()
    {
        foreach (var button in snippetCards)
        {
            Destroy(button.gameObject);
        }

        snippetCards.Clear();
    }

    public void PlayAnimations()
    {
        DOTween.Kill(DOTWEEN_ID);

        Sequence introSequence = DOTween.Sequence().SetId(DOTWEEN_ID);

        introSequence
            .AppendInterval(delay)
            .AppendCallback(() => CardsSequence());

        introSequence.Play();
    }

    private void CardsSequence()
    {
        float time = 0;

        foreach (var button in snippetCards)
        {
            DOVirtual.DelayedCall(time, () =>
            {
                button.PlayAnimations();

            }).SetId(DOTWEEN_ID);

            time += delay;
        }
    }

    private void OnSnippetOpen(StorySnippet snippet)
    {
        if (snippet.IsUnlocked)
        {
            PlayedTimeline?.Invoke(snippet);

            Debug.Log($"Opening snippet: {snippet.StoryType}");
        }
        else
        {
            Debug.Log("This snippet is locked.");
        }
    }

    public void Conclude()
    {
        DOTween.Kill(DOTWEEN_ID);

        RemoveListeners();

        if (snippetCards.Count > 0)
        {
            foreach (var card in snippetCards)
            {
                if (card != null)
                    card.Conclude();
            }
        }

        ClearGallery();
    }
}
