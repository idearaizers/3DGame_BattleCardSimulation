using UnityEngine;
using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// NOTE: 現状ではセーフエリアは使用しないため一旦コメントアウト
    /// </summary>
    public class SafeArea : MonoBehaviour
    {
        // public enum SimDevice
        // {
        //     None = 0,
        //     iPhoneX
        // }

        // private const float minAspectRatio = 9.0f / 20.0f;
        // private const float maxAspectRatio = 3.0f / 4.0f;

        // private Dictionary<SimDevice, Rect> protraiteDeviceRects = new Dictionary<SimDevice, Rect>()
        // {
        //     { SimDevice.iPhoneX, new Rect(0.0f, 102.0f / 2436.0f, 1.0f, 2202.0f / 2436.0f)}
        // };

        // private Dictionary<SimDevice, Rect> landscapeSimDeviceRects = new Dictionary<SimDevice, Rect>()
        // {
        //     { SimDevice.iPhoneX, new Rect(132.0f / 2436.0f, 63.0f / 1125.0f, 2172.0f / 2436.0f, 1062.0f / 1125.0f)}
        // };

        // [SerializeField]
        // private RectTransform rectTransform;

        // [SerializeField]
        // private bool ignoreX = false;

        // [SerializeField]
        // private bool ignoreY = false;

        // private static SimDevice simDevice = SimDevice.None;

        // private Rect lastSafeArea = new Rect(0, 0, 0, 0);
        // private Vector2Int lastScreenSize = new Vector2Int(0, 0);
        // private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;

        // private void Reset()
        // {
        //     rectTransform = GetComponent<RectTransform>();
        // }

        // private void Awake()
        // {
        //     // NOTE: 一旦OFF
        //     // if (rectTransform == null)
        //     // {
        //     //     rectTransform = GetComponent<RectTransform>();
        //     // }
        //     // Refresh();
        // }

        // private void Update()
        // {
        //     // NOTE: 一旦OFF
        //     // if (Application.isEditor && Input.GetKeyDown(KeyCode.X))
        //     // {
        //     //     simDevice = (int)(simDevice + 1) == System.Enum.GetValues(typeof(SimDevice)).Length
        //     //         ? SimDevice.None
        //     //         : (SimDevice)((int)simDevice + 1);
        //     // }

        //     // Refresh();
        // }

        // private void Refresh()
        // {
        //     var safeArea = GetSafeArea();

        //     if (safeArea != lastSafeArea ||
        //         Screen.width != lastScreenSize.x || 
        //         Screen.height != lastScreenSize.y || 
        //         Screen.orientation != lastOrientation)
        //     {
        //         lastScreenSize.x = Screen.width;
        //         lastScreenSize.y = Screen.height;
        //         lastOrientation = Screen.orientation;

        //         ApplySafeArea(safeArea);
        //     }
        // }

        // private Rect GetSafeArea()
        // {
        //     var safeArea = Screen.safeArea;

        //     if (Application.isEditor && simDevice != SimDevice.None)
        //     {
        //         Rect nsa;

        //         if (Screen.height > Screen.width)
        //         {
        //             nsa = protraiteDeviceRects[simDevice];
        //         }
        //         else
        //         {
        //             nsa = landscapeSimDeviceRects[simDevice];
        //         }

        //         safeArea = new Rect
        //         (
        //             Screen.width * nsa.x,
        //             Screen.height * nsa.y,
        //             Screen.width * nsa.width,
        //             Screen.height * nsa.height
        //         );
        //     }

        //     return safeArea;
        // }

        // private void ApplySafeArea(Rect r)
        // {
        //     lastSafeArea = r;

        //     if (ignoreX)
        //     {
        //         r.x = 0;
        //         r.width = Screen.width;
        //     }

        //     if (ignoreY)
        //     {
        //         r.y = 0;
        //         r.height = Screen.height;
        //     }

        //     if (Screen.width > 0 && Screen.height > 0)
        //     {
        //         var anchorMin = r.position;
        //         var anchorMax = r.position + r.size;

        //         anchorMin.x /= Screen.width;
        //         anchorMin.y /= Screen.height;
        //         anchorMax.x /= Screen.width;
        //         anchorMax.y /= Screen.height;

        //         if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
        //         {
        //             ClampAnchorWithAspectRatio(ref anchorMin, ref anchorMax);

        //             rectTransform.anchorMin = anchorMin;
        //             rectTransform.anchorMax = anchorMax;
        //         }
        //     }
        // }

        // private void ClampAnchorWithAspectRatio(ref Vector2 anchorMin, ref Vector2 anchorMax)
        // {
        //     var rate = (float)Screen.width / (float)Screen.height;

        //     if (minAspectRatio > 0 && rate < minAspectRatio)
        //     {
        //         var h = Screen.width / minAspectRatio;
        //         var y = (Screen.height - h) * 0.5f;
        //         anchorMin.y = Mathf.Max(anchorMin.y, y / Screen.height);
        //         anchorMax.y = Mathf.Max(anchorMin.y, (y + h) / Screen.height);
        //     }
        //     else if (maxAspectRatio > 0 && rate > maxAspectRatio)
        //     {
        //         var w = Screen.height * maxAspectRatio;
        //         var x = (Screen.width - w) * 0.5f;

        //         anchorMin.x = Mathf.Max(anchorMin.x, x / Screen.width);
        //         anchorMax.x = Mathf.Max(anchorMin.x, (x + w) / Screen.width);
        //     }
        // }
    }
}
