using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameLogic;
using TEngine;

[RequireComponent(typeof(AudioSource))]
public class ShowBeat : MonoBehaviour
{
    public string clip = "rythem2";
    public double bpm = 123.046875d;
    public double offset = 1.27709750d;
    public SpriteRenderer show;
    public float showTime = .1f;
    
    private bool _isShow;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        BeatManager.Instance.LoadClip(clip, bpm, offset);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (BeatManager.Instance.IsPlaying)
            {
                BeatManager.Instance.Pause();
            }
            else
            {
                if (BeatManager.Instance.IsPaused)
                {
                    BeatManager.Instance.Resume();
                }
                else
                {
                    BeatManager.Instance.Play();
                }
            }
        }

        if (!BeatManager.Instance.IsPlaying)
        {
            return;
        }
        
        var beatInfo = BeatManager.Instance.GetBeatInfo();
        if (!_isShow && beatInfo.TimeToNextBeat < showTime)
        {
            Show();
        }
    }

    void Show()
    {
        _isShow = true;
        var seq = DOTween.Sequence();
        seq.Append(show.DOFade(1, showTime));
        seq.Append(show.DOFade(0, showTime));
        seq.Play().OnComplete(()=>{_isShow = false;});
    }
}
