﻿using UnityEngine;

/// <summary>
/// Power cards have:
/// Direction + Power
/// </summary>
public enum CardType
{
    RunLeft,
    RunRight,
    JumpLow,
    JumpHigh,
    Block
}

public class PlayingCardController : MonoBehaviour
{
    public CardType cardType;
    public int cardPower;

    #region Drag Properties

    private Vector3 mDragOffset;

    // If this is getting picked up, keep tabs on the starter slot in to clear it out
    private Transform mSourceTarget;

    private Transform mDropTarget;
    private Vector3 kDropOffset = new Vector3(0, 0, -1.0f);

    #endregion Drag Properties

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 50, Color.blue);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("{0} >>> Entering >>> {1}", gameObject.name, other.name));
        mDropTarget = other.transform;
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log(string.Format("{0} <<< Exiting <<< {1}", gameObject.name, other.name));
        if (mDropTarget == other.transform)
        {
            mDropTarget = null;
        }
    }

    public void OnBeginDrag()
    {
        mSourceTarget = mDropTarget;
        mDragOffset = Input.mousePosition - transform.position;
    }

    public void OnDrag()
    {
        transform.position = Input.mousePosition - mDragOffset;
    }

    public void OnEndDrag()
    {
        if (mDropTarget)
        {
            GoToDropTarget();
        }
    }

    private void GoToDropTarget()
    {
        // First clear the old one
        if (mSourceTarget != null)
        {
            CardSlotController oldCSC = mSourceTarget.GetComponent<CardSlotController>();
            if (oldCSC)
            {
                oldCSC.TakeCard();
            }
        }
        CardSlotController newCSC = mDropTarget.GetComponent<CardSlotController>();
        if (newCSC != null)
        {
            newCSC.PlaceCard(gameObject);
        }
        transform.position = mDropTarget.transform.position + kDropOffset;
    }

    private void OnDrawGizmos()
    {
        TextGizmo.Instance.DrawText(transform.position, string.Format("{0},{1}", cardType, cardPower));
    }
}