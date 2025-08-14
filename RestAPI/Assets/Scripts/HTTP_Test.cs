using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HTTP_Test : MonoBehaviour
{
    [SerializeField]
    private int characterId = 102;

    private string APIUrl = "https://my-json-server.typicode.com/manolovillarreal/jsonDB/users/";
    private string RicKandMortyUrl = "https://rickandmortyapi.com/api/character/";

    [SerializeField]
    private RawImage rawImage;

    void Start()
    {
        StartCoroutine(GetUser(1));
        StartCoroutine(GetCharacter(characterId));
    }

    IEnumerator GetUser(int userId)
    {
        UnityWebRequest request = UnityWebRequest.Get(APIUrl + userId);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                // Show results as text
                string json = request.downloadHandler.text;
                Debug.Log(json);

            }
            else
            {
                string mensaje = "status:" + request.responseCode;
                mensaje += "\nError: " + request.error;
                Debug.Log(mensaje);
            }

        }
    }
    IEnumerator GetCharacter(int characterId)
    {
        UnityWebRequest request = UnityWebRequest.Get(RicKandMortyUrl + characterId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                // Show results as text
                string json = request.downloadHandler.text;
                Character character = JsonUtility.FromJson<Character>(json);
                Debug.Log(character.id + ":" + character.name + " is an " + character.species);
                StartCoroutine(GetImage(character.image));
            }
            else
            {
                string mensaje = "status:" + request.responseCode;
                mensaje += "\nError: " + request.error;
                Debug.Log(mensaje);
            }

        }
    }

    IEnumerator GetImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                // Show results as texture
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Debug.Log("Image downloaded successfully");
                rawImage.texture = texture;
            }
            else
            {
                string mensaje = "status:" + request.responseCode;
                mensaje += "\nError: " + request.error;
                Debug.Log(mensaje);
            }
        }
    }
}

public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}
