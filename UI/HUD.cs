using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] Image lifeBar;
    [SerializeField] Image shieldBar;
    [SerializeField] Image shieldIcon;
    [SerializeField] Image frame;
    [SerializeField] Image nameUI;

    Color startColor;
    [SerializeField] Sprite shieldSprite;
    [SerializeField] Sprite warnSprite;

    [SerializeField] GameObject warning;
    [SerializeField] AudioClip warningSFX;
    [SerializeField] AudioClip warningSound;

    [SerializeField] TextMeshProUGUI shieldTMP;
    [SerializeField] TextMeshProUGUI lifeTMP;
    [SerializeField] TextMeshProUGUI granadeCount;

    private void Awake()
    {
        startColor = lifeBar.color;
    }

    private void Start()
    {
        lifeBar.color = new Color32(44, 230, 232, 255);
        shieldBar.color = new Color32(44, 230, 232, 255);
        nameUI.color = Color.white;
        shieldIcon.color = new Color32(44, 230, 232, 255);
    }

    public void UpdateBars()
    {
        shieldBar.fillAmount = player.shield / player.maxShield;
        lifeBar.fillAmount = player.health / player.maxHealth;
        /*
                if (shieldBar.fillAmount > 0)
                    shieldTMP.text = "" + Mathf.Round(shieldBar.fillAmount * 100);
                else
                    shieldTMP.text = "";

                if (lifeBar.fillAmount > 0)
                    lifeTMP.text = "" + Mathf.Round(lifeBar.fillAmount * 100);
                else
                    lifeTMP.text = "";
        */
        //shieldTMP.rectTransform.localPosition = new Vector3((shieldBar.rectTransform.sizeDelta.x - shieldBar.rectTransform.sizeDelta.x * shieldBar.fillAmount), shieldTMP.rectTransform.localPosition.y, shieldTMP.rectTransform.localPosition.z);
        if (shieldBar.fillAmount <= 0)
            Warning();
        else
            Normal();

        granadeCount.text = player.granadeAmmo.ToString();

        lifeTMP.rectTransform.anchoredPosition = new Vector2((-370 + 370 * lifeBar.fillAmount) + 13, lifeTMP.rectTransform.anchoredPosition.y);
        lifeTMP.text = "" + Mathf.RoundToInt(lifeBar.fillAmount * 100);

        shieldTMP.rectTransform.anchoredPosition = new Vector2((-370 + 370 * shieldBar.fillAmount) + 13, shieldTMP.rectTransform.anchoredPosition.y);
        shieldTMP.text = "" + Mathf.RoundToInt(shieldBar.fillAmount * 100);

        if (shieldBar.fillAmount <= 0)
            shieldTMP.gameObject.SetActive(false);
        else
            shieldTMP.gameObject.SetActive(true);

        lifeBar.color = new Color32(44, 230, 232, 255);
        shieldBar.color = new Color32(44, 230, 232, 255);
        nameUI.color = Color.white;
        shieldIcon.color = new Color32(44, 230, 232, 255);
    }

    public void Warning()
    {
        nameUI.color = Color.red;
        frame.color = Color.red;
        lifeBar.color = Color.red;
        shieldBar.color = Color.red;
        shieldIcon.color = Color.white;
        shieldIcon.sprite = warnSprite;
        shieldIcon.SetNativeSize();

        if (!warning.activeSelf)
        {
            warning.SetActive(true);
            AudioManager.Instance.PlaySFX(warningSFX);
            InvokeRepeating("DetectWarningState", 0, 3);
        }
    }

    void DetectWarningState()
    {
        if (warning.activeSelf)
            AudioManager.Instance.PlaySFX(warningSound);
        else
            CancelInvoke("DetectWarningState");
    }

    void Normal()
    {
        nameUI.color = startColor;
        frame.color = startColor;
        lifeBar.color = startColor;
        shieldBar.color = startColor;
        shieldIcon.color = startColor;
        shieldIcon.sprite = shieldSprite;
        shieldIcon.SetNativeSize();
        warning.SetActive(false);
    }

}
