using UnityEngine;

    public class StateManager : MonoBehaviour
    {
        public virtual void LevelChanged(Upgradeable upgradeable)
        {
        }

        public virtual void HpChanged(Destroyable destroyable)
        {
            
        }

        public void PathFinished(MoverPrefab moverPrefab)
        {
            Debug.Log("Path Finished");
        }
    }
