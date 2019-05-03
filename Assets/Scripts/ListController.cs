using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

//TODO: Add Name List dictionary
public class ListController : MonoBehaviour
{
    public Transform targetTransform;
    //-----Pooling variables
    public int activeNumber;
    public List<ListItemDisplay> activeList;// List of Active objects should be equal to init number alway 
    public List<ListItemDisplay> pooledListItems;
    public ListItemDisplay displayPrefab;
    
    private int _poolSize;
    private float _prevValue;
    private Vector3 _firstItemPosition;
    private float _itemGap = 62.5F;
    private List<string> names = new List<string>{"Aprilia", "BMW", "Buell", "Ducati", "Harley Davidson", "Honda", "Husqvarna", "Indian", "Kawasaki", "KTM", "Laverda", "Moto Guzzi", "MV Agusta", "Norton", "Piaggio", "Royal Enfield", "Suzuki", "Triumph", "Ural", "Yamaha"};
        
    
    void Start(){
        //Init Pool
        _poolSize = names.Count;
        for (int i = 0; i < _poolSize; i++){
            ListItemDisplay newDisplay = (ListItemDisplay) Instantiate(displayPrefab, targetTransform, false);
            newDisplay.id = i;
            newDisplay.displayTextName.text = names[i];
            newDisplay.gameObject.SetActive(false);
            pooledListItems.Add(newDisplay);
        }
        //Init Active List
        for (int i = 0; i < activeNumber; i++){
            pooledListItems[i].gameObject.SetActive(true);
            activeList.Add(pooledListItems[i]);
        }
        _firstItemPosition = new Vector3(activeList[0].transform.position.x, activeList[0].transform.position.y, activeList[0].transform.position.z);
    }

    void Update()
    {
        Vector3 currentSecondItemPosition = activeList[1].transform.position;
        if (currentSecondItemPosition.y - _firstItemPosition.y >= _itemGap){
            ScrollDownUpdate();
        }

        Vector3 currentFirstItemPosition = activeList[0].transform.position;
        
        if (currentFirstItemPosition.y - _firstItemPosition.y == -50 && activeList[0].gameObject.GetComponent<ListItemDisplay>().id != 0){
            ScrollUpUpdate();
        }
    }

    void ScrollDownUpdate(){
        Debug.Log("Recycle First Item and Add new item to list");
        int nextItemId = activeList[activeNumber - 1].gameObject.GetComponent<ListItemDisplay>().id + 1;
        if (nextItemId < _poolSize){
            pooledListItems[nextItemId].gameObject.SetActive(true);
            activeList.Add(pooledListItems[nextItemId]);
            activeList[0].gameObject.SetActive(false);
            activeList.RemoveAt(0);
            GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 2*_itemGap - 10);
        }
    }

    void ScrollUpUpdate(){
        Debug.Log("Recycle Last item and Add new item to list");
        int nextItemId = activeList[0].gameObject.GetComponent<ListItemDisplay>().id - 1;
        if (nextItemId >= 0){
            activeList[activeNumber-1].gameObject.SetActive(false);
            activeList.RemoveAt(activeNumber-1);
            pooledListItems[nextItemId].gameObject.SetActive(true);
            activeList.Insert(0, pooledListItems[nextItemId]);
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 2*_itemGap - 10);
        }
    }
}
