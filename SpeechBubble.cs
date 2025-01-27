﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]
    TextMeshPro text;

    [SerializeField]
    GameObject BG;

    [SerializeField]
    SpriteRenderer Player;
    [SerializeField]
    SpriteRenderer Enemy;

    [SerializeField]
    Sprite defaultPEmotion, defaultEEmotion;

    bool open = false;

    public delegate void StatusChanged(bool on);
    public StatusChanged statusChanged;

    DialogueSequence ds;

    int cooldown = 0;

    bool reverse = false;

    public void Say(DialogueSequence ds, bool rev = false)
    {
        this.reverse = rev;
        this.ds = ds;
        ds.SetI(0);

        Dialogue cd = ds.GetNextDialogue();

        this.text.text = cd.text;

        if(!reverse)
        {
            EmotionDatabase.INSTANCE.SetCharacterEmotion(cd.playerEmotion, EmotionDatabase.INSTANCE.PlayerSprite, Player);
            EmotionDatabase.INSTANCE.SetCharacterEmotion(cd.enemyEmotion, EmotionDatabase.INSTANCE.EnemySprite, Enemy);
        }
        else
        {
            EmotionDatabase.INSTANCE.SetCharacterEmotion(cd.playerEmotion, EmotionDatabase.INSTANCE.EnemySprite, Enemy);
            EmotionDatabase.INSTANCE.SetCharacterEmotion(cd.enemyEmotion, EmotionDatabase.INSTANCE.PlayerSprite, Player);
        }

        this.text.gameObject.SetActive(true);

        BG.SetActive(true);

        statusChanged?.Invoke(true);

        open = true;

        cooldown = 20;
    }

    private void Update()
    {
        cooldown--;

        if (cooldown > 0) { return; }
        if(Input.GetMouseButtonDown(0))
        {
            if(open)
            {
                Dialogue d = ds.GetNextDialogue();
                if (d == null)
                {
                    text.gameObject.SetActive(false);
                    statusChanged?.Invoke(false);
                    BG.SetActive(false);
                    open = false;
                    Player.sprite = defaultPEmotion;
                    Enemy.sprite = defaultEEmotion;
                }
                else
                {
                    this.text.text = d.text;

                    if (!reverse)
                    {
                        EmotionDatabase.INSTANCE.SetCharacterEmotion(d.playerEmotion, EmotionDatabase.INSTANCE.PlayerSprite, Player);
                        EmotionDatabase.INSTANCE.SetCharacterEmotion(d.enemyEmotion, EmotionDatabase.INSTANCE.EnemySprite, Enemy);
                    }
                    else
                    {
                        EmotionDatabase.INSTANCE.SetCharacterEmotion(d.playerEmotion, EmotionDatabase.INSTANCE.EnemySprite, Enemy);
                        EmotionDatabase.INSTANCE.SetCharacterEmotion(d.enemyEmotion, EmotionDatabase.INSTANCE.PlayerSprite, Player);
                    }

                    this.text.gameObject.SetActive(true);
                    BG.SetActive(true);
                }
            }
        }
    }


}
