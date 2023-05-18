using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningHall : MonoBehaviour
{
    private List<Client> _clients;

    [SerializeField] private GameObject _clientPrefab;
    [SerializeField] private Match3 _match3;

    void Start()
    {
        _match3.OnNewItemChanged += ItemChanged;

        _clients = new List<Client>();

        //TODO: magic numbers
        transform.GetChild(0).transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height * 0.8f, 0.0f));
        transform.GetChild(1).transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.8f, 0.0f));
        transform.GetChild(2).transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.8f, Screen.height * 0.8f, 0.0f));

        for(int i = 0; i < 3; i++)
        {
            InstantiateClient(new Vector2(transform.GetChild(i).transform.position.x, transform.GetChild(i).transform.position.y));
        }
    }

    private void InstantiateClient(Vector2 position)
    {
        GameObject tmp = Instantiate(_clientPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
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

    public void ItemChanged(object sender, System.EventArgs e)
    {
        foreach(Client client in _clients){
            if(client.desiredDish == _match3.GetLastChosen())
            {
                DeleteClient(client);
                return;
            }
        }
    }
}
