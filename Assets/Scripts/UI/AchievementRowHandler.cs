using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementRowHandler : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    public Sprite Icon
    {
        set
        {
            _icon.sprite = value;
        }
    }
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

    public void SetData(Achievements.AchievementData data)
    {
        Icon = data.Icon;
        Title = data.Title;
        Description = data.Description;
        Score = data.Score.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
