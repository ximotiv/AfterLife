using TMPro;
using UnityEngine;
using Zenject;

public class UnitsCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _spiritScore;
    [SerializeField] private TMP_Text _humanityScore;

    private UnitCounter _counter;

    private void Start()
    {
        UpdateScore();
    }

    private void OnEnable()
    {
        _counter.OnUpdatedScore += UpdateScore;
    }

    private void OnDisable()
    {
        _counter.OnUpdatedScore -= UpdateScore;
    }

    private void UpdateScore()
    {
        _spiritScore.text = $"{_counter.SpiritTeamScore}";
        _humanityScore.text = $"{_counter.HumanityTeamScore}";
    }

    [Inject]
    public void Construct(UnitCounter counter)
    {
        _counter = counter;
    }
}
