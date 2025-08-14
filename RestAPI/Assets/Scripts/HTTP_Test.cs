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

    private VisualElement[] imageChars;
    private Label[] nameLabels;
    private Label[] speciesLabels;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        characterIds = new int[] { character1Id, character2Id, character3Id, character4Id, character5Id };

        imageChars = new VisualElement[5];
        nameLabels = new Label[5];
        speciesLabels = new Label[5];

        for (int i = 0; i < 5; i++)
        {
            imageChars[i] = uiDocument.rootVisualElement.Q($"CharacterPic{i + 1}") as VisualElement;
            nameLabels[i] = uiDocument.rootVisualElement.Q($"CharacterName{i + 1}") as Label;
            speciesLabels[i] = uiDocument.rootVisualElement.Q($"CharacterSpecies{i + 1}") as Label;
        }

        for (int i = 0; i < characterIds.Length; i++)
        {
            StartCoroutine(GetCharacter(characterIds[i], i));
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
        IEnumerator GetCharacter(int id, int index)
        {
            UnityWebRequest request = UnityWebRequest.Get(RicKandMortyUrl + id);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);
                nameLabels[index].text = character.name;
                speciesLabels[index].text = character.species;
                StartCoroutine(GetImage(character.image, index));
            }
        }

        IEnumerator GetImage(string imageUrl, int index)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                imageChars[index].style.backgroundImage = new StyleBackground(texture);
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
