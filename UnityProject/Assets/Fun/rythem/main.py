import librosa
import numpy as np

# 加载音频
filename = "rythem2.mp3"
y, sr = librosa.load(filename)

# -------------------------
# 1. 检测整体 BPM + beat 时间
# -------------------------
tempo, beat_frames = librosa.beat.beat_track(y=y, sr=sr)

# 处理 tempo 可能是数组的情况
if isinstance(tempo, np.ndarray):
    tempo = float(tempo[0])

beat_times = librosa.frames_to_time(beat_frames, sr=sr)

# 四舍五入到整数 BPM
bpm_int = round(float(tempo))

# -------------------------
# 2. 检测 onsets（音符起点）
# -------------------------
onset_frames = librosa.onset.onset_detect(y=y, sr=sr)
onset_times = librosa.frames_to_time(onset_frames, sr=sr)

# -------------------------
# 3. 输出结果
# -------------------------
print("估算 BPM (浮点):", tempo)
print("估算 BPM (整数):", bpm_int)

if len(onset_times) > 0:
    print("第一个 Onset (音符起点):", onset_times[0], "秒")
else:
    print("未检测到 Onset")

if len(beat_times) > 0:
    print("第一个 Beat (节拍):", beat_times[0], "秒")
else:
    print("未检测到 Beat")
