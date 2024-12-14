using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseCube : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _cubeSprite;
    [SerializeField] protected TMP_Text _value;
    public GameObject OnTop;
    public GameObject OnBottom;
    private Dictionary<int, Color> _valueToColor = new Dictionary<int, Color>()
    {
        {2,Color.blue},
        {4,Color.green},
        {8,Color.cyan},
        {16,Color.red},
        {32,Color.yellow}
    };

    public virtual void Initialize(int value)
    {
        _cubeSprite.color = _valueToColor[value];
        _value.text = value.ToString();
        
    }
}
