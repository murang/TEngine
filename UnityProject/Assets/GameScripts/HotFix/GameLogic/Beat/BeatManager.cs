using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using YooAsset;

namespace GameLogic
{
    public struct BeatInfo
    {
        public int BeatIndex;
        public double TimeIntoBeat;
        public double TimeToNextBeat;
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class BeatManager : SingletonBehaviour<BeatManager>
    {
        private double _bpm;
        private double _offset;
        private double _beatInterval;
        private AudioSource _audioSource;
        private double _startPlay;
        private double _pauseOffset;
        private double _pauseStart;
        private bool _isPaused;
        private bool _isReady;
        private bool _isPlaying;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_audioSource.clip)
            {
                return;
            }
            
            var beatInfo = GetBeatInfo();
            Log.Debug($"beatInfo: {beatInfo.BeatIndex}, {beatInfo.TimeIntoBeat}, {beatInfo.TimeToNextBeat}");
        }

        public void LoadClip(string clip, double bpm, double offset)
        {
            Stop();
            _audioSource.clip = null;
            _isReady = false;
            _isPlaying = false;
            _isPaused = false;
            
            _bpm = bpm;
            _offset = offset;
            _beatInterval = 60d / bpm;
            AssetHandle handle = GameModule.Resource.LoadAssetAsyncHandle<AudioClip>(clip);
            handle.Completed += OnClipLoadCompleted;
        }

        private void OnClipLoadCompleted(AssetHandle handle)
        {
            _audioSource.clip = handle.AssetObject as AudioClip;
            _isReady = true;
        }

        public BeatInfo GetBeatInfo()
        {
            double playbackTime = GetPlaybackTime() - _offset;

            if (playbackTime < 0)
            {
                return new BeatInfo
                {
                    BeatIndex = -1,
                    TimeIntoBeat = 0,
                    TimeToNextBeat = -playbackTime
                };
            }
            
            var beatIndex = Mathf.FloorToInt((float)(playbackTime / _beatInterval));
            var timeIntoBeat = playbackTime % _beatInterval;
            var timeToNextBeat = _beatInterval - timeIntoBeat;

            return new BeatInfo
            {
                BeatIndex = beatIndex,
                TimeIntoBeat = timeIntoBeat,
                TimeToNextBeat = timeToNextBeat,
            };
        }

        public double GetPlaybackTime()
        {
            if (_audioSource.clip == null)
                return 0;

            double dspNow = _isPaused ? _pauseStart : AudioSettings.dspTime;

            double elapsed = (dspNow - _startPlay - _pauseOffset) * _audioSource.pitch;

            if (_audioSource.loop)
            {
                double length = _audioSource.clip.length;
                elapsed %= length;
            }

            return elapsed;
        }

        public void Play()
        {
            if (_audioSource.clip == null)
            {
                Log.Warning("BeatManager play clip is null");
                return;
            }

            _startPlay = AudioSettings.dspTime;
            _pauseOffset = 0d;

            _audioSource.PlayScheduled(_startPlay);
            _isPlaying = true;
        }

        public void Pause()
        {
            if (!_isPaused)
            {
                _audioSource.Pause();
                _pauseStart = AudioSettings.dspTime;
                _isPaused = true;
                _isPlaying = false;
            }
        }

        public void Resume()
        {
            if (_isPaused)
            {
                _audioSource.UnPause();
                _pauseOffset += AudioSettings.dspTime - _pauseStart;
                _isPaused = false;
                _isPlaying = true;
            }
        }

        public void Stop()
        {
            _audioSource.Stop();
            _isPlaying = false;
            _isPaused = false;
        }

        public bool IsPlaying
        {
            get => _isPlaying;
        }
        
        public bool IsPaused
        {
            get => _isPaused;
        }

        public bool IsReady
        {
            get => _isReady;
        }
    }
}