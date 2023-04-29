using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private RectTransform selectedBtn, previousSelectedBtn;
    public float decelerationRate;
    public Vector2 selectedBtnsize, normalBtnSize;
    public Transform bottomBar;

    private void Awake()
    {
        SnapScroll.OnCurrentPageChanged += SelectedTransformUpdated;
    }

    private void Update()
    {
        if (selectedBtn == null) return;
        if (selectedBtn.transform.position.sqrMagnitude < 1) return;
        // prevent overshooting with values greater than 1
        float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
        selectedBtn.sizeDelta = Vector2.Lerp(selectedBtn.sizeDelta, selectedBtnsize, decelerate);
        transform.position = Vector3.Lerp(transform.position, selectedBtn.position, decelerate);

        if (previousSelectedBtn != null)
            previousSelectedBtn.sizeDelta = Vector2.Lerp(previousSelectedBtn.sizeDelta, normalBtnSize, decelerate);
    }

    private void SelectedTransformUpdated(int id)
    {
        if (previousSelectedBtn != null)
            previousSelectedBtn.sizeDelta = normalBtnSize;

        if (selectedBtn != null)
        {
            previousSelectedBtn = selectedBtn;
            previousSelectedBtn.GetChild(0).GetChild(1).gameObject.SetActive(false);
;        }

        selectedBtn = bottomBar.GetChild(Mathf.Clamp(id, 0, bottomBar.childCount)).GetComponent<RectTransform>();
        selectedBtn.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        SnapScroll.OnCurrentPageChanged -= SelectedTransformUpdated;
    }
}