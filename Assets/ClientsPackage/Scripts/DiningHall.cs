using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningHall : MonoBehaviour
{
    private List<Client> _clients;

    [SerializeField] private GameObject _clientPrefab;

    void Start()
    {
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

    void InstantiateClient(Vector2 position)
    {
        GameObject tmp = Instantiate(_clientPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        _clients.Add(tmp.GetComponent<Client>());
    }

    void DeleteClient(int idx)
    {
        Client tmp = _clients[idx];
        _clients.RemoveAt(idx);
        Destroy(tmp.transform.parent);
    }
}
