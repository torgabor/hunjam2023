using UnityEngine;

public class PathManager : MonoBehaviour
{
    public virtual void PathFinished(MoverPrefab moverPrefab)
    {
        Debug.Log("Path Finished");
    }
}
    public class StateManager : MonoBehaviour
    {
        public virtual void LevelChanged(Upgradeable upgradeable)
        {
        }

        public virtual void HpChanged(Destroyable destroyable)
        {
            
        }
        
    }
