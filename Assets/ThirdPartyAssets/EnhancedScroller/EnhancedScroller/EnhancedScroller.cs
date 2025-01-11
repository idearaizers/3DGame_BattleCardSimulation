using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Enhanced
{
    public delegate void CellViewVisibilityChangedDelegate(EnhancedScrollerCellView cellView);
    public delegate void CellViewWillRecycleDelegate(EnhancedScrollerCellView cellView);
    public delegate void ScrollerScrolledDelegate(EnhancedScroller scroller, Vector2 val, float scrollPosition);
    public delegate void ScrollerSnappedDelegate(EnhancedScroller scroller, int cellIndex, int dataIndex, EnhancedScrollerCellView cellView);
    public delegate void ScrollerScrollingChangedDelegate(EnhancedScroller scroller, bool scrolling);
    public delegate void ScrollerTweeningChangedDelegate(EnhancedScroller scroller, bool tweening);
    public delegate void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView);
    public delegate void CellViewReused(EnhancedScroller scroller, EnhancedScrollerCellView cellView);

    // [RequireComponent(typeof(ScrollRect))]
    public class EnhancedScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private float tweenTimeLeft;
        private bool initialized;
        private bool updateSpacing;
        private ScrollRect scrollRect;
        private RectTransform scrollRectTransform;
        private Scrollbar scrollbar;
        private RectTransform container;
        private HorizontalOrVerticalLayoutGroup layoutGroup;
        private IEnhancedScrollerDelegate ienhancedScrollerDelegate;
        private bool reloadData;
        private bool refreshActive;
        private SmallList<EnhancedScrollerCellView> recycledCellViews = new SmallList<EnhancedScrollerCellView>();
        private LayoutElement firstPadder;
        private LayoutElement lastPadder;
        private RectTransform recycledCellViewContainer;
        private SmallList<float> cellViewSizeArray = new SmallList<float>();
        private SmallList<float> cellViewOffsetArray = new SmallList<float>();
        private SmallList<EnhancedScrollerCellView> activeCellViews = new SmallList<EnhancedScrollerCellView>();
        private int activeCellViewsStartIndex;
        private int activeCellViewsEndIndex;
        private int loopFirstCellIndex;
        private int loopLastCellIndex;
        private float loopFirstScrollPosition;
        private float loopLastScrollPosition;
        private float loopFirstJumpTrigger;
        private float loopLastJumpTrigger;
        private float lastScrollRectSize;
        private bool lastLoop;
        private int snapCellViewIndex;
        private int snapDataIndex;
        private bool snapJumping;
        private bool snapInertia;
        private ScrollbarVisibilityType lastScrollbarVisibility;
        private float singleLoopGroupSize;
        private bool snapBeforeDrag;
        private bool loopBeforeDrag;
        private bool ignoreLoopJump;
        private int dragFingerCount;
        private float lookAheadBefore;
        private float lookAheadAfter;

        [SerializeField]
        private float scrollPosition;

        public float ScrollPosition
        {
            get
            {
                return scrollPosition;
            }
            set
            {
                if (!loop)
                {
                    value = Mathf.Clamp(value, 0, ScrollSize);
                }

                if (scrollPosition != value)
                {
                    scrollPosition = value;

                    if (scrollDirection == ScrollDirectionType.Vertical)
                    {
                        scrollRect.verticalNormalizedPosition = 1.0f - (scrollPosition / ScrollSize);
                    }
                    else
                    {
                        scrollRect.horizontalNormalizedPosition = (scrollPosition / ScrollSize);
                    }
                }
            }
        }

        [SerializeField]
        private ScrollDirectionType scrollDirection;

        [SerializeField]
        private float spacing;

        [SerializeField]
        private RectOffset padding;

        [SerializeField]
        private bool loop;

        [SerializeField]
        private bool loopWhileDragging = true;

        [SerializeField]
        private float maxVelocity;

        [SerializeField]
        private ScrollbarVisibilityType scrollbarVisibility;

        [SerializeField]
        private bool snapping;

        [SerializeField]
        private float snapVelocityThreshold;

        [SerializeField]
        private float snapWatchOffset;

        [SerializeField]
        private float snapJumpToOffset;

        [SerializeField]
        private float snapCellCenterOffset;

        [SerializeField]
        private bool snapUseCellSpacing;

        [SerializeField]
        private TweenType snapTweenType;

        [SerializeField]
        private float snapTweenTime;

        [SerializeField]
        private bool snapWhileDragging;

        public CellViewVisibilityChangedDelegate cellViewVisibilityChanged { get; set; }
        public CellViewWillRecycleDelegate cellViewWillRecycle { get; set; }
        public ScrollerScrolledDelegate scrollerScrolled { get; set; }
        public ScrollerSnappedDelegate scrollerSnapped { get; set; }
        public ScrollerScrollingChangedDelegate scrollerScrollingChanged { get; set; }
        public ScrollerTweeningChangedDelegate scrollerTweeningChanged { get; set; }
        public CellViewInstantiated cellViewInstantiated { get; set; }
        public CellViewReused cellViewReused { get; set; }
        public bool IsScrolling { get; private set; }
        public bool IsTweening { get; private set; }

        public LayoutElement FirstPadder => firstPadder;
        public LayoutElement LastPadder => lastPadder;
        public RectTransform Container => container;
        public int StartCellViewIndex => activeCellViewsStartIndex;
        public int EndCellViewIndex => activeCellViewsEndIndex;
        public ScrollRect ScrollRect => scrollRect;
        public int StartDataIndex => activeCellViewsStartIndex % NumberOfCells;
        public int EndDataIndex => activeCellViewsEndIndex % NumberOfCells;
        public int NumberOfCells => (ienhancedScrollerDelegate != null ? ienhancedScrollerDelegate.GetNumberOfCells(this) : 0);
        public Vector2 Velocity => scrollRect.velocity;

        public float LookAheadBefore
        {
            get
            {
                return lookAheadBefore;
            }
            set
            {
                lookAheadBefore = Mathf.Abs(value);
            }
        }

        public float LookAheadAfter
        {
            get
            {
                return lookAheadAfter;
            }
            set
            {
                lookAheadAfter = Mathf.Abs(value);
            }
        }

        public IEnhancedScrollerDelegate Delegate
        {
            get
            {
                return ienhancedScrollerDelegate;
            }
            set
            {
                ienhancedScrollerDelegate = value;
                reloadData = true;
            }
        }

        public float ScrollSize
        {
            get
            {
                if (scrollDirection == ScrollDirectionType.Vertical)
                {
                    return Mathf.Max(container.rect.height - scrollRectTransform.rect.height, 0);
                }
                else
                {
                    return Mathf.Max(container.rect.width - scrollRectTransform.rect.width, 0);
                }
            }
        }

        public float NormalizedScrollPosition
        {
            get
            {
                var scrollPosition = ScrollPosition;
                return (scrollPosition <= 0 ? 0 : this.scrollPosition / ScrollSize);
            }
        }

        public bool Loop
        {
            get
            {
                return loop;
            }
            set
            {
                if (loop != value)
                {
                    var originalScrollPosition = scrollPosition;
                    loop = value;
                    Resize(false);

                    if (loop)
                    {
                        ScrollPosition = loopFirstScrollPosition + originalScrollPosition;
                    }
                    else
                    {
                        ScrollPosition = originalScrollPosition - loopFirstScrollPosition;
                    }

                    ScrollbarVisibility = scrollbarVisibility;
                }
            }
        }

        public ScrollbarVisibilityType ScrollbarVisibility
        {
            get
            {
                return scrollbarVisibility;
            }
            set
            {
                scrollbarVisibility = value;

                if (scrollbar != null)
                {
                    if (cellViewOffsetArray != null && cellViewOffsetArray.Count > 0)
                    {
                        if (cellViewOffsetArray.Last() < GetScrollRectSize() || loop)
                        {
                            scrollbar.gameObject.SetActive(scrollbarVisibility == ScrollbarVisibilityType.Always);
                        }
                        else
                        {
                            scrollbar.gameObject.SetActive(scrollbarVisibility != ScrollbarVisibilityType.Never);
                        }
                    }
                }
            }
        }

        public float LinearVelocity
        {
            get
            {
                return (scrollDirection == ScrollDirectionType.Vertical ? scrollRect.velocity.y : scrollRect.velocity.x);
            }
            set
            {
                if (scrollDirection == ScrollDirectionType.Vertical)
                {
                    scrollRect.velocity = new Vector2(0, value);
                }
                else
                {
                    scrollRect.velocity = new Vector2(value, 0);
                }
            }
        }

        public float GetScrollRectSize()
        {
            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                return scrollRectTransform.rect.height;
            }
            else
            {
                return scrollRectTransform.rect.width;
            }
        }

        private void Awake()
        {
            GameObject go;

            scrollRect = GetComponent<ScrollRect>();
            scrollRectTransform = scrollRect.GetComponent<RectTransform>();

            if (scrollRect.content != null)
            {
                DestroyImmediate(scrollRect.content.gameObject);
            }

            go = new GameObject("Container", typeof(RectTransform));

            go.transform.SetParent(scrollRectTransform);

            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                go.AddComponent<VerticalLayoutGroup>();
            }
            else
            {
                go.AddComponent<HorizontalLayoutGroup>();
            }

            container = go.GetComponent<RectTransform>();

            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                container.anchorMin = new Vector2(0, 1);
                container.anchorMax = Vector2.one;
                container.pivot = new Vector2(0.5f, 1.0f);
            }
            else
            {
                container.anchorMin = Vector2.zero;
                container.anchorMax = new Vector2(0, 1.0f);
                container.pivot = new Vector2(0, 0.5f);
            }

            container.offsetMax = Vector2.zero;
            container.offsetMin = Vector2.zero;
            container.localPosition = Vector2.zero;
            container.localRotation = Quaternion.identity;
            container.localScale = Vector3.one;

            scrollRect.content = container;

            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                scrollbar = scrollRect.verticalScrollbar;
            }
            else
            {
                scrollbar = scrollRect.horizontalScrollbar;
            }

            layoutGroup = container.GetComponent<HorizontalOrVerticalLayoutGroup>();
            layoutGroup.spacing = spacing;
            layoutGroup.padding = padding;

            layoutGroup.childAlignment = TextAnchor.UpperLeft;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childForceExpandWidth = true;

            scrollRect.horizontal = scrollDirection == ScrollDirectionType.Horizontal;
            scrollRect.vertical = scrollDirection == ScrollDirectionType.Vertical;

            go = new GameObject("First Padder", typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(container, false);
            firstPadder = go.GetComponent<LayoutElement>();

            go = new GameObject("Last Padder", typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(container, false);
            lastPadder = go.GetComponent<LayoutElement>();

            go = new GameObject("Recycled Cells", typeof(RectTransform));
            go.transform.SetParent(scrollRect.transform, false);
            recycledCellViewContainer = go.GetComponent<RectTransform>();
            recycledCellViewContainer.gameObject.SetActive(false);

            lastScrollRectSize = GetScrollRectSize();
            lastLoop = loop;

            initialized = true;
        }

        public void OnBeginDrag(PointerEventData data)
        {
            dragFingerCount++;
            if (dragFingerCount > 1) return;

            snapBeforeDrag = snapping;
            if (!snapWhileDragging)
            {
                snapping = false;
            }

            loopBeforeDrag = loop;
            if (!loopWhileDragging)
            {
                loop = false;
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            dragFingerCount--;
            if (dragFingerCount < 0)
            {
                dragFingerCount = 0;
            }

            snapping = snapBeforeDrag;
            loop = loopBeforeDrag;
        }

        private void Update()
        {
            if (updateSpacing)
            {
                UpdateSpacing(spacing);
                reloadData = false;
            }

            if (reloadData)
            {
                ReloadData();
            }

            if ((loop && lastScrollRectSize != GetScrollRectSize()) || (loop != lastLoop))
            {
                Resize(true);
                lastScrollRectSize = GetScrollRectSize();
                lastLoop = loop;
            }

            if (lastScrollbarVisibility != scrollbarVisibility)
            {
                ScrollbarVisibility = scrollbarVisibility;
                lastScrollbarVisibility = scrollbarVisibility;
            }

            if (LinearVelocity != 0 && !IsScrolling)
            {
                IsScrolling = true;
                if (scrollerScrollingChanged != null)
                {
                    scrollerScrollingChanged(this, true);
                }
            }
            else if (LinearVelocity == 0 && IsScrolling)
            {
                IsScrolling = false;
                if (scrollerScrollingChanged != null)
                {
                    scrollerScrollingChanged(this, false);
                }
            }
        }

        private void OnValidate()
        {
            if (initialized && spacing != layoutGroup.spacing)
            {
                updateSpacing = true;
            }
        }

        private void LateUpdate()
        {
            if (maxVelocity > 0)
            {
                if (scrollDirection == ScrollDirectionType.Horizontal)
                {
                    scrollRect.velocity = new Vector2(Mathf.Clamp(Mathf.Abs(Velocity.x), 0, maxVelocity) * Mathf.Sign(Velocity.x), Velocity.y);
                }
                else
                {
                    scrollRect.velocity = new Vector2(Velocity.x, Mathf.Clamp(Mathf.Abs(Velocity.y), 0, maxVelocity) * Mathf.Sign(Velocity.y));
                }
            }
        }

        private void OnEnable()
        {
            scrollRect.onValueChanged.AddListener(ScrollRectOnValueChanged);
        }

        private void OnDisable()
        {
            scrollRect.onValueChanged.RemoveListener(ScrollRectOnValueChanged);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScrollerCellView cellPrefab)
        {
            var cellView = GetRecycledCellView(cellPrefab);
            if (cellView == null)
            {
                var go = Instantiate(cellPrefab.gameObject);
                cellView = go.GetComponent<EnhancedScrollerCellView>();
                cellView.transform.SetParent(container);
                cellView.transform.localPosition = Vector3.zero;
                cellView.transform.localRotation = Quaternion.identity;

                if (cellViewInstantiated != null)
                {
                    cellViewInstantiated(this, cellView);
                }
            }
            else
            {
                cellView.gameObject.SetActive(true);

                if (cellViewReused != null)
                {
                    cellViewReused(this, cellView);
                }
            }

            return cellView;
        }

        /// <summary>
        /// データの更新
        /// </summary>
        /// <param name="scrollPositionFactor">反映時にその位置にジャンプする</param>
        public void ReloadData(float scrollPositionFactor = 0)
        {
            reloadData = false;

            RecycleAllCells();

            if (ienhancedScrollerDelegate != null)
            {
                Resize(false);
            }

            if (scrollRect == null || scrollRectTransform == null || container == null)
            {
                scrollPosition = 0.0f;
                return;
            }

            scrollPosition = Mathf.Clamp(scrollPositionFactor * ScrollSize, 0, ScrollSize);
            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                scrollRect.verticalNormalizedPosition = 1.0f - scrollPositionFactor;
            }
            else
            {
                scrollRect.horizontalNormalizedPosition = scrollPositionFactor;
            }

            RefreshActive();
        }

        public void RefreshActiveCellView()
        {
            for (int i = 0; i < activeCellViews.Count; i++)
            {
                activeCellViews[i].RefreshCellView();
            }
        }

        public void ClearAll()
        {
            ClearActive();
            ClearRecycled();
        }

        public void ClearActive()
        {
            for (int i = 0; i < activeCellViews.Count; i++)
            {
                DestroyImmediate(activeCellViews[i].gameObject);
            }

            activeCellViews.Clear();
        }

        public void ClearRecycled()
        {
            for (int i = 0; i < recycledCellViews.Count; i++)
            {
                DestroyImmediate(recycledCellViews[i].gameObject);
            }

            recycledCellViews.Clear();
        }

        public void ToggleLoop()
        {
            Loop = !loop;
        }

        public void IgnoreLoopJump(bool ignore)
        {
            ignoreLoopJump = ignore;
        }

        public void SetScrollPositionImmediately(float scrollPosition)
        {
            ScrollPosition = scrollPosition;
            RefreshActive();
        }

        public void JumpToDataIndex
        (
            int dataIndex,
            float scrollerOffset = 0,
            float cellOffset = 0,
            bool useSpacing = true,
            TweenType tweenType = TweenType.Immediate,
            float tweenTime = 0.0f,
            Action jumpComplete = null,
            LoopJumpDirectionType loopJumpDirection = LoopJumpDirectionType.Closest,
            bool forceCalculateRange = false
        )
        {
            var cellOffsetPosition = 0.0f;

            if (cellOffset != 0)
            {
                var cellsize = (ienhancedScrollerDelegate != null ? ienhancedScrollerDelegate.GetCellViewSize(this, dataIndex) : 0);

                if (useSpacing)
                {
                    cellsize += spacing;
                    if (dataIndex > 0 && dataIndex < (NumberOfCells - 1))
                    {
                        cellsize += spacing;
                    }
                }

                cellOffsetPosition = cellsize * cellOffset;
            }

            if (scrollerOffset == 1.0f)
            {
                cellOffsetPosition += padding.bottom;
            }

            var offset = -(scrollerOffset * GetScrollRectSize()) + cellOffsetPosition;

            var newScrollPosition = 0.0f;

            if (loop)
            {
                var numberOfCells = NumberOfCells;

                var set1CellViewIndex = loopFirstCellIndex - (numberOfCells - dataIndex);
                var set2CellViewIndex = loopFirstCellIndex + dataIndex;
                var set3CellViewIndex = loopFirstCellIndex + numberOfCells + dataIndex;

                var set1Position = GetScrollPositionForCellViewIndex(set1CellViewIndex, CellViewPositionType.Before) + offset;
                var set2Position = GetScrollPositionForCellViewIndex(set2CellViewIndex, CellViewPositionType.Before) + offset;
                var set3Position = GetScrollPositionForCellViewIndex(set3CellViewIndex, CellViewPositionType.Before) + offset;
                
                var set1Diff = (Mathf.Abs(scrollPosition - set1Position));
                var set2Diff = (Mathf.Abs(scrollPosition - set2Position));
                var set3Diff = (Mathf.Abs(scrollPosition - set3Position));

                var setOffset = -(scrollerOffset * GetScrollRectSize());

                var currentSet = 0;
                var currentCellViewIndex = 0;
                var nextCellViewIndex = 0;

                if (loopJumpDirection == LoopJumpDirectionType.Up || loopJumpDirection == LoopJumpDirectionType.Down)
                {
                    currentCellViewIndex = GetCellViewIndexAtPosition(scrollPosition - setOffset + 0.0001f);
                    
                    if (currentCellViewIndex < numberOfCells)
                    {
                        currentSet = 1;
                        nextCellViewIndex = dataIndex;
                    }
                    else if (currentCellViewIndex >= numberOfCells && currentCellViewIndex < (numberOfCells * 2))
                    {
                        currentSet = 2;
                        nextCellViewIndex = dataIndex + numberOfCells;
                    }
                    else
                    {
                        currentSet = 3;
                        nextCellViewIndex = dataIndex + (numberOfCells * 2);
                    }
                }

                switch (loopJumpDirection)
                {
                    case LoopJumpDirectionType.Closest:

                        if (set1Diff < set2Diff)
                        {
                            if (set1Diff < set3Diff)
                            {
                                newScrollPosition = set1Position;
                            }
                            else
                            {
                                newScrollPosition = set3Position;
                            }
                        }
                        else
                        {
                            if (set2Diff < set3Diff)
                            {
                                newScrollPosition = set2Position;
                            }
                            else
                            {
                                newScrollPosition = set3Position;
                            }
                        }
                        break;
                    
                    case LoopJumpDirectionType.Up:

                        if (nextCellViewIndex < currentCellViewIndex)
                        {
                            newScrollPosition = (currentSet == 1 ? set1Position : (currentSet == 2 ? set2Position : set3Position));
                        }
                        else
                        {
                            if (currentSet == 1 && (currentCellViewIndex == dataIndex))
                            {
                                newScrollPosition = set1Position - singleLoopGroupSize;
                            }
                            else
                            {
                                newScrollPosition = (currentSet == 1 ? set3Position : (currentSet == 2 ? set1Position : set2Position));
                            }

                        }

                        break;
                    
                    case LoopJumpDirectionType.Down:

                        if (nextCellViewIndex > currentCellViewIndex)
                        {
                            newScrollPosition = (currentSet == 1 ? set1Position : (currentSet == 2 ? set2Position : set3Position));
                        }
                        else
                        {
                            if (currentSet == 3 && (currentCellViewIndex == nextCellViewIndex))
                            {
                                newScrollPosition = set3Position + singleLoopGroupSize;
                            }
                            else
                            {
                                newScrollPosition = (currentSet == 1 ? set2Position : (currentSet == 2 ? set3Position : set1Position));
                            }
                        }

                        break;
                }

                if (useSpacing)
                {
                    newScrollPosition -= spacing;
                }
            }
            else
            {
                newScrollPosition = GetScrollPositionForDataIndex(dataIndex, CellViewPositionType.Before) + offset;
                newScrollPosition = Mathf.Clamp(newScrollPosition - (useSpacing ? spacing : 0), 0, ScrollSize);
            }

            if (newScrollPosition == scrollPosition)
            {
                if (jumpComplete != null)
                {
                    jumpComplete();
                    return;
                }
            }

            StartCoroutine(TweenPosition(tweenType, tweenTime, ScrollPosition, newScrollPosition, jumpComplete, forceCalculateRange));
        }

        public void Snap()
        {
            if (NumberOfCells == 0) return;

            snapJumping = true;

            LinearVelocity = 0;

            snapInertia = scrollRect.inertia;
            scrollRect.inertia = false;

            var snapPosition = ScrollPosition + (GetScrollRectSize() * Mathf.Clamp01(snapWatchOffset));

            snapCellViewIndex = GetCellViewIndexAtPosition(snapPosition);

            snapDataIndex = snapCellViewIndex % NumberOfCells;

            JumpToDataIndex(snapDataIndex, snapJumpToOffset, snapCellCenterOffset, snapUseCellSpacing, snapTweenType, snapTweenTime, SnapJumpComplete);
        }

        public float GetScrollPositionForCellViewIndex(int cellViewIndex, CellViewPositionType insertPosition)
        {
            if (NumberOfCells == 0) return 0;

            if (cellViewIndex < 0)
            {
                cellViewIndex = 0;
            } 

            if (cellViewIndex == 0 && insertPosition == CellViewPositionType.Before)
            {
                return 0;
            }
            else
            {
                if (cellViewIndex < cellViewOffsetArray.Count)
                {
                    if (insertPosition == CellViewPositionType.Before)
                    {
                        return cellViewOffsetArray[cellViewIndex - 1] + spacing + (scrollDirection == ScrollDirectionType.Vertical ? padding.top : padding.left);
                    }
                    else
                    {
                        return cellViewOffsetArray[cellViewIndex] + (scrollDirection == ScrollDirectionType.Vertical ? padding.top : padding.left);
                    }
                }
                else
                {
                    return cellViewOffsetArray[cellViewOffsetArray.Count - 2];
                }
            }
        }

        public float GetScrollPositionForDataIndex(int dataIndex, CellViewPositionType insertPosition)
        {
            return GetScrollPositionForCellViewIndex(loop ? ienhancedScrollerDelegate.GetNumberOfCells(this) + dataIndex : dataIndex, insertPosition);
        }

        public int GetCellViewIndexAtPosition(float position)
        {
            return GetCellIndexAtPosition(position, 0, cellViewOffsetArray.Count - 1);
        }

        public EnhancedScrollerCellView GetCellViewAtDataIndex(int dataIndex)
        {
            for (int i = 0; i < activeCellViews.Count; i++)
            {
                if (activeCellViews[i].DataIndex == dataIndex)
                {
                    return activeCellViews[i];
                }
            }

            return null;
        }

        private void Resize(bool keepPosition)
        {
            var originalScrollPosition = scrollPosition;

            cellViewSizeArray.Clear();
            var offset = AddCellViewSizes();

            if (loop)
            {
                var cellCount = cellViewSizeArray.Count;

                if (offset < GetScrollRectSize())
                {
                    int additionalRounds = Mathf.CeilToInt((float)Mathf.CeilToInt(GetScrollRectSize() / offset) / 2.0f) * 2;
                    DuplicateCellViewSizes(additionalRounds, cellCount);
                    loopFirstCellIndex = cellCount * (1 + (additionalRounds / 2));
                }
                else
                {
                    loopFirstCellIndex = cellCount;
                }

                loopLastCellIndex = loopFirstCellIndex + cellCount - 1;
                DuplicateCellViewSizes(2, cellCount);
            }

            CalculateCellViewOffsets();

            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                container.sizeDelta = new Vector2(container.sizeDelta.x, cellViewOffsetArray.Last() + padding.top + padding.bottom);
            }
            else
            {
                container.sizeDelta = new Vector2(cellViewOffsetArray.Last() + padding.left + padding.right, container.sizeDelta.y);
            }

            if (loop)
            {
                loopFirstScrollPosition = GetScrollPositionForCellViewIndex(loopFirstCellIndex, CellViewPositionType.Before) + (spacing * 0.5f);
                loopLastScrollPosition  = GetScrollPositionForCellViewIndex(loopLastCellIndex, CellViewPositionType.After) - GetScrollRectSize() + (spacing * 0.5f);

                loopFirstJumpTrigger = loopFirstScrollPosition - GetScrollRectSize();
                loopLastJumpTrigger = loopLastScrollPosition + GetScrollRectSize();
            }

            ResetVisibleCellViews();

            if (keepPosition)
            {
                ScrollPosition = originalScrollPosition;
            }
            else
            {
                if (loop)
                {
                    ScrollPosition = loopFirstScrollPosition;
                }
                else
                {
                    ScrollPosition = 0;
                }
            }

            ScrollbarVisibility = scrollbarVisibility;
        }

        private void UpdateSpacing(float spacing)
        {
            updateSpacing = false;
            layoutGroup.spacing = spacing;
            ReloadData(NormalizedScrollPosition);
        }

        private float AddCellViewSizes()
        {
            var offset = 0.0f;
            singleLoopGroupSize = 0;

            for (int i = 0; i < NumberOfCells; i++)
            {
                cellViewSizeArray.Add(ienhancedScrollerDelegate.GetCellViewSize(this, i) + (i == 0 ? 0 : layoutGroup.spacing));
                singleLoopGroupSize += cellViewSizeArray[cellViewSizeArray.Count - 1];
                offset += cellViewSizeArray[cellViewSizeArray.Count - 1];
            }

            return offset;
        }

        private void DuplicateCellViewSizes(int numberOfTimes, int cellCount)
        {
            for (int i = 0; i < numberOfTimes; i++)
            {
                for (int j = 0; j < cellCount; j++)
                {
                    cellViewSizeArray.Add(cellViewSizeArray[j] + (j == 0 ? layoutGroup.spacing : 0));
                }
            }
        }

        private void CalculateCellViewOffsets()
        {
            cellViewOffsetArray.Clear();
            var offset = 0.0f;

            for (int i = 0; i < cellViewSizeArray.Count; i++)
            {
                offset += cellViewSizeArray[i];
                cellViewOffsetArray.Add(offset);
            }
        }

        private EnhancedScrollerCellView GetRecycledCellView(EnhancedScrollerCellView cellPrefab)
        {
            for (int i = 0; i < recycledCellViews.Count; i++)
            {
                if (recycledCellViews[i].CellIdentifier == cellPrefab.CellIdentifier)
                {
                    var cellView = recycledCellViews.RemoveAt(i);
                    return cellView;
                }
            }

            return null;
        }

        private void ResetVisibleCellViews()
        {
            int startIndex;
            int endIndex;

            CalculateCurrentActiveCellRange(out startIndex, out endIndex);

            var i = 0;
            SmallList<int> remainingCellIndices = new SmallList<int>();

            while(i < activeCellViews.Count)
            {
                if (activeCellViews[i].CellIndex < startIndex || activeCellViews[i].CellIndex > endIndex)
                {
                    RecycleCell(activeCellViews[i]);
                }
                else
                {
                    remainingCellIndices.Add(activeCellViews[i].CellIndex);
                    i++;
                }
            }

            if (remainingCellIndices.Count == 0)
            {
                for (i = startIndex; i <= endIndex; i++)
                {
                    AddCellView(i, ListPositionType.Last);
                }
            }
            else
            {
                for (i = endIndex; i >= startIndex; i--)
                {
                    if (i < remainingCellIndices.First())
                    {
                        AddCellView(i, ListPositionType.First);
                    }
                }

                for (i = startIndex; i <= endIndex; i++)
                {
                    if (i > remainingCellIndices.Last())
                    {
                        AddCellView(i, ListPositionType.Last);
                    }
                }
            }

            activeCellViewsStartIndex = startIndex;
            activeCellViewsEndIndex = endIndex;

            SetPadders();
        }

        private void RecycleAllCells()
        {
            while (activeCellViews.Count > 0)
            {
                RecycleCell(activeCellViews[0]);
            }

            activeCellViewsStartIndex = 0;
            activeCellViewsEndIndex = 0;
        }

        private void RecycleCell(EnhancedScrollerCellView cellView)
        {
            if (cellViewWillRecycle != null)
            {
                cellViewWillRecycle(cellView);
            }

            activeCellViews.Remove(cellView);
            recycledCellViews.Add(cellView);

            cellView.transform.gameObject.SetActive(false);

            cellView.DataIndex = 0;
            cellView.CellIndex = 0;
            cellView.Active = false;

            if (cellViewVisibilityChanged != null)
            {
                cellViewVisibilityChanged(cellView);
            }
        }

        private void AddCellView(int cellIndex, ListPositionType listPosition)
        {
            if (NumberOfCells == 0) return;

            var dataIndex = cellIndex % NumberOfCells;
            var cellView = ienhancedScrollerDelegate.GetCellView(this, dataIndex, cellIndex);

            cellView.CellIndex = cellIndex;
            cellView.DataIndex = dataIndex;
            cellView.Active = true;

            cellView.transform.SetParent(container, false);
            cellView.transform.localScale = Vector3.one;

            LayoutElement layoutElement = cellView.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = cellView.gameObject.AddComponent<LayoutElement>();
            }

            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                layoutElement.minHeight = cellViewSizeArray[cellIndex] - (cellIndex > 0 ? layoutGroup.spacing : 0); 
            }
            else
            {
                layoutElement.minWidth = cellViewSizeArray[cellIndex] - (cellIndex > 0 ? layoutGroup.spacing : 0); 
            }

            if (listPosition == ListPositionType.First)
            {
                activeCellViews.AddStart(cellView);
            }
            else
            {
                activeCellViews.Add(cellView);
            }

            if (listPosition == ListPositionType.Last)
            {
                cellView.transform.SetSiblingIndex(container.childCount - 2);
            }
            else if (listPosition == ListPositionType.First)
            {
                cellView.transform.SetSiblingIndex(1);
            }

            if (cellViewVisibilityChanged != null)
            {
                cellViewVisibilityChanged(cellView);
            }
        }

        private void SetPadders()
        {
            if (NumberOfCells == 0) return;

            var firstSize = cellViewOffsetArray[activeCellViewsStartIndex] - cellViewSizeArray[activeCellViewsStartIndex];
            var lastSize = cellViewOffsetArray.Last() - cellViewOffsetArray[activeCellViewsEndIndex];

            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                firstPadder.minHeight = firstSize;
                firstPadder.gameObject.SetActive(firstPadder.minHeight > 0);

                lastPadder.minHeight = lastSize;
                lastPadder.gameObject.SetActive(lastPadder.minHeight > 0);
            }
            else
            {
                firstPadder.minWidth = firstSize;
                firstPadder.gameObject.SetActive(firstPadder.minWidth > 0);

                lastPadder.minWidth = lastSize;
                lastPadder.gameObject.SetActive(lastPadder.minWidth > 0);
            }
        }

        private void RefreshActive()
        {
            int startIndex;
            int endIndex;
            var velocity = Vector2.zero;

            if (loop && !ignoreLoopJump)
            {
                if (scrollPosition < loopFirstJumpTrigger)
                {
                    velocity = scrollRect.velocity;
                    ScrollPosition = loopLastScrollPosition - (loopFirstJumpTrigger - scrollPosition) + spacing;
                    scrollRect.velocity = velocity;
                }
                else if (scrollPosition > loopLastJumpTrigger)
                {
                    velocity = scrollRect.velocity;
                    ScrollPosition = loopFirstScrollPosition + (scrollPosition - loopLastJumpTrigger) - spacing;
                    scrollRect.velocity = velocity;
                }
            }

            CalculateCurrentActiveCellRange(out startIndex, out endIndex);

            if (startIndex == activeCellViewsStartIndex && endIndex == activeCellViewsEndIndex) return;

            ResetVisibleCellViews();
        }

        private void CalculateCurrentActiveCellRange(out int startIndex, out int endIndex)
        {
            startIndex = 0;
            endIndex = 0;

            var startPosition = scrollPosition - LookAheadBefore;
            var endPosition = scrollPosition + (scrollDirection == ScrollDirectionType.Vertical ? scrollRectTransform.rect.height : scrollRectTransform.rect.width) + LookAheadAfter;
            
            startIndex = GetCellViewIndexAtPosition(startPosition);
            endIndex = GetCellViewIndexAtPosition(endPosition);
        }

        private int GetCellIndexAtPosition(float position, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex)
            {
                return startIndex;
            }

            var middleIndex = (startIndex + endIndex) / 2;

            var pad = scrollDirection == ScrollDirectionType.Vertical ? padding.top : padding.left;

            if ((cellViewOffsetArray[middleIndex] + pad) >= (position + (pad == 0 ? 0 : 1.00001f)))
            {
                return GetCellIndexAtPosition(position, startIndex, middleIndex);
            }
            else
            {
                return GetCellIndexAtPosition(position, middleIndex + 1, endIndex);
            }
        }

        private void ScrollRectOnValueChanged(Vector2 val)
        {
            if (scrollDirection == ScrollDirectionType.Vertical)
            {
                scrollPosition = (1.0f - val.y) * ScrollSize;
            }
            else
            {
                scrollPosition = val.x * ScrollSize;
            }

            scrollPosition = Mathf.Clamp(scrollPosition, 0, ScrollSize);

            if (scrollerScrolled != null)
            {
                scrollerScrolled(this, val, scrollPosition);
            }

            if (snapping && snapJumping)
            {
                if (Mathf.Abs(LinearVelocity) <= snapVelocityThreshold && LinearVelocity != 0)
                {
                    var normalized = NormalizedScrollPosition;

                    if (loop || (!loop && normalized > 0 && normalized < 1.0f))
                    {
                        Snap();
                    }
                }
            }

            RefreshActive();
        }

        private void SnapJumpComplete()
        {
            snapJumping = false;
            scrollRect.inertia = snapInertia;
            
            EnhancedScrollerCellView cellView = null;

            for (int i = 0; i < activeCellViews.Count; i++)
            {
                if (activeCellViews[i].DataIndex == snapDataIndex)
                {
                    cellView = activeCellViews[i];
                    break;
                }
            }

            if (scrollerSnapped != null)
            {
                scrollerSnapped(this, snapCellViewIndex, snapDataIndex, cellView);
            }
        }

        private IEnumerator TweenPosition(TweenType tweenType, float time, float start, float end, Action tweenComplete, bool forceCaculateRange)
        {
            if (!(tweenType == TweenType.Immediate || time == 0))
            {
                scrollRect.velocity = Vector2.zero;
                IsTweening = true;
                if (scrollerTweeningChanged != null)
                {
                    scrollerTweeningChanged(this, true);
                }

                tweenTimeLeft = 0;
                var newPosition = 0.0f;

                while (tweenTimeLeft < time)
                {
                    switch (tweenType)
                    {
                        case TweenType.Linear:          newPosition = EnhancedScrollerEasing.Linear(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.Spring:          newPosition = EnhancedScrollerEasing.Spring(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInQuad:      newPosition = EnhancedScrollerEasing.EaseInQuad(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutQuad:     newPosition = EnhancedScrollerEasing.EaseOutQuad(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutQuad:   newPosition = EnhancedScrollerEasing.EaseInOutQuad(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInCubic:     newPosition = EnhancedScrollerEasing.EaseInCubic(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutCubic:    newPosition = EnhancedScrollerEasing.EaseOutCubic(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutCubic:  newPosition = EnhancedScrollerEasing.EaseInOutCubic(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInQuart:     newPosition = EnhancedScrollerEasing.EaseInQuart(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutQuart:    newPosition = EnhancedScrollerEasing.EaseOutQuart(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutQuart:  newPosition = EnhancedScrollerEasing.EaseInOutQuart(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInQuint:     newPosition = EnhancedScrollerEasing.EaseInQuint(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutQuint:    newPosition = EnhancedScrollerEasing.EaseOutQuint(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutQuint:  newPosition = EnhancedScrollerEasing.EaseInOutQuint(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInSine:      newPosition = EnhancedScrollerEasing.EaseInSine(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutSine:     newPosition = EnhancedScrollerEasing.EaseOutSine(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutSine:   newPosition = EnhancedScrollerEasing.EaseInOutSine(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInExpo:      newPosition = EnhancedScrollerEasing.EaseInExpo(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutExpo:     newPosition = EnhancedScrollerEasing.EaseOutExpo(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutExpo:   newPosition = EnhancedScrollerEasing.EaseInOutExpo(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInCirc:      newPosition = EnhancedScrollerEasing.EaseInCirc(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutCirc:     newPosition = EnhancedScrollerEasing.EaseOutCirc(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutCirc:   newPosition = EnhancedScrollerEasing.EaseInOutCirc(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInBounce:    newPosition = EnhancedScrollerEasing.EaseInBounce(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutBounce:   newPosition = EnhancedScrollerEasing.EaseOutBounce(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutBounce: newPosition = EnhancedScrollerEasing.EaseInOutBounce(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInBack:      newPosition = EnhancedScrollerEasing.EaseInBack(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutBack:   newPosition = EnhancedScrollerEasing.EaseInOutBack(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutBack:     newPosition = EnhancedScrollerEasing.EaseOutBack(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInElastic:   newPosition = EnhancedScrollerEasing.EaseInElastic(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseOutElastic:  newPosition = EnhancedScrollerEasing.EaseOutElastic(start, end, (tweenTimeLeft / time)); break;
                        case TweenType.EaseInOutElastic:newPosition = EnhancedScrollerEasing.EaseInOutElastic(start, end, (tweenTimeLeft / time)); break;
                    }

                    ScrollPosition = newPosition;

                    tweenTimeLeft += Time.unscaledDeltaTime;

                    yield return null;
                }
            }

            ScrollPosition = end;

            if (forceCaculateRange)
            {
                RefreshActive();
            }

            if (tweenComplete != null)
            {
                tweenComplete();
            }

            IsTweening = false;
            
            if (scrollerTweeningChanged != null)
            {
                scrollerTweeningChanged(this, false);
            }
        }
    }
}
