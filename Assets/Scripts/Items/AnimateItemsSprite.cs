using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimateItemsSprite : MonoBehaviour {

    private SpriteRenderer _renderer;

    [SerializeField] private Sprite[] frames;
    [SerializeField] private float animationTime = .5f;
    private float time = 0f;
    private int currentIndex = 0;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (frames.Length > 0 && _renderer != null)
        {
            time += Time.deltaTime;
            if (time > animationTime)
            {
                time -= animationTime;
                currentIndex++;
                if (currentIndex > frames.Length - 1)
                    currentIndex = 0;
            }
            _renderer.sprite = frames[currentIndex];
        }
    }
}
