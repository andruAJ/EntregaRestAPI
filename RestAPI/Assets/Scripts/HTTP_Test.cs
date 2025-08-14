using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HTTP_Test : MonoBehaviour
{
    [SerializeField]
    private int character1Id = 102;
    private int character2Id = 34;
    private int character3Id = 56;
    private int character4Id = 79;
    private int character5Id = 203;

    private int[] characterIds = new int[5] ;
    private string APIUrl = "https://my-json-server.typicode.com/andruAJ/EntregaRestAPI";
    private string RicKandMortyUrl = "https://rickandmortyapi.com/api/character/";

    private UIDocument uiDocument;

    private VisualElement imageChar1;
    private string nameChar1;
    private string speciesChar1;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        StartCoroutine(GetUser(1));
        StartCoroutine(GetCharacter(characterIds));
        imageChar1 = uiDocument.rootVisualElement.Q("CharacterPic") as VisualElement;
        characterIds[character1Id, character2Id, character3Id, character4Id, character5Id];

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
    IEnumerator GetCharacter(int[] characterIds)
    {
        foreach (int id in characterIds)
        {
            UnityWebRequest request = UnityWebRequest.Get(RicKandMortyUrl + id);
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
                imageChar1.style.backgroundImage = texture;
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
