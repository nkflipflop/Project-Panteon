using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ScrollBarController : MonoBehaviour, IScrollHandler 
{
    public ProductionController productionController;

    public int totalCount = -1;             // Total count, negative means INFINITE mode
    private Pool _pool;                     // Pool for Production Menu

    private float _threshold = 1;
    private int _firstIndex = 0;            // Index of first child of ScrollBar Content
    private int _lastIndex = 0;             // Index of last child of ScrollBar Content
    private bool _onTop = true;             // When index is zero in [0, positive infinite] interval

    [SerializeField]
    protected RectTransform _content;       // Rectangle of Content object of ScrollBar
    private RectTransform _viewRect {get { return (RectTransform) transform;}}  // Bound of children of Content object
    private Vector3[] _corners = new Vector3[4];
    private int contentConstraintCount = 2; // Columns count for grid

    // Initializing the scrollBar content
    public void CreateScrollBar(Pool pool) {
        _pool = pool;
        _pool.InitPool(transform);
        
        // Filling ScrollBar with poolObject
        float sizeFilled = 0;
        while (sizeFilled < _viewRect.rect.size.y)
            sizeFilled += NewItemAtEnd();
    }

    // When Scrolling
    public  void OnScroll(PointerEventData data) {
        // Controlling very top of data (0 point)
        if (!_onTop || data.scrollDelta.y < 0)
            _content.anchoredPosition -= (data.scrollDelta * 50);

        // Updating Content of ScrollBar, if it is possible
        _onTop = false;
        UpdateItems();
    }

    // Updates Content of ScrollBar
    private  void UpdateItems() {
        Bounds contentBounds = GetBounds();
        Bounds viewBounds = new Bounds(_viewRect.rect.center, _viewRect.rect.size);

        if (viewBounds.min.y < contentBounds.min.y) {
            float size = NewItemAtEnd();
            float totalSize = size;
            while (size > 0 && viewBounds.min.y < contentBounds.min.y - totalSize) {
                size = NewItemAtEnd();
                totalSize += size;
            }
        }

        if (viewBounds.max.y > contentBounds.max.y) {
            float size = NewItemAtStart();
            float totalSize = size;
            while (size > 0 && viewBounds.max.y > contentBounds.max.y + totalSize) {
                size = NewItemAtStart();
                totalSize += size;
            }
        }

        if (viewBounds.min.y > contentBounds.min.y + _threshold) {
            float size = DeleteItemAtEnd();
            float totalSize = size;
            while (size > 0 && viewBounds.min.y > contentBounds.min.y + _threshold + totalSize) {
                size = DeleteItemAtEnd();
                totalSize += size;
            }
        }

        if (viewBounds.max.y < contentBounds.max.y - _threshold) {
            float size = DeleteItemAtStart();
            float totalSize = size;
            while (size > 0 && viewBounds.max.y < contentBounds.max.y - _threshold - totalSize) {
                size = DeleteItemAtStart();
                totalSize += size;
            }
        }
    }

    // Gets bound of all children of Content
    private Bounds GetBounds() {
        var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        var toLocal = _viewRect.worldToLocalMatrix;
        _content.GetWorldCorners(_corners);
        for (int i = 0; i < 4; i++) {
            Vector3 v = toLocal.MultiplyPoint3x4(_corners[i]);
            vMin = Vector3.Min(v, vMin);
            vMax = Vector3.Max(v, vMax);
        }

        var bounds = new Bounds(vMin, Vector3.zero);
        bounds.Encapsulate(vMax);
        return bounds;
    }

    // Adds two cells on the top,
    protected float NewItemAtStart() {
        // If very top of data
        if (_firstIndex - contentConstraintCount < 0) {
            _onTop = true;
            return 0;
        }

        // Adding cells
        float size = 0;
        for (int i = 0; i < contentConstraintCount; i++) {
            _firstIndex--;
            RectTransform newItem = BringCell(_firstIndex);
            newItem.SetAsFirstSibling();
            size = LayoutUtility.GetPreferredHeight(newItem);
        }

        _threshold =  size * 1.5f;
        _content.anchoredPosition +=  new Vector2 (0, size + 5);        
        return size;
    }

    // Deletes two cell from the top 
    protected float DeleteItemAtStart() {
        // If data count limited
        if (( totalCount >= 0 && _lastIndex >= totalCount - 1) || _content.childCount == 0)
            return 0;

        // Adding cells
        float size = 0;
        for (int i = 0; i < contentConstraintCount; i++) {
            RectTransform oldItem = _content.GetChild(0) as RectTransform;
            size = LayoutUtility.GetPreferredHeight(oldItem);
            _pool.ReturnObjectToPool(oldItem);
            _firstIndex++;
        }

        _content.anchoredPosition -=  new Vector2 (0, size + 5);
        return size;
    }

    // Adds two cells on the end
    protected float NewItemAtEnd() {
        // If data count limited
        if (totalCount >= 0 && _lastIndex >= totalCount)
            return 0;

        // Adding cells
        float size = 0;
        int count = contentConstraintCount - (_content.childCount % contentConstraintCount);
        for (int i = 0; i < count; i++) {
            RectTransform newItem = BringCell(_lastIndex);
            size = LayoutUtility.GetPreferredHeight(newItem);
            _lastIndex++;
            if (totalCount >= 0 && _lastIndex >= totalCount)
                break;
        }

        _threshold = size * 1.5f;
        return size;
    }

    // Deletes two cells from the end
    protected float DeleteItemAtEnd() {
        // If data count limited
        if ((totalCount >= 0 && _firstIndex < contentConstraintCount) || _content.childCount == 0)
            return 0;

        // Adding cells
        float size = 0;
        for (int i = 0; i < contentConstraintCount; i++) {
            RectTransform oldItem = _content.GetChild(_content.childCount - 1) as RectTransform;
            size = LayoutUtility.GetPreferredHeight(oldItem);
            _pool.ReturnObjectToPool(oldItem);
            _lastIndex--;
            if (_lastIndex % contentConstraintCount == 0 || _content.childCount == 0)
                break;  // Just delete the whole row
        }
        
        return size;
    }

    // Brings a cell from the pool
    private RectTransform BringCell(int itemIndex) { 
        GameObject nextItem = _pool.PopObject();
        nextItem.transform.SetParent(_content, false);
        nextItem.SetActive(true);
        nextItem.GetComponent<PoolCell>().CellIndexing(itemIndex, productionController);
        return nextItem.transform as RectTransform;
    }
}