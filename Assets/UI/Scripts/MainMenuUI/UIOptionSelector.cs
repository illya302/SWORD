using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIOptionSelector : MonoBehaviour
{
    [SerializeField] private ScrollRect menuOptionScroller;
    [SerializeField] private RectTransform menuOptionScrollerContent;

    [SerializeField] private List<RectTransform> menuItems;
    [SerializeField] private RectTransform selectedMenuItem;
    [SerializeField] private float scrollSpeed;

    private void Awake()
    {
        InitializeScroller();
    }

    private void InitializeScroller()
    {
        menuItems = new List<RectTransform>();
        for (int i = 0; i < menuOptionScrollerContent.childCount; i++)
        {
            menuItems.Add(menuOptionScrollerContent.GetChild(i).GetComponent<RectTransform>());
        }
    }

    public void ScrollTo(int direction)
    {
        int index = menuItems.IndexOf(selectedMenuItem) + direction;
        if (index < 0)
            return;
        if (index > menuItems.Count - 1)
            return;

        selectedMenuItem = menuItems[index];

        

        
        StartCoroutine(ScrollCoroutine(direction));
    }

    IEnumerator ScrollCoroutine(int direction)
    {
        float distance = .5f;
        float passedDistance = 0;

        while (direction != 0)
        {
            float passedForFrame = scrollSpeed * Time.deltaTime;

            //Debug.Log("Direction -- " + "(" + direction + ")");
            if (direction > 0)
            {
                if (menuOptionScroller.horizontalNormalizedPosition < 1f)
                {
                    
                    passedDistance += passedForFrame;
                    menuOptionScroller.horizontalNormalizedPosition += passedForFrame;

                    if (passedDistance >= distance)
                    {
                        float roundedValue = (int)(menuOptionScroller.horizontalNormalizedPosition * 10) / 10f;
                        Debug.Log("roundedValue -- " + roundedValue);
                        menuOptionScroller.horizontalNormalizedPosition = roundedValue;
                        Debug.Log("passedDistance -- " + passedDistance);
                        direction = 0;
                    }

                }
                else
                    direction = 0;
            }
            else if (direction < 0)
            {
                if (menuOptionScroller.horizontalNormalizedPosition > 0f)
                {
                    passedDistance += passedForFrame;
                    menuOptionScroller.horizontalNormalizedPosition -= passedForFrame;

                    if (passedDistance >= distance)
                    {
                        float roundedValue = (int)((menuOptionScroller.horizontalNormalizedPosition + 0.09) * 10) / 10f;
                        Debug.Log("roundedValue -- " + roundedValue);
                        menuOptionScroller.horizontalNormalizedPosition = roundedValue;
                        Debug.Log("passedDistance -- " + passedDistance);
                        direction = 0;
                    }
                }
                else
                    direction = 0;
            }

            yield return null;
        }



    }
}
