using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int CurrentIndex { get; private set; }
    
    [SerializeField] private Transform _model;
    
    public void Move(List<BoardPosition> positions, Action onComplete)
    {
        StartCoroutine(MoveSequence(positions, onComplete));
    }

    private IEnumerator MoveSequence(List<BoardPosition> positions, Action onComplete)
    {
        const float moveDuration = .35f;
        var startY = _model.position.y;
        var endY = startY + .5f;
        foreach (var position in positions)
        {
            transform.DOMove(position.transform.position, moveDuration);
            _model.DOLocalMoveY(endY, moveDuration * .5f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(moveDuration);
        }
        
        var lastPosition = positions[^1];
        CurrentIndex = lastPosition.Index;
        
        onComplete?.Invoke();
    }
}
