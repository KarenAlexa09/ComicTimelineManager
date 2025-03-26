using UnityEngine;

public class PlayableDirectorIdentifier : MonoBehaviour
{
    public string snippetID;

    public void Initialize(string id)
    {
        snippetID = id;
    }
}
