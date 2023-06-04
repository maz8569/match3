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
    [SerializeField] private int _baseClientPoints = 2;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private GameObject _losingScreen;
    [SerializeField] private GameObject _winningScreen;
    [SerializeField] private List<GameObject> _seats; //TODO: Client struct (?)
    [SerializeField] private List<GameObject> _plates; //TODO: Client struct (?)
    [SerializeField] private List<GameObject> _clouds; //TODO: Client struct (?)

    private float points = 0;

    void Start()
    {
        _match3.OnMove += ItemChanged;

        _clients = new List<Client>();

        //TODO: magic numbers
        _seats[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height * 0.7f, 0.0f));
        _seats[1].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.7f, 0.0f));
        _seats[2].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.8f, Screen.height * 0.7f, 0.0f));

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
        }
        Debug.Log(points);
    }

    private IEnumerator InitializeClients()
    {
        StartCoroutine(InstantiateClient(0));

        yield return new WaitForSeconds(_waitTimeBetweenClients);

        StartCoroutine(InstantiateClient(1));

        yield return new WaitForSeconds(_waitTimeBetweenClients);

        StartCoroutine(InstantiateClient(2));
    }

    private IEnumerator InstantiateClient(int seat)
    {
        yield return new WaitForSeconds(_waitTimeBetweenClients);
        GameObject tmp = Instantiate(_clientPrefab, new Vector3(_seats[seat].transform.position.x, _seats[seat].transform.position.y, 0), Quaternion.identity);

        Client tmpClient = tmp.GetComponent<Client>();
        tmpClient._match3 = _match3; //TODO: setters/auto-fetch
        tmpClient._seatNr = seat;
        tmpClient.diningHall = this;

        tmpClient.plate = _plates[seat];
        tmpClient.cloud = _clouds[seat];

        _plates[seat].SetActive(true);

        _clients.Add(tmp.GetComponent<Client>());
    }

    public IEnumerator DeleteClient(Client client)
    {
        int freedSeat = client._seatNr;

        _clouds[freedSeat].transform.GetChild(0).GetComponent<Image>().sprite = client.desiredDish.Sprite;
        _clouds[freedSeat].SetActive(true);

        yield return new WaitForSeconds(_waitTimeBetweenClients);

        _clients.Remove(client);
        
        _plates[freedSeat].SetActive(false);
        _clouds[freedSeat].SetActive(false);

        Destroy(client.transform.gameObject);

        StartCoroutine(InstantiateClient(freedSeat));
    }

    public void ItemChanged(object sender, System.EventArgs e) //TODO: change name
    {
        foreach(Client client in _clients){
            if(client.desiredDish == _match3.GetLastChosen())
            {
                points += client.state * _baseClientPoints;
                StartCoroutine(DeleteClient(client));
                return;
            }
        }
    }
}
