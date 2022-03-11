﻿using System;
using System.Collections;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.Components;

[RequireComponent(typeof(RawImage))]
public class ActionButtonComponent : MonoBehaviour
{
    public CallType MoveType;
    public MahjongPlayerComponentBase MahjongPlayer;
    public MahjongTileComponent LastDiscardedTile;

    public Text ActionName;

    public Color SkipColor;
    public Color PonColor;
    public Color ChiColor;
    public Color KanColor;
    public Color RiichiColor;
    public Color TsumoColor;
    public Color RonColor;

    public float gizmoSize = .1f;

    public Vector3 ForcePositionOffset = Vector3.up * 1;

    private Func<IEnumerator> _buttonAction = () => throw new NotImplementedException("undefined action");
    public bool Submitted { get; private set; }

    public void Initialize(MahjongPlayerComponentBase player, CallType moveType, MahjongTileComponent lastDiscard)
    {
        MahjongPlayer = player;
        MoveType = moveType;
        LastDiscardedTile = lastDiscard;

        OnValidate();
    }

    public void Start()
    {
        var eventTrigger = gameObject.AddComponent<EventTrigger>();

        var enlarge = new EventTrigger.TriggerEvent();
        enlarge.AddListener(x => transform.localScale = Vector3.one * 1.2f);
        var pointerEnterEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter,
            callback = enlarge
        };

        var diminish = new EventTrigger.TriggerEvent();
        diminish.AddListener(x => transform.localScale = Vector3.one);
        var pointerExitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit,
            callback = diminish
        };

        var triggerAction = new EventTrigger.TriggerEvent();
        triggerAction.AddListener(x => StartCoroutine(TriggerActionOnPlayer()));
        var pointerClickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick,
            callback = triggerAction
        };

        eventTrigger.triggers.Add(pointerEnterEntry);
        eventTrigger.triggers.Add(pointerExitEntry);
        eventTrigger.triggers.Add(pointerClickEntry);
    }

    public void OnValidate()
    {
        InitializeByCallType();
    }

    public IEnumerator TriggerActionOnPlayer()
    {
        LastDiscardedTile = MahjongPlayer.HandParent.GetComponentsInChildren<MahjongTileComponent>()[0];
        yield return _buttonAction.Invoke();
        Submitted = true;
    }

    private void InitializeByCallType()
    {
        ActionName.text = MoveType.ToString();
        if (MoveType == CallType.OpenKan || MoveType == CallType.HiddenKan)
            ActionName.text = "Kan";

        var image = GetComponent<RawImage>();
        switch (MoveType)
        {
            case CallType.Skip:
                image.color = SkipColor;
                ActionName.color = SkipColor;
                _buttonAction = () => null;
                break;
            case CallType.Pon:
                image.color = PonColor;
                ActionName.color = PonColor;

                _buttonAction = () => MahjongPlayer.Pon(LastDiscardedTile,
                    LastDiscardedTile.transform.rotation * ForcePositionOffset);
                break;
            case CallType.Chi:
                image.color = ChiColor;
                ActionName.color = ChiColor;
                _buttonAction = () => MahjongPlayer.Chi();
                break;
            case CallType.OpenKan:
                image.color = KanColor;
                ActionName.color = KanColor;
                _buttonAction = () => MahjongPlayer.OpenKan();
                break;
            case CallType.HiddenKan:
                image.color = KanColor;
                ActionName.color = KanColor;
                _buttonAction = () => MahjongPlayer.HiddenKan();
                break;
            case CallType.Riichi:
                image.color = RiichiColor;
                ActionName.color = RiichiColor;
                _buttonAction = () => MahjongPlayer.Riichi();
                break;
            case CallType.Tsumo:
                image.color = TsumoColor;
                ActionName.color = TsumoColor;
                _buttonAction = () => MahjongPlayer.Tsumo();
                break;
            case CallType.Ron:
                image.color = RonColor;
                ActionName.color = RonColor;
                _buttonAction = () => MahjongPlayer.Ron();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}