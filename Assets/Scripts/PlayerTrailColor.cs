using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class PlayerTrailColor : MonoBehaviour {

    private TrailRenderer _renderer;
    private LifePlayer life;

    [SerializeField] private Gradient normalColor;
    [SerializeField] private Gradient infectedColor;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<TrailRenderer>();
        life = GetComponent<LifePlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (life != null)
        {
            if (life.getContaminated())
                _renderer.colorGradient = infectedColor;
            else
                _renderer.colorGradient = normalColor;
        }
	}
}
