using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Bandit.FSM;
using PER.Common;
using UnityEngine;
using System.Linq;
using Characters.Bandit;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private BoxCollider2D _leftBound;
    [SerializeField]
    private BoxCollider2D _rightBound;
    [SerializeField]
    private int _simultaneousBandits;
    private BanditsPool _banditsPool;
    [SerializeField]
    private Transform _leftSpawn;
    [SerializeField]
    private Transform _rightSpawn;

    public BoxCollider2D LeftBound => _leftBound;
    public BoxCollider2D RightBound => _rightBound;
    public float SceneSizeX => Mathf.Abs((RightBound.bounds.center.x - RightBound.bounds.size.x * 0.5f) - (LeftBound.bounds.center.x + LeftBound.bounds.size.x * 0.5f));

    override protected void Awake()
    {
        _banditsPool = FindObjectOfType<BanditsPool>();
        base.Awake();
    }

    internal void Start()
    {
        StartCoroutine(SpawnBandits());
        Debug.Log(SceneSizeX);
    }

    public IEnumerator SpawnBandits()
    {
        var bandits = GameObject.FindObjectsOfType<BanditBrain>().Where(x => x.Alive).ToArray().Length;
        var spawningBandits = _simultaneousBandits - bandits;
        for (int i = 0; i < spawningBandits; i++)
        {
            yield return new WaitForSeconds(1f);
            SpawnRandomBandit();
        }
    }

    private BanditBrain SpawnRandomBandit()
    {
        var variants = new Func<BanditBrain>[] { SpawnBanditLeft, SpawnBanditRight };
        var indx = UnityEngine.Random.Range(0, 2);
        var bandit = variants[indx]?.Invoke();
        return bandit;
    }

    private BanditBrain SpawnBanditRight()
    {
        var bandit = _banditsPool.Get().BanditBrain;
        bandit.transform.position = _rightSpawn.position;
        Vector2 spot;
        if (bandit.RandomSpotRight(out spot) || bandit.RandomSpotLeft(out spot))
            bandit.Run(spot);
        return bandit;
    }

    private BanditBrain SpawnBanditLeft()
    {
        var bandit = _banditsPool.Get().BanditBrain;
        bandit.transform.position = _leftSpawn.position;
        Vector2 spot;
        if (bandit.RandomSpotLeft(out spot) || bandit.RandomSpotRight(out spot))
        {
            bandit.Run(spot);
            return bandit;
        }
        FindObjectOfType<BanditsPool>().Add(bandit.GetComponent<BanditPoolMember>());
        return null;
    }

    public void OnKnightDied()
    {
    }

    public void OnBanditDied()
    {
        SpawnRandomBandit();
    }
}
