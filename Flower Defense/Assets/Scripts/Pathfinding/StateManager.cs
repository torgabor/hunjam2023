using UnityEngine;

    public class StateManager : MonoBehaviour
    {
        public virtual void LevelChanged(Upgradeable upgradeable)
        {
        }

        public virtual void HpChanged(Destroyable destroyable)
        {
            
        }
    }
