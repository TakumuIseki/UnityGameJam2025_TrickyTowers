using UnityEngine;
using Cinemachine;

/// <summary>
/// カメラ制御
/// </summary>
public class CameraConttoller : MonoBehaviour
{
    /// <summary>
    /// インスタンス
    /// </summary>
    public static CameraConttoller Instance { get; private set; }

    [Header("バーチャルカメラ"),SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// 揺れタイマー
    /// </summary>
    private float _shakeTimer;

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // シングルトン
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if(_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            // 揺れ時間が終了したら揺れを止める
            if(_shakeTimer <= 0)
            {
                var cinemachineBasicMultiChannelPerlin =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    public void Shake(float intensity,float time)
    {
        // カメラのCinemachineBasicMultiChannelPerlinコンポーネントを取得
        var cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // 揺れの強さと時間を設定
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _shakeTimer = time;
    }
}
