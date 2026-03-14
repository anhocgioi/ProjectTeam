using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CharacterSelect/Character")]
public class CharacterData : ScriptableObject
{
    public string charName;
    public Sprite fullBodySprite;
    public Sprite icon;

    // Đổi tên biến này để khớp với LevelManager
    public GameObject characterPrefab;
}