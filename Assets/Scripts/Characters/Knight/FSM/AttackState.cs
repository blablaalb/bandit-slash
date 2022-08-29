using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AttackState : IState
{
    private KnightAnimations _animations;
    private float _distance;
    private Transform _transform;
    private float _damage;
    private float _comboWaitTime;
    private Coroutine _comboWaitCoroutine;
    private KnightBrain _context;
    private bool _waiting;
    private int _maxCombos;
    private int _combo;
    [SerializeField]
    private float _yDistanceThreshold;

    public string StateName => "Attack";
    public bool WaitingForCombo => _waiting;
    public int Combo => _combo;

    public void Initialize(float distance, KnightAnimations animations, Transform transform, float damage, float comboWaitTime, KnightBrain context, int maxCombos)
    {
        _distance = distance;
        _animations = animations;
        _transform = transform;
        _damage = damage;
        _comboWaitTime = comboWaitTime;
        _context = context;
        _animations.CheckHit.AddListener(CheckHit);
        _animations.AttackFinished.AddListener(OnAttackAnimationFinished);
        _maxCombos = maxCombos;
    }

    public void Enter()
    {
        KillComboCoroutine();
        _combo++;
        _animations.Attack();
    }

    public void Exit()
    {
        KillComboCoroutine();
        _combo = 0;
        _waiting = false;
        _comboWaitCoroutine = null;
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }

    private void CheckHit()
    {
        var bandits = GameObject.FindObjectsOfType<BanditController>();
        var inRange = bandits.Where(b => Mathf.Abs(b.transform.position.y - _transform.position.y) <= _yDistanceThreshold).Where(b => Vector3.Distance(b.transform.position, _transform.position) <= _distance).Where(b =>
        {
            var x = b.transform.position.x > _transform.position.x ? 1 : -1;
            return _transform.GetChild(0).localScale.x - x == 0;
        });
        foreach (var bandit in inRange)
        {
            bandit.TakeHit(_damage);
        }
    }

    private void OnAttackAnimationFinished()
    {
        if (_comboWaitCoroutine == null && _combo < _maxCombos)
            _comboWaitCoroutine = _animations.StartCoroutine(ComboWaitCoroutine());
        else if (_combo == _maxCombos) _context.Idle();
    }

    private IEnumerator ComboWaitCoroutine()
    {
        _waiting = true;
        var wait = new WaitForSeconds(_comboWaitTime);
        yield return wait;
        _waiting = false;
        _comboWaitCoroutine = null;
        if (_context.State == this)
            _context.Idle();
    }

    private void KillComboCoroutine()
    {
        if (_comboWaitCoroutine != null)
            _animations.StopCoroutine(_comboWaitCoroutine);
    }
}
