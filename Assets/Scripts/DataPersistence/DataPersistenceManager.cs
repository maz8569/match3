using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string filename;

    private LevelData levelData;
    private List<IDataPesristence> dataPesristenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, filename);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void Restart()
    {
        InitializeLevel();
        dataHandler.Save(levelData);
        PushLoaded();
    }

    public void InitializeLevel()
    {
        levelData = new LevelData();
    }

    public void SaveLevel()
    {
        foreach(IDataPesristence dataPesristenceObj in dataPesristenceObjects)
        {
            dataPesristenceObj.SaveData(ref levelData);
        }

        dataHandler.Save(levelData);
    }

    public void LoadLevel()
    {
        levelData = dataHandler.Load();

        if(levelData == null)
        {
            Debug.Log("No data was found. Creating new one.");
            InitializeLevel();
        }

        PushLoaded();
    }

    public void FindObjectsToLoad()
    {
        dataPesristenceObjects = FindAllDataPersistenceObjects();

    }

    public void PushLoaded()
    {
        foreach (IDataPesristence dataPesristenceObj in dataPesristenceObjects)
        {
            dataPesristenceObj.LoadData(levelData);
        }
    }

    private List<IDataPesristence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPesristence> dataPesristenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPesristence>();

        return new List<IDataPesristence>(dataPesristenceObjects);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindObjectsToLoad();
        LoadLevel();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveLevel();
    }

}
