using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Health, Granade, DoubleJump, Glide, GranadeLauncher }
    public ItemType itemType;

    [SerializeField] AudioClip pickUpSFX;

    void PickUp(Player player)
    {
        if (itemType == ItemType.Health)
            player.Heal(50);

        if (itemType == ItemType.Granade)
        {
            player.granadeAmmo += 5;
            GameObject.FindObjectOfType<HUD>().UpdateBars();
        }


        if (itemType == ItemType.Glide)
            player.GetComponent<Gliding>().canGlide = true;

        if (itemType == ItemType.DoubleJump)
            player.GetComponent<Jump>().extraJumps = 1;

        if (itemType == ItemType.GranadeLauncher)
            player.GetComponentInChildren<Gun>().granadeLauncher = true;

        AudioManager.Instance.PlaySFX(pickUpSFX);

        GetComponent<DialogueTrigger>().TriggerDialogue();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            PickUp(other.GetComponent<Player>());
    }
}
