using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class sound_on_select : MonoBehaviour, ISelectHandler {

    public void OnSelect(BaseEventData eventData)
    {
        music_manager.play_by_name("ui");
    }


}
