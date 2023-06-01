using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiningHall : MonoBehaviour
{
    private List<Client> _clients;

    [SerializeField] private GameObject _clientPrefab;
    [SerializeField] private Match3 _match3;
    [SerializeField] private float _roundTime; //TODO: somewhere else (?)/clock script
    [SerializeField] private RectTransform _clockArrow; //TODO: clock script
    [SerializeField] private int _baseClientPoints = 2;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private GameObject _losingScreen;
    [SerializeField] private GameObject _winningScreen;

    private float points = 0;

    void Start()
    {
        _match3.OnMove += ItemChanged;

        _clients = new List<Client>();

        //TODO: magic numbers
        transform.GetChild(0).transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height * 0.8f, 0.0f));
        transform.GetChild(1).transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.8f, 0.0f));
        transform.GetChild(2).transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.8f, Screen.height * 0.8f, 0.0f));

        for(int i = 0; i < 3; i++)
        {
            InstantiateClient(new Vector2(transform.GetChild(i).transform.position.x, transform.GetChild(i).transform.position.y));
        }

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

    private void InstantiateClient(Vector2 position)
    {
        GameObject tmp = Instantiate(_clientPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        tmp.GetComponent<Client>()._match3 = _match3; //TODO: setters/auto-fetch
        tmp.GetComponent<Client>()._seatNr = _clients.Count;
        tmp.GetComponent<Client>().diningHall = this;
        _clients.Add(tmp.GetComponent<Client>());
    }

    public void DeleteClient(Client client)
    {
        _clients.Remove(client);
        Vector2 freedPosition = new Vector2(client.transform.position.x, client.transform.position.y);
        
        Destroy(client.transform.gameObject);

        InstantiateClient(freedPosition);
    }

    public void ItemChanged(object sender, System.EventArgs e) //TODO: change name
    {
        foreach(Client client in _clients){
            if(client.desiredDish == _match3.GetLastChosen())
            {
                points += client.state * _baseClientPoints;
                DeleteClient(client);
                return;
            }
        }
    }
}
