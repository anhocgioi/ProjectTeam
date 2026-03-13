using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CharacterSelect/Character")]
public class CharacterData : ScriptableObject
{
    public string charName;     // Tên nhân vật
    public Sprite fullBodySprite; // Ảnh lớn hiện ở Preview
    public Sprite icon;         // Ảnh nhỏ hiện ở ô Grid
    public GameObject prefab;   // Prefab để sinh ra khi vào trận
}