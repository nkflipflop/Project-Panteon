using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

    public class ScrollBarController : MonoBehaviour, IScrollHandler {
        public int totalCount = -1;                 // Total count, negative means INFINITE mode
        public Pool pool;                           // Pool for Production Menu

        private float _threshold = 1;
        private int itemTypeStart = 0;
        private int itemTypeEnd = 0;

        private int contentConstraintCount {
            get
            {
                int ConstraintCount = 1;
                if (_content != null)
                {
                    GridLayoutGroup layout2 = _content.GetComponent<GridLayoutGroup>();
                    ConstraintCount = layout2.constraintCount;
                }
                return ConstraintCount;
            }
        }

        [SerializeField]
        protected RectTransform _content;
        private RectTransform viewRect {
            get {
                return (RectTransform) transform;
            }
        }
        private Vector3[] _corners = new Vector3[4];

        private Bounds m_ContentBounds;
        private Bounds m_ViewBounds;

        void Start() {
            pool.InitPool();
            FillScrollBar();
        }

        public void FillScrollBar() {
            itemTypeStart =  0;
            itemTypeEnd = 0;

            for (int i = _content.childCount - 1; i >= 0; i--)
                pool.ReturnObjectToPool(_content.GetChild(i));

            float sizeToFill = 0, sizeFilled = 0;
            sizeToFill = viewRect.rect.size.y;


            while (sizeToFill > sizeFilled)
                sizeFilled += NewItemAtEnd();


            Vector2 pos = _content.anchoredPosition;
            pos.y = 0;
            _content.anchoredPosition = pos;
        }

        protected float NewItemAtStart()
        {
            if (totalCount >= 0 && itemTypeStart - contentConstraintCount < 0)
                return 0;

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                itemTypeStart--;
                RectTransform newItem = InstantiateNextItem(itemTypeStart);
                newItem.SetAsFirstSibling();
                size = LayoutUtility.GetPreferredHeight(newItem);
            }

            _threshold =  size * 1.5f;
    
            _content.anchoredPosition +=  new Vector2 (0, size);        
            return size;
        }

        protected float DeleteItemAtStart()
        {
            // special case: when moving or dragging, we cannot simply delete start when we've reached the end
            if (( totalCount >= 0 && itemTypeEnd >= totalCount - 1) || _content.childCount == 0)
                return 0;

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                RectTransform oldItem = _content.GetChild(0) as RectTransform;
                size = LayoutUtility.GetPreferredHeight(oldItem);
                pool.ReturnObjectToPool(oldItem);

                itemTypeStart++;

            }

            _content.anchoredPosition -=  new Vector2 (0, size);
            return size;
        }

        protected float NewItemAtEnd() {
            if (totalCount >= 0 && itemTypeEnd >= totalCount)
                return 0;

            float size = 0;
            int count = contentConstraintCount - (_content.childCount % contentConstraintCount);
            for (int i = 0; i < count; i++)
            {
                RectTransform newItem = InstantiateNextItem(itemTypeEnd);
                size = LayoutUtility.GetPreferredHeight(newItem);
                itemTypeEnd++;
                if (totalCount >= 0 && itemTypeEnd >= totalCount)
                    break;
            }

            _threshold = size * 1.5f;
            return size;
        }

        protected float DeleteItemAtEnd() {
            if ((totalCount >= 0 && itemTypeStart < contentConstraintCount) || _content.childCount == 0)
                return 0;

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++) {
                RectTransform oldItem = _content.GetChild(_content.childCount - 1) as RectTransform;
                size = LayoutUtility.GetPreferredHeight(oldItem);
                pool.ReturnObjectToPool(oldItem);

                itemTypeEnd--;
                if (itemTypeEnd % contentConstraintCount == 0 || _content.childCount == 0)
                    break;  //just delete the whole row
            }
            
            return size;
        }

        private RectTransform InstantiateNextItem(int itemIndex) { 
            RectTransform nextItem = pool.GetObjectFromPool().transform as RectTransform;
            nextItem.transform.SetParent(_content, false);
            nextItem.gameObject.SetActive(true);
            nextItem.transform.SendMessage("ScrollCellIndex", itemIndex);
            return nextItem;
        }

        public  void OnScroll(PointerEventData data) {
            _content.anchoredPosition -= (data.scrollDelta * 100);

            m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            Debug.Log(m_ViewBounds);
            m_ContentBounds = GetBounds();
            UpdateItems(m_ViewBounds, m_ContentBounds);
        }

        private Bounds GetBounds()
        {
            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = viewRect.worldToLocalMatrix;
            _content.GetWorldCorners(_corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(_corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }
    
        private  void UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            if (viewBounds.min.y < contentBounds.min.y)
            {
                float totalSize = NewItemAtEnd();
                while ( viewBounds.min.y < contentBounds.min.y - totalSize)
                    totalSize += NewItemAtEnd();
            }

            if (viewBounds.max.y > contentBounds.max.y)
            {
                float totalSize = NewItemAtStart();
                while ( viewBounds.max.y > contentBounds.max.y + totalSize)
                    totalSize += NewItemAtStart();
            }

            if (viewBounds.min.y > contentBounds.min.y )
            {
                float totalSize = DeleteItemAtEnd();
                while ( viewBounds.min.y > contentBounds.min.y + _threshold + totalSize)
                    totalSize += DeleteItemAtEnd();
            }

            if (viewBounds.max.y < contentBounds.max.y )
            {
                float totalSize = DeleteItemAtStart();
                while (viewBounds.max.y < contentBounds.max.y - _threshold - totalSize)
                    totalSize += DeleteItemAtStart();
            }
        }
    }