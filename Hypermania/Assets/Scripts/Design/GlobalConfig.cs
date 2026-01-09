using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Design
{
    [CreateAssetMenu(menuName = "Hypermania/Global Config")]
    public class GlobalConfig : ScriptableObject
    {
        public float Gravity = -20;
        public float GroundY = -3f;
        public float WallsX = 4f;
        public int ClankTicks = 30;

        [SerializeField]
        private List<CharacterConfig> _configs;

        public CharacterConfig Get(Character character)
        {
            foreach (CharacterConfig config in _configs)
            {
                if (config.Character == character)
                {
                    return config;
                }
            }
            return null;
        }
    }
}
