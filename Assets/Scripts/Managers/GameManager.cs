using System.Collections;
using System.Collections.Generic;
using PER.Common;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private BoxCollider2D _leftBound;
    [SerializeField]
    private BoxCollider2D _rightBound;
    [SerializeField]
    private int _maxBandits;
    [SerializeField]
    private int _simultaneousBandits;
    private BanditsPool _banditsPool;
    [SerializeField]
    private Transform _leftSpawn;
    [SerializeField]
    private Transform _rightSpawn;

    public BoxCollider2D LeftBound => _leftBound;
    public BoxCollider2D RightBound => _rightBound;

    override protected void Awake()
    {
        _banditsPool = FindObjectOfType<BanditsPool>();
        base.Awake();
    }

    internal void Start()
    {
        for (int i = 0; i < 1; i++)
        {
            var bandit = _banditsPool.Get().BanditBrain;
            bandit.transform.position = _rightSpawn.position;
            Vector2 spot;
            bandit.RandomSpotRight(out spot);
            bandit.Run(spot);
        }
    }

    internal void Update()
    {

    }

    private Transform RandomSpawnPoint()
    {
        var sps = new[] { _leftSpawn, _rightSpawn };
        var index = Random.Range(0, 2);
        return sps[index];
    }
}
