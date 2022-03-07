using UnityEngine;
using UnityEngine.EventSystems;
public class anNarrateSelectable : MonoBehaviour, ISelectHandler
{
    public string NarrateOnSelect;
    public void OnSelect(BaseEventData eventData)
    {
        anMenuNarrator.Instance.NarrateMessage(NarrateOnSelect);
    }
}