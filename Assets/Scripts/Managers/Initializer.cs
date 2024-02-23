using System.Linq;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [Tooltip("Asure these objects are initialized after Awake/OnEnable calls")]
    [SerializeReference] private Object[] orderedObjectsToInit;

    private void Start()
    {
        foreach (IOrderedInitialization orderedObject in orderedObjectsToInit.Cast<IOrderedInitialization>())
        {
            orderedObject.Initialize();
        }
    }
}
