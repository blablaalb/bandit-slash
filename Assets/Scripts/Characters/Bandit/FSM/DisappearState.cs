using PER.Common.FSM;
using UnityEngine;
using Characters.Bandit;

namespace Characters.Bandit.FSM
{
    [System.Serializable]
    public class DisappearState : IState
    {
        private BanditPoolMember _poolMember;

        public string StateName => "Disappear";

        public void Initialize(BanditPoolMember poolMember)
        {
            _poolMember = poolMember;
        }

        public void Enter()
        {
            var pool = GameObject.FindObjectOfType<BanditsPool>();
            pool.Add(_poolMember);
        }

        public void Exit()
        {
        }

        public void OnFixedUpdate()
        {
        }

        public void OnUpdate()
        {
        }
    }
}