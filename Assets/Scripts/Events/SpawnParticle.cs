using UnityEngine;

namespace Events
{
    public struct SpawnParticle
    {
        public Vector3 Position;
        public ParticleType Type;
    }

    public enum ParticleType
    {
        FeatherExplosion,
        CharacterDeath,
        FightAction,
        Upgrade,
        UpgradeFailed,
    }
}
