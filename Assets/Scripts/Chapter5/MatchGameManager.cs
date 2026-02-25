using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MatchGameManager : MonoBehaviour
{
    [System.Serializable]
    public struct ParejaImágenes
    {
        public Sprite imagenArriba;
        public Sprite imagenAbajoPar;
    }

    [Header("Bancos de Datos")]
    public List<ParejaImágenes> bancoParejas;
    public List<Sprite> bancoIncorrectas;

    [Header("Referencias de Escena")]
    public CardItem[] cardsArriba;
    public CardItem[] cardsAbajo;

    [Header("Referencias UI")]
    public GameObject linePrefab;
    public Transform lineContainer;

    [Header("Audio")]
    public AudioClip correctClip;
    public AudioClip incorrectClip;

    [Header("Feedback Visual")]
    public Color normalColor = Color.white;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    private LineController currentLine;

    private int counter;

    private void Start()
    {
        counter = 0;
        ConfigurarNivel();
    }

    private void Update()
    {
        if (counter >= 3)
        {
            Debug.Log("Completado");
        }
    }

    public void ConfigurarNivel()
    {
        List<ParejaImágenes> seleccionadas = bancoParejas.OrderBy(x => Random.value).Take(3).ToList();

        for (int i = 0; i < cardsArriba.Length; i++)
        {
            cardsArriba[i].cardID = i;
            cardsArriba[i].isSource = true;
            cardsArriba[i].isMatched = false;
            cardsArriba[i].GetComponent<Image>().sprite = seleccionadas[i].imagenArriba;
        }

        var opcionesAbajo = new List<(Sprite img, int id)>();
        for (int i = 0; i < seleccionadas.Count; i++)
        {
            opcionesAbajo.Add((seleccionadas[i].imagenAbajoPar, i));
        }

        Sprite imgExtra = bancoIncorrectas[Random.Range(0, bancoIncorrectas.Count)];
        opcionesAbajo.Add((imgExtra, -1));

        opcionesAbajo = opcionesAbajo.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < cardsAbajo.Length; i++)
        {
            cardsAbajo[i].cardID = opcionesAbajo[i].id;
            cardsAbajo[i].isSource = false;
            cardsAbajo[i].isMatched = false;
            cardsAbajo[i].GetComponent<Image>().sprite = opcionesAbajo[i].img;
        }
    }

    public void StartDrawing(CardItem sourceCard)
    {
        GameObject lineObj = Instantiate(linePrefab, lineContainer);
        currentLine = lineObj.GetComponent<LineController>();
        currentLine.SetColor(normalColor);

        currentLine.DrawLine(sourceCard.GetAnchorPosition(), Input.mousePosition);
    }

    public void UpdateDrawing(Vector3 mousePos)
    {
        if (currentLine != null)
        {
            currentLine.DrawLine(currentLine.transform.position, mousePos);
        }
    }

    public void FinishDrawing(CardItem sourceCard, CardItem targetCard)
    {
        if (currentLine == null) return;

        bool matchSuccess = false;

        if (targetCard != null && !targetCard.isSource && !targetCard.isMatched)
        {
            if (sourceCard.cardID == targetCard.cardID)
            {
                matchSuccess = true;
            }
        }

        if (matchSuccess)
        {
            currentLine.DrawLine(sourceCard.GetAnchorPosition(), targetCard.GetAnchorPosition());
            currentLine.SetColor(correctColor);

            sourceCard.isMatched = true;
            targetCard.isMatched = true;

            counter += 1;

            PlayAudio(correctClip);

            currentLine = null;
        }
        else
        {
            Destroy(currentLine.gameObject);
            currentLine = null;

            if (targetCard != null) PlayAudio(incorrectClip);
        }
    }

    public void PlayVoice()
    {
        AudioController.Instance.ReplayVoice();
    }

    private void PlayAudio(AudioClip clip)
    {
        AudioController.Instance.PlaySfx(clip);
    }
}