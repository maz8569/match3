using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiningHall : MonoBehaviour
{
    private List<Client> _clients;

    [SerializeField] private float _waitTimeBetweenClients;
    [SerializeField] private GameObject _clientPrefab;
    [SerializeField] private Match3 _match3;
    [SerializeField] private float _roundTime; //TODO: somewhere else (?)/clock script
    [SerializeField] private RectTransform _clockArrow; //TODO: clock script
    [SerializeField] private Image _passedTime; //TODO: clock script
    [SerializeField] private int _baseClientPoints = 2;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private SummaryScreen _summaryScreen;
    [SerializeField] private GameObject _losingScreen; //TODO: move to SummaryScreen script
    [SerializeField] private GameObject _winningScreen; //TODO: move to SummaryScreen script
    [SerializeField] private List<GameObject> _seats; //TODO: Client struct (?)
    [SerializeField] private List<GameObject> _plates; //TODO: Client struct (?)
    [SerializeField] private List<GameObject> _clouds; //TODO: Client struct (?)
    [SerializeField] private List<GameObject> _dishes; //TODO: Client struct (?)
    [SerializeField] private List<Image> _stars; //TODO: move to SummaryScreen script
    [SerializeField] private Sprite _filledStar; //TODO: move to SummaryScreen script
    [SerializeField] private Sprite _unfilledStar; //TODO: move to SummaryScreen script

    private float points = 0;
    public Dictionary<RecipeSO, Dictionary<ItemSO, int>> _recipesSummary; //TODO: make private with getter
    public Dictionary<RecipeSO, int> _dishesSummary;

    void Start()
    {
        _recipesSummary = new Dictionary<RecipeSO, Dictionary<ItemSO, int>>();
        _dishesSummary = new Dictionary<RecipeSO, int>();
        _match3.OnMove += ItemChanged;
        _match3.OnMove += CheckBoard;

        _clients = new List<Client>();

        //TODO: magic numbers
        _seats[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height * 0.68f, 0.0f));
        _seats[1].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.68f, 0.0f));
        _seats[2].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.8f, Screen.height * 0.68f, 0.0f));

        StartCoroutine(InitializeClients());

        StartCoroutine(StartCountdown());
    }
    
    private IEnumerator StartCountdown()
    {
        float _waitingTime = _roundTime;
        while (_waitingTime >= 0)
        {            
            yield return new WaitForSeconds(0.1f);//TODO: magic numbers
            _waitingTime -= 0.1f; //TODO: magic numbers
            float state = _waitingTime / _roundTime;
            _clockArrow.rotation = Quaternion.Euler(0.0f, 0.0f, state * 360.0f);
            _passedTime.fillAmount = 1 - state;
        }

        _match3.score += (int)points;

        _endScreen.SetActiveRecursively(true);

        if(_match3.checkedStars == 0)
        {
            _winningScreen.SetActive(false);
        }
        else
        {
            _losingScreen.SetActive(false);

            for (int i = 0; i < _match3.checkedStars; i++)
            {
                _stars[i].sprite = _filledStar;
            }

        }

        _summaryScreen.Activate();
        Debug.Log(points);
    }

    private IEnumerator InitializeClients()
    {
        yield return new WaitForSeconds(0.1f); //TODO: magic number (fix to appear after closing initial message?)

        StartCoroutine(InstantiateClient(0));

        yield return new WaitForSeconds(_waitTimeBetweenClients);

        StartCoroutine(InstantiateClient(1));

        yield return new WaitForSeconds(_waitTimeBetweenClients);

        StartCoroutine(InstantiateClient(2));
    }

    private IEnumerator InstantiateClient(int seat)
    {
        GameObject tmp = Instantiate(_clientPrefab, new Vector3(_seats[seat].transform.position.x, _seats[seat].transform.position.y, 0), Quaternion.identity);

        Client tmpClient = tmp.GetComponent<Client>();
        tmpClient._match3 = _match3; //TODO: setters/auto-fetch
        tmpClient._seatNr = seat;
        tmpClient.diningHall = this;

        tmpClient.plate = _plates[seat];
        tmpClient.cloud = _clouds[seat];
        tmpClient.dish = _dishes[seat];
        tmpClient.client = _match3.levelSO.clients[Random.Range(0, _match3.levelSO.clients.Count)];

        _plates[seat].SetActive(true);
        _clouds[seat].SetActiveRecursively(true);

        _clients.Add(tmp.GetComponent<Client>());
        yield return null;
    }

    public IEnumerator DeleteClient(Client client)
    {
        int freedSeat = client._seatNr;

        if(client.IsServed())
        {        
        _plates[freedSeat].SetActive(false);
        _dishes[freedSeat].GetComponent<Image>().sprite = client.desiredDish.ServedSprite;
        _dishes[freedSeat].SetActive(true);
        }
        
        yield return new WaitForSeconds(_waitTimeBetweenClients);

        _clients.Remove(client);
        
        _plates[freedSeat].SetActive(false);
        _clouds[freedSeat].SetActive(false);
        _dishes[freedSeat].SetActive(false);

        Destroy(client.transform.gameObject); //TODO: fix error

        yield return new WaitForSeconds(_waitTimeBetweenClients);
        StartCoroutine(InstantiateClient(freedSeat));
    }

    public void ItemChanged(object sender, System.EventArgs e) //TODO: change name
    {
        foreach(Client client in _clients){
            if(client.desiredDish == _match3.GetLastChosen() && !client.IsServed())
            {
                Debug.Log(_match3.GetLastChosen());
                //Make client euphoric
                client.SetServed(true);

                //Save used ingredients for summary screen
                if(!_recipesSummary.ContainsKey(client.desiredDish))
                {
                    _recipesSummary.Add(client.desiredDish, new Dictionary<ItemSO, int>());
                    _recipesSummary[client.desiredDish].Add(client.desiredDish.Ingredient1, _match3.GetSelectedItems()[client.desiredDish.Ingredient1]);
                    _recipesSummary[client.desiredDish].Add(client.desiredDish.Ingredient2, _match3.GetSelectedItems()[client.desiredDish.Ingredient2]);
                    if (client.desiredDish.Ingredient3 != null)
                    {
                        _recipesSummary[client.desiredDish].Add(client.desiredDish.Ingredient3, _match3.GetSelectedItems()[client.desiredDish.Ingredient3]);
                    }
                    _dishesSummary.Add(client.desiredDish, 1);
                }
                else
                {
                    _recipesSummary[client.desiredDish][client.desiredDish.Ingredient1] += _match3.GetSelectedItems()[client.desiredDish.Ingredient1]; //TODO: fix this absolute garbage
                    
                    _recipesSummary[client.desiredDish][client.desiredDish.Ingredient2] += _match3.GetSelectedItems()[client.desiredDish.Ingredient2];

                    if (client.desiredDish.Ingredient3 != null)
                    {
                        _recipesSummary[client.desiredDish][client.desiredDish.Ingredient3] += _match3.GetSelectedItems()[client.desiredDish.Ingredient3];
                    }

                    _dishesSummary[client.desiredDish] += 1;
                }
                
                //Add points and delete client
                points += client.state * _baseClientPoints;
                StartCoroutine(DeleteClient(client));
                return;
            }
        }
    }

    public void CheckBoard(object sender, System.EventArgs e)
    {
        foreach(Client client in _clients)
        {
            _match3.CheckBoardForItem(client.desiredDish);
        }
    }

    public List<RecipeSO> GetWantedDishes()
    {
        List<RecipeSO> ret = new List<RecipeSO>();

        foreach(Client client in _clients)
        {
            ret.Add(client.desiredDish);
        }

        return ret;
    }
}
