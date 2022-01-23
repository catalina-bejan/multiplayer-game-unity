using UnityEngine;
using UnityEngine.UI;

public class MessageListing : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public void SetMessageInfo(string sender, string message)
    {
        text.text = sender + ": " + message;
    }
}