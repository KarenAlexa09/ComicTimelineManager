using System;
using UnityEngine;

[Serializable]
public class StoryEntity
{
    [SerializeField] private string title;
    [SerializeField] private StorysTypes storysType;
    [SerializeField] private float goal = 0;
    [SerializeField] private bool _isCompleted;
    [SerializeField] private float _currentProgress = 0f;

    public StorysTypes StorysType { get { return storysType; } }
    public float Goal { get { return goal; } }

    public bool IsCompleted { get { return _isCompleted; } }

    public float CurrentProgress { get { return _currentProgress; } set { _currentProgress = value; } }

    public string Title { get { return title; } }
}
