using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementRowHandler : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    public string Title
    {
        get
        {
            return _title.text;
        }
        set
        {
            _title.text = value;
        }
    }
    [SerializeField]
    private Text _description;
    public string Description
    {
        get
        {
            return _description.text;
        }
        set
        {
            _description.text = value;
        }
    }
    [SerializeField]
    private Text _score;
    public string Score
    {
        get
        {
            return _score.text;
        }
        set
        {
            _score.text = value;
        }
    }
    [SerializeField]
    private Text _steps;

    [SerializeField]
    private Image _scoreImage;

    [SerializeField]
    private Color _completeColor;
    [SerializeField]
    private Color _uncompleteColor;

    public (int current, int max) Steps
    {
        set
        {
            if (value.current >= 0 && value.current < value.max)
            {
                _steps.text = $"{value.current} / {value.max}";
                if (value.current == value.max)
                    _scoreImage.color = _completeColor;
                else
                    _scoreImage.color = _uncompleteColor;
            }
            
        }
    }

    public void SetData(Achievements.AchievementData data)
    {
        Title = data.Title;
        Description = data.Description;
        Score = data.Score.ToString();
        Steps = (0, data.Steps);
    }
}
