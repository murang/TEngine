using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Rythem : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 1024;
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

    public float sensitivity = 1.5f; // 阈值敏感度
    public float beatCooldown = 0.2f; // 节奏点最小间隔(秒)，防止重复触发

    private float[] spectrum;
    private float lastBeatTime;
    
    // Start is called before the first frame update
    void Start()
    {
		Application.targetFrameRate = 60;
        spectrum = new float[sampleSize];
        lastBeatTime = -beatCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetSpectrumData(spectrum, 0, fftWindow);
        
        // 取低频区间 (大约前 50 个频段)
        float energy = 0f;
        for (int i = 0; i < 50; i++)
        {
            energy += spectrum[i];
        }

        // 求平均能量
        float avgEnergy = energy / 50f;

        // 判断是否触发 Beat
        if (avgEnergy > sensitivity * GetBackgroundLevel() && 
            Time.time - lastBeatTime > beatCooldown)
        {
            OnBeat();
            lastBeatTime = Time.time;
        }
    }
    
    private float backgroundLevel = 0f;
    float GetBackgroundLevel()
    {
        backgroundLevel = Mathf.Lerp(backgroundLevel, spectrum[0], 0.05f);
        return backgroundLevel;
    }

    void OnBeat()
    {
        Debug.Log("Beat! Time = " + audioSource.time);
        // 在这里触发游戏动作
    }
}
