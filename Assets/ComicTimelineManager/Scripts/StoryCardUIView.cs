using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class StoryCardUIView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image snippetImage;
    [SerializeField] private Button snippetButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup lockedOverlay;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text descriptionTxt;
    [SerializeField] private TMP_Text unlockedPercentageTxt;
    [SerializeField] private Slider slider;

    [Header("Animation Configuration")]
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private float scaleValue = 1.5f;
    [SerializeField] private Ease easeFade = Ease.InOutCubic;
    [SerializeField] private Ease easeScaleIn = Ease.OutBack;
    [SerializeField] private Ease easeScaleOut = Ease.OutBounce;

    private StorySnippet snippet;

    public event Action<StorySnippet> SnippetOpened;

    private const string DOTWEEN_ID = "UICardView";

    public void Initialize(StorySnippet _snippet)
    {
        snippet = _snippet;

        snippetButton.onClick.RemoveAllListeners();
        snippetButton.onClick.AddListener(() => OnSnippetOpen(snippet));

        canvasGroup.alpha = 0;

        SetupSnippetView(snippet);
    }

    private void SetupSnippetView(StorySnippet snippet)
    {
        snippetImage.sprite = snippet.CoverImage;
        unlockedPercentageTxt.text = $"{snippet.Story.CurrentProgress} of {snippet.Story.Goal}";
        descriptionTxt.text = snippet.Description;
        title.text = snippet.Story.Title;

        if (snippet.Story.IsCompleted)
        {
            slider.value = 1;
        }
        else
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = (float)snippet.Story.CurrentProgress / snippet.Story.Goal;

            UpdateSnippetState(snippet.IsUnlocked);
        }
    }

    private void UpdateSnippetState(bool isUnlocked)
    {
        lockedOverlay.alpha = isUnlocked ? 0 : 1;
        lockedOverlay.interactable = isUnlocked ? false : true;
        lockedOverlay.blocksRaycasts = isUnlocked ? false : true;

        snippetButton.interactable = isUnlocked;
    }

    private void OnSnippetOpen(StorySnippet snippet)
    {
        if (snippet.IsUnlocked && SnippetOpened != null)
        {
            Debug.Log("Open story...");

            SnippetOpened?.Invoke(snippet);
        }
    }

    public void PlayAnimations()
    {
        canvasGroup.alpha = 0;

        var sequence = DOTween.Sequence().SetId(DOTWEEN_ID);

        sequence
        .Append(canvasGroup.DOFade(1, animDuration).SetEase(easeFade))
        .Join(canvasGroup.transform.DOScale(scaleValue, animDuration).SetEase(easeScaleIn))
        .Append(canvasGroup.transform.DOScale(1f, animDuration).SetEase(easeScaleOut));

        sequence.Play();
    }

    public void Conclude()
    {
        DOTween.Kill(DOTWEEN_ID);
        snippetButton.onClick.RemoveAllListeners();
    }
}
