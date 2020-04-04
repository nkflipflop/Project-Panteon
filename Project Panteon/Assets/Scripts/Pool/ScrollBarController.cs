using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

    public class ScrollBarController : MonoBehaviour, IScrollHandler {
        public int totalCount = -1;                 // Total count, negative means INFINITE mode
        public Pool pool;                           // Pool for Production Menu

        private float _threshold = 1;
        private int _firstIndex = 0;
        private int _lastIndex = 0;
        private bool _onTop = true;

        private int contentConstraintCount {
            get {
                int ConstraintCount = 1;
                if (_content != null) {
                    GridLayoutGroup layout = _content.GetComponent<GridLayoutGroup>();
                    ConstraintCount = layout.constraintCount;
                }
                return ConstraintCount;
            }
        }

        [SerializeField]
        protected RectTransform _content;
        private RectTransform _viewRect {get { return (RectTransform) transform;}}
        private Vector3[] _corners = new Vector3[4];


        void Start() {
            pool.InitPool();
            
            // Filling ScrollBar
            float sizeFilled = 0;
            while (sizeFilled < _viewRect.rect.size.y)
                sizeFilled += NewItemAtEnd();
        }

        public  void OnScroll(PointerEventData data) {
            if (!_onTop || data.scrollDelta.y < 0)
                _content.anchoredPosition -= (data.scrollDelta * 100);
                
            _onTop = false;
            UpdateItems();
        }

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

        protected float NewItemAtStart() {
            if (_firstIndex - contentConstraintCount < 0) {
                _onTop = true;
                return 0;
            }

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++) {
                _firstIndex--;
                RectTransform newItem = InstantiateNextItem(_firstIndex);
                newItem.SetAsFirstSibling();
                size = LayoutUtility.GetPreferredHeight(newItem);
            }

            _threshold =  size * 1.5f;
            _content.anchoredPosition +=  new Vector2 (0, size + 5);        
            return size;
        }

        protected float DeleteItemAtStart() {
            if (( totalCount >= 0 && _lastIndex >= totalCount - 1) || _content.childCount == 0)
                return 0;

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++) {
                RectTransform oldItem = _content.GetChild(0) as RectTransform;
                size = LayoutUtility.GetPreferredHeight(oldItem);
                pool.ReturnObjectToPool(oldItem);
                _firstIndex++;
            }

            _content.anchoredPosition -=  new Vector2 (0, size + 5);
            return size;
        }

        protected float NewItemAtEnd() {
            if (totalCount >= 0 && _lastIndex >= totalCount)
                return 0;

            float size = 0;
            int count = contentConstraintCount - (_content.childCount % contentConstraintCount);
            for (int i = 0; i < count; i++) {
                RectTransform newItem = InstantiateNextItem(_lastIndex);
                size = LayoutUtility.GetPreferredHeight(newItem);
                _lastIndex++;
                if (totalCount >= 0 && _lastIndex >= totalCount)
                    break;
            }

            _threshold = size * 1.5f;
            return size;
        }

        protected float DeleteItemAtEnd() {
            if ((totalCount >= 0 && _firstIndex < contentConstraintCount) || _content.childCount == 0)
                return 0;

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++) {
                RectTransform oldItem = _content.GetChild(_content.childCount - 1) as RectTransform;
                size = LayoutUtility.GetPreferredHeight(oldItem);
                pool.ReturnObjectToPool(oldItem);

                _lastIndex--;
                if (_lastIndex % contentConstraintCount == 0 || _content.childCount == 0)
                    break;  // Just delete the whole row
            }
            
            return size;
        }

        private RectTransform InstantiateNextItem(int itemIndex) { 
            RectTransform nextItem = pool.PopObject().transform as RectTransform;
            nextItem.transform.SetParent(_content, false);
            nextItem.gameObject.SetActive(true);
            nextItem.transform.SendMessage("CellIndex", itemIndex);
            return nextItem;
        }
    }