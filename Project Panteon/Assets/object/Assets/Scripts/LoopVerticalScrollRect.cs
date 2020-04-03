using UnityEngine;
using UnityEngine.EventSystems;
using System;

using UnityEngine.UI;

    public class LoopVerticalScrollRect : MonoBehaviour, IScrollHandler {
        public int totalCount = -1;     // Total count, negative means INFINITE mode
        public LoopScrollPrefabSource prefabSource; // Prefab Source

        [HideInInspector]
        [NonSerialized]
        public LoopScrollDataSource dataSource = LoopScrollDataSource.Instance;

        protected float threshold = 1;
        protected int itemTypeStart = 0;
        protected int itemTypeEnd = 0;

        protected int contentConstraintCount {
            get
            {
                int ConstraintCount = 1;
                if (content != null)
                {
                    GridLayoutGroup layout2 = content.GetComponent<GridLayoutGroup>();
                    ConstraintCount = layout2.constraintCount;
                }
                return ConstraintCount;
            }
        }

        //==========LoopScrollRect==========

        [SerializeField]
        private RectTransform m_Content;
        public RectTransform content { get { return m_Content; } set { m_Content = value; } }

        protected RectTransform viewRect {
            get {
                return (RectTransform) transform;
            }
        }

        private Bounds m_ContentBounds;
        private Bounds m_ViewBounds;

        void Start() {
            RefillCells();
        }

        public void RefillCells() {
            itemTypeStart =  0;
            itemTypeEnd = 0;


            for (int i = m_Content.childCount - 1; i >= 0; i--)
                prefabSource.ReturnObject(m_Content.GetChild(i));

            float sizeToFill = 0, sizeFilled = 0;
            sizeToFill = viewRect.rect.size.y;


            while (sizeToFill > sizeFilled) {
                float size =  NewItemAtEnd();
                if(size <= 0) 
                    break;
                sizeFilled += size;
            }


            Vector2 pos = m_Content.anchoredPosition;
            pos.y = 0;
            m_Content.anchoredPosition = pos;
        }

        protected float NewItemAtStart()
        {
            if (totalCount >= 0 && itemTypeStart - contentConstraintCount < 0)
            {
                return 0;
            }
            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                itemTypeStart--;
                RectTransform newItem = InstantiateNextItem(itemTypeStart);
                newItem.SetAsFirstSibling();
                size = GetSize(newItem);
            }
            threshold = Mathf.Max(threshold, size * 1.5f);

    
            content.anchoredPosition +=  new Vector2 (0, size);        
            return size;
        }

        protected float DeleteItemAtStart()
        {
            // special case: when moving or dragging, we cannot simply delete start when we've reached the end
            if (( totalCount >= 0 && itemTypeEnd >= totalCount - 1) || content.childCount == 0)
                return 0;

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                RectTransform oldItem = content.GetChild(0) as RectTransform;
                size = GetSize(oldItem);
                prefabSource.ReturnObject(oldItem);

                itemTypeStart++;

                if (content.childCount == 0)
                    break;
            }


            content.anchoredPosition -=  new Vector2 (0, size);
            return size;
        }

        protected float NewItemAtEnd() {
            if (totalCount >= 0 && itemTypeEnd >= totalCount)
                return 0;

            float size = 0;
            int count = contentConstraintCount - (content.childCount % contentConstraintCount);
            for (int i = 0; i < count; i++)
            {
                RectTransform newItem = InstantiateNextItem(itemTypeEnd);
                size = GetSize(newItem);
                itemTypeEnd++;
                if (totalCount >= 0 && itemTypeEnd >= totalCount)
                    break;
            }
            threshold = Mathf.Max(threshold, size * 1.5f);

            
            return size;
        }

        protected float DeleteItemAtEnd() {
            if ((totalCount >= 0 && itemTypeStart < contentConstraintCount) || content.childCount == 0)
                return 0;

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++) {
                RectTransform oldItem = content.GetChild(content.childCount - 1) as RectTransform;
                size = GetSize(oldItem);
                prefabSource.ReturnObject(oldItem);

                itemTypeEnd--;
                if (itemTypeEnd % contentConstraintCount == 0 || content.childCount == 0)
                    break;  //just delete the whole row
            }

            return size;
        }

        private RectTransform InstantiateNextItem(int itemIdx) { 
            RectTransform nextItem = prefabSource.GetObject().transform as RectTransform;
            nextItem.transform.SetParent(content, false);
            nextItem.gameObject.SetActive(true);
            dataSource.ProvideData(nextItem, itemIdx);
            return nextItem;
        }

        public  void OnScroll(PointerEventData data) {
            UpdateBounds();

            Vector2 delta = data.scrollDelta;
            // Down is positive for scroll events, while in UI system up is positive.
            delta.y *= -1;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                delta.y = delta.x;
            delta.x = 0;

            Vector2 position = m_Content.anchoredPosition;
            position += delta * 25;

            if (position != m_Content.anchoredPosition) {
                m_Content.anchoredPosition = position;
                UpdateBounds(true);
            }
            UpdateBounds();
        }

        private void UpdateBounds(bool updateItems = false) {
            m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            m_ContentBounds = GetBounds();

            if (Application.isPlaying && updateItems && UpdateItems(m_ViewBounds, m_ContentBounds)) {
                Canvas.ForceUpdateCanvases();
                m_ContentBounds = GetBounds();
            }
        }

        private readonly Vector3[] m_Corners = new Vector3[4];

        private Bounds GetBounds()
        {
            if (m_Content == null)
                return new Bounds();

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = viewRect.worldToLocalMatrix;
            m_Content.GetWorldCorners(m_Corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(m_Corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }
    
        protected  float GetSize(RectTransform item) {
            float size = 5 + LayoutUtility.GetPreferredHeight(item);
            return size;
        }

        protected  bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            bool changed = false;

            if (viewBounds.min.y < contentBounds.min.y)
            {
                float size = NewItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.min.y < contentBounds.min.y - totalSize)
                {
                    size = NewItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            if (viewBounds.max.y > contentBounds.max.y)
            {
                float size = NewItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.max.y > contentBounds.max.y + totalSize)
                {
                    size = NewItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            if (viewBounds.min.y > contentBounds.min.y + threshold)
            {
                float size = DeleteItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.min.y > contentBounds.min.y + threshold + totalSize)
                {
                    size = DeleteItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            if (viewBounds.max.y < contentBounds.max.y - threshold)
            {
                float size = DeleteItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.max.y < contentBounds.max.y - threshold - totalSize)
                {
                    size = DeleteItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            return changed;
        }
    }