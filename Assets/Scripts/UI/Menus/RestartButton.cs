using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private DataPersistenceManager _persistenceManager;
    // Start is called before the first frame update
    void Start()
    {
        _persistenceManager = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        GetComponent<Button>().onClick.AddListener(() => _persistenceManager.Restart());
    }
}
