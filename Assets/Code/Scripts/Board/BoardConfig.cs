using UnityEngine;

[CreateAssetMenu]
public class BoardConfig : ScriptableObject
{
    public int Width;
    public int Height;

    public Candy[] CandyPrefabs;
}
