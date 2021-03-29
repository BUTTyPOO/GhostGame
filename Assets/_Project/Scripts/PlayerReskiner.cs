using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerReskiner : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public IntRef playerSkinIndex;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSkinIndex.value = Saver.LoadSkin();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerSkinIndex.value == 0)
            return;
        
        var subSprites = Resources.LoadAll<Sprite>("PlayerSprites/" + "GhostSprites" + playerSkinIndex.value);

        string spriteName = spriteRenderer.sprite.name;
        int i = int.Parse(spriteName[spriteName.Length - 1].ToString());    // last char of string
        var newSprite = Array.Find(subSprites, item => item.name.Substring(item.name.Length - 2) == spriteName.Substring(spriteName.Length - 2));
        spriteRenderer.sprite = newSprite;
    }
}
