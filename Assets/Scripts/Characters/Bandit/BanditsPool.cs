using System.Collections;
using System.Collections.Generic;
using Characters.Bandit;
using UnityEngine;

public class BanditsPool : GenericPool<BanditPoolMember>
{
    private int _banditPower;

    override protected void Awake()
    {
        _banditPower = 1;
        base.Awake();
    }

    internal void Start()
    {
        StartCoroutine(BanditPowerCountdown());
    }

    private IEnumerator BanditPowerCountdown()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(10);
            _banditPower += 1;
        }
    }

    public override BanditPoolMember Get()
    {
        var bandit = base.Get();
        bandit.BanditBrain.Health = bandit.BanditBrain.MaxHelth;
        bandit.GetComponent<Rigidbody2D>().simulated = true;
        bandit.GetComponent<Collider2D>().isTrigger = false;
        bandit.GetComponent<Collider2D>().enabled = true;
        bandit.BanditBrain.Power = _banditPower;
        bandit.gameObject.SetActive(true);
        bandit.BanditBrain.Idle();
        return bandit;
    }
}
