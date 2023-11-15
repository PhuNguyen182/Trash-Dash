using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnClickButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private AudioClip onClickClip;

    private void Awake()
    {
        button?.onClick.AddListener(() =>
        {
            MusicController.Instance.PlayOneShot(onClickClip);
        });
    }
}
