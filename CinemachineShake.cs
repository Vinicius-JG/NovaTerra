using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance;
    CinemachineVirtualCamera cvc;

    private void Awake()
    {
        Instance = this;
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin multiChannel =
        cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        multiChannel.m_AmplitudeGain = intensity;

        StartCoroutine(ShakeTime(time));
    }

    IEnumerator ShakeTime(float time)
    {
        yield return new WaitForSeconds(time);

        CinemachineBasicMultiChannelPerlin multiChannel =
                cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        multiChannel.m_AmplitudeGain = 0;
    }
}
