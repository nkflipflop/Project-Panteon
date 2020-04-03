using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

using UnityEngine.UI;

    public class LoopVerticalScrollRect : UIBehaviour, IScrollHandler
    {
        //==========LoopScrollRect==========
        public LoopScrollPrefabSource prefabSource; // Prefab Source
        public int totalCount;  // Total count, negative means INFINITE mode

        [HideInInspector]
        [NonSerialized]
        public LoopScrollDataSource dataSource = LoopScrollSendIndexSource.Instance;

        protected float threshold = 0;
        protected int itemTypeStart = 0;
        protected int itemTypeEnd = 0;

        protected GridLayoutGroup m_GridLayout = null;

        private int m_ContentConstraintCount = 0;
        protected int contentConstraintCount
        {
            get
            {
                if (m_ContentConstraintCount > 0)
                {
                    return m_ContentConstraintCount;
                }
                m_ContentConstraintCount = 1;
                if (content != null)
                {
                    GridLayoutGroup layout2 = content.GetComponent<GridLayoutGroup>();
                    if (layout2 != null)
                    {
                        if (layout2.constraint == GridLayoutGroup.Constraint.Flexible)
                        {
                            Debug.LogWarning("[LoopScrollRect] Flexible not supported yet");
                        }
                        m_ContentConstraintCount = layout2.constraintCount;
                    }
                }
                return m_ContentConstraintCount;
            }
        }

        //==========LoopScrollRect==========

        [SerializeField]
        private RectTransform m_Content;
        public RectTransform content { get { return m_Content; } set { m_Content = value; } }

        private RectTransform m_ViewRect;
        protected RectTransform viewRect
        {
            get
            {
                if (m_ViewRect == null)
                    m_ViewRect = (RectTransform)transform;
                return m_ViewRect;
            }
        }

        private Bounds m_ContentBounds;
        private Bounds m_ViewBounds;


        //==========LoopScrollRect==========

        public void RefillCells(int offset = 0, bool fillViewRect = false)
        {
            if (!Application.isPlaying || prefabSource == null)
                return;

            itemTypeStart =  offset;
            itemTypeEnd = itemTypeStart;

            if (totalCount >= 0 && itemTypeStart % contentConstraintCount != 0)
                Debug.LogWarning("Grid will become strange since we can't fill items in the first line");

            // Don't `Canvas.ForceUpdateCanvases();` here, or it will new/delete cells to change itemTypeStart/End
            for (int i = m_Content.childCount - 1; i >= 0; i--)
            {
                prefabSource.ReturnObject(m_Content.GetChild(i));
            }

            float sizeToFill = 0, sizeFilled = 0;
            // m_ViewBounds may be not ready when RefillCells on Start
            sizeToFill = viewRect.rect.size.y;

            float itemSize = 0;

            while (sizeToFill > sizeFilled)
            {
                float size =  NewItemAtEnd();
                if(size <= 0) break;
                else itemSize = size;
                sizeFilled += size;
            }

            if (fillViewRect && itemSize > 0 && sizeFilled < sizeToFill)
            {
                int itemsToAddCount = (int)((sizeToFill - sizeFilled) / itemSize);        //calculate how many items can be added above the offset, so it still is visible in the view
                int newOffset = offset - itemsToAddCount;
                if (newOffset < 0) newOffset = 0;
                if (newOffset != offset) RefillCells(newOffset);                 //refill again, with the new offset value, and now with fillViewRect disabled.
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
                size = Mathf.Max(GetSize(newItem), size);
            }
            threshold = Mathf.Max(threshold, size * 1.5f);

    
                Vector2 offset = new Vector2 (0, size);
                content.anchoredPosition += offset;
            
            return size;
        }

        protected float DeleteItemAtStart()
        {
            // special case: when moving or dragging, we cannot simply delete start when we've reached the end
            if (( totalCount >= 0 && itemTypeEnd >= totalCount - 1) 
                || content.childCount == 0)
            {
                return 0;
            }

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                RectTransform oldItem = content.GetChild(0) as RectTransform;
                size = Mathf.Max(GetSize(oldItem), size);
                prefabSource.ReturnObject(oldItem);

                itemTypeStart++;

                if (content.childCount == 0)
                {
                    break;
                }
            }


                Vector2 offset = new Vector2 (0, size);
                content.anchoredPosition -= offset;
            return size;
        }

        protected float NewItemAtEnd()
        {
            if (totalCount >= 0 && itemTypeEnd >= totalCount)
            {
                return 0;
            }
            float size = 0;
            // issue 4: fill lines to end first
            int count = contentConstraintCount - (content.childCount % contentConstraintCount);
            for (int i = 0; i < count; i++)
            {
                RectTransform newItem = InstantiateNextItem(itemTypeEnd);
                size = Mathf.Max(GetSize(newItem), size);
                itemTypeEnd++;
                if (totalCount >= 0 && itemTypeEnd >= totalCount)
                {
                    break;
                }
            }
            threshold = Mathf.Max(threshold, size * 1.5f);


            
            return size;
        }

        protected float DeleteItemAtEnd()
        {
            if ((totalCount >= 0 && itemTypeStart < contentConstraintCount) 
                || content.childCount == 0)
            {
                return 0;
            }

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                RectTransform oldItem = content.GetChild(content.childCount - 1) as RectTransform;
                size = Mathf.Max(GetSize(oldItem), size);
                prefabSource.ReturnObject(oldItem);

                itemTypeEnd--;
                if (itemTypeEnd % contentConstraintCount == 0 || content.childCount == 0)
                {
                    break;  //just delete the whole row
                }
            }


            return size;
        }

        private RectTransform InstantiateNextItem(int itemIdx)
        {            
            RectTransform nextItem = prefabSource.GetObject().transform as RectTransform;
            nextItem.transform.SetParent(content, false);
            nextItem.gameObject.SetActive(true);
            dataSource.ProvideData(nextItem, itemIdx);
            return nextItem;
        }
        //==========LoopScrollRect==========  

        public  void OnScroll(PointerEventData data)
        {
            UpdateBounds();

            Vector2 delta = data.scrollDelta;
            // Down is positive for scroll events, while in UI system up is positive.
            delta.y *= -1;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                delta.y = delta.x;
            delta.x = 0;

            Vector2 position = m_Content.anchoredPosition;
            position += delta * 25;
            SetContentAnchoredPosition(position);
            UpdateBounds();
        }

        protected  void SetContentAnchoredPosition(Vector2 position)
        {
            if (position != m_Content.anchoredPosition)
            {
                m_Content.anchoredPosition = position;
                UpdateBounds(true);
            }
        }

        private void UpdateBounds(bool updateItems = false)
        {
            m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            m_ContentBounds = GetBounds();

            if (m_Content == null)
                return;

            // ============LoopScrollRect============
            // Don't do this in Rebuild
            if (Application.isPlaying && updateItems && UpdateItems(m_ViewBounds, m_ContentBounds))
            {
                Canvas.ForceUpdateCanvases();
                m_ContentBounds = GetBounds();
            }
            // ============LoopScrollRect============

            // Make sure content bounds are at least as large as view by adding padding if not.
            // One might think at first that if the content is smaller than the view, scrolling should be allowed.
            // However, that's not how scroll views normally work.
            // Scrolling is *only* possible when content is *larger* than view.
            // We use the pivot of the content rect to decide in which directions the content bounds should be expanded.
            // E.g. if pivot is at top, bounds are expanded downwards.
            // This also works nicely when ContentSizeFitter is used on the content.
            Vector3 contentSize = m_ContentBounds.size;
            Vector3 contentPos = m_ContentBounds.center;
            Vector3 excess = m_ViewBounds.size - contentSize;
            if (excess.x > 0)
            {
                contentPos.x -= excess.x * (m_Content.pivot.x - 0.5f);
                contentSize.x = m_ViewBounds.size.x;
            }
            if (excess.y > 0)
            {
                contentPos.y -= excess.y * (m_Content.pivot.y - 0.5f);
                contentSize.y = m_ViewBounds.size.y;
            }

            m_ContentBounds.size = contentSize;
            m_ContentBounds.center = contentPos;
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
    
        protected  float GetSize(RectTransform item)
        {
            float size = 5;
            if (m_GridLayout != null)
            {
                size += m_GridLayout.cellSize.y;
            }
            else
            {
                size += LayoutUtility.GetPreferredHeight(item);
            }
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