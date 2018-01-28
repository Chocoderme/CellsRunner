using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class IPickable : MonoBehaviour {

    [Header("Behavior Parameters")]
    [SerializeField] protected bool isEnabled = true;
    public GameObject lastTrigger { get; private set; }
    protected bool isPicked = false;
    public SpriteRenderer _renderer { get; private set; }

    [Header("Color Parameters")]
    [SerializeField] protected bool enablePickColor = true;
    [SerializeField] protected Color normalColor = Color.white;
    [SerializeField] protected Color pickedColor = new Color(0, 0, 0, 0);

    [Header("Lock Time Parameters")]
    [SerializeField] protected bool doRespawn = true;
    [SerializeField] protected float lockTime = 2f;
    private float actualLock = 0;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPicked)
        {
            // CHANGE PICK COLOR IF NEEDED
            if (enablePickColor)
                _renderer.color = pickedColor;

            // RESPAWN PICKABLE IF NEEDED
            if (doRespawn)
            {
                actualLock += Time.deltaTime;
                if (actualLock > lockTime)
                {
                    actualLock = 0f;
                    isPicked = false;
                }
            }
        }
        else
        {
            // RESET PICK COLOR IF NEEDED
            if (enablePickColor)
                _renderer.color = normalColor;
        }

    }

    public virtual bool isPickable()
    {
        return isEnabled && !isPicked;
    }

    public virtual void Pickup()
    {
        if (isPickable())
        {
            isPicked = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isPickable())
        {
            lastTrigger = collision.gameObject;
            Pickup();
        }
    }
}
