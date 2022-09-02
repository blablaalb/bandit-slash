using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Bandit.FSM;

namespace Characters.Bandit
{
    public class BanditPoolMember : PoolMember<BanditsPool>
    {
        public BanditBrain BanditBrain { get; private set; }

        override protected void Awake()
        {
            BanditBrain = GetComponent<BanditBrain>();
            base.Awake();
        }
    }
}
