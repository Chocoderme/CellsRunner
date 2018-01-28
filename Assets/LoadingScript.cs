using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour {

    [SerializeField] private GameObject blueLoading;
    [SerializeField] private GameObject redLoading;

    [SerializeField] private bool isShown = false;

    [SerializeField] private Animator wheelAnimator;

    [SerializeField] private bool HideOnStart = false;

    [SerializeField] private GameObject ReadyText;
    [SerializeField] private GameObject GoText;

    public void Toggle()
    {
        if (isShown)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        blueLoading.GetComponent<RectTransform>().localPosition = blueEndPosition;
        redLoading.GetComponent<RectTransform>().localPosition = redEndPosition;
        wheelAnimator.SetBool("Animate", false);
        StartCoroutine(Translate(blueLoading, blueEndPosition, blueInitialPosition, 1f, true));
        StartCoroutine(Translate(redLoading, redEndPosition, redInitialPosition, 1f, false));
    }

    private IEnumerator Translate(GameObject go, Vector3 from, Vector3 to, float duration, bool playAtEnd)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            if (t > duration)
                t = duration;
            go.GetComponent<RectTransform>().localPosition = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t / duration));
            yield return null;
        }

        if (playAtEnd)
            wheelAnimator.SetBool("Animate", true);
    }

    public void Hide()
    {
        blueLoading.GetComponent<RectTransform>().localPosition = blueInitialPosition;
        redLoading.GetComponent<RectTransform>().localPosition = redInitialPosition;
        wheelAnimator.SetBool("Animate", false);
        StartCoroutine(Translate(blueLoading, blueInitialPosition, blueEndPosition, 1f, false));
        StartCoroutine(Translate(redLoading, redInitialPosition, redEndPosition, 1f, false));
    }

    private Vector3 blueInitialPosition;
    private Vector3 blueEndPosition;
    private Vector3 redInitialPosition;
    private Vector3 redEndPosition;
    // Use this for initialization
    void Start () {
        blueInitialPosition = blueLoading.GetComponent<RectTransform>().localPosition;
        blueEndPosition = new Vector3(blueInitialPosition.x - 576, blueInitialPosition.y + 162, blueInitialPosition.z);
        redInitialPosition = redLoading.GetComponent<RectTransform>().localPosition;
        redEndPosition = new Vector3(redInitialPosition.x + 576, redInitialPosition.y - 162, redInitialPosition.z);

        if (!HideOnStart)
        {
            blueLoading.GetComponent<RectTransform>().localPosition = blueEndPosition;
            redLoading.GetComponent<RectTransform>().localPosition = redEndPosition;
        }
        else
        {
            blueLoading.GetComponent<RectTransform>().localPosition = blueInitialPosition;
            redLoading.GetComponent<RectTransform>().localPosition = redInitialPosition;
            wheelAnimator.SetBool("Animate", true);
            wheelAnimator.SetBool("HideWheel", false);
            StartCoroutine(levelStart());
        }
    }

    private IEnumerator levelStart()
    {
        yield return new WaitForSeconds(2f);
        wheelAnimator.SetBool("HideWheel", true);
        yield return new WaitForSeconds(1.5f);
        ReadyText.SetActive(true);
        yield return new WaitForSeconds(.7f);
        Hide();
        yield return new WaitForSeconds(.5f);
        ReadyText.SetActive(false);
        GoText.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        GoText.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
