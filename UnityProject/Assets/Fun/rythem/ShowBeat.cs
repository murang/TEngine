using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class ShowBeat : MonoBehaviour
{
    public AudioClip audioClip;
    public float bpm = 117.45f;
    public float offset = 2.53f;
    public SpriteRenderer show;
    public float showTime = .1f;
    
    private float _beatInterval;
    private AudioSource _audioSource;
    private bool _isShow;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = audioClip;
    }

    // Start is called before the first frame update
    void Start()
    {
        _beatInterval = 60f/bpm;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
            else
            {
                _audioSource.Play();
            }
        }

        if (!_audioSource.isPlaying)
        {
            return;
        }

        if (_audioSource.time < offset - showTime)
        {
            return;
        }
        
        var _dspTime = AudioSettings.dspTime - offset - showTime;

        if (!_isShow && _dspTime%_beatInterval < showTime)
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
