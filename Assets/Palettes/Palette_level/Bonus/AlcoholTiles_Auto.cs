using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AlcoholTiles_Auto : Tile
{

    [SerializeField]
    private Sprite[] alcoholSprites;

    [SerializeField]
    private Sprite preview;

    [SerializeField]
    private Sprite errorSprite;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/Tiles/AlcoholTile")]
    public static void CreateAlcoholTile()
    {
        string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Alcohol Tile", "New Alcohol Tile", "asset", "Save Alcohol Tile", "Assets");
        if (path == "")
            return;
        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AlcoholTiles_Auto>(), path);
    }
#endif

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int nPos = new Vector3Int(position.x + i, position.y + j, position.z);
                if (isAlcohol(tilemap, nPos))
                    tilemap.RefreshTile(nPos);
            }
        }
    }

    private bool isAlcohol(ITilemap map, Vector3Int pos)
    {
        return map.GetTile(pos) == this;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        string score = "";
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int nPos = new Vector3Int(position.x + i, position.y + j, position.z);
                if (nPos != position)
                {
                    if (isAlcohol(tilemap, nPos))
                        score += "B";
                    else
                        score += "E";
                }
            }
        }

        tileData.sprite = getTile(score);
        tileData.colliderType = ColliderType.Sprite;
    }

    private Sprite getTile(string score)
    {
        if (score[0] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return alcoholSprites[16];
        if (score[5] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return alcoholSprites[15];
        if (score[7] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return alcoholSprites[10];
        if (score[2] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return alcoholSprites[11];

        if (score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return alcoholSprites[3];
        if (score[3] == 'B' && score[4] == 'E' && score[1] == 'B' && score[6] == 'B')
            return alcoholSprites[6];
        if (score[3] == 'E' && score[4] == 'B' && score[1] == 'B' && score[6] == 'B')
            return alcoholSprites[0];
        if (score[3] == 'B' && score[4] == 'B' && score[1] == 'E' && score[6] == 'B')
            return alcoholSprites[4];
        if (score[3] == 'B' && score[4] == 'B' && score[1] == 'B' && score[6] == 'E')
            return alcoholSprites[5];
        if (score[0] == 'E' && score[1] == 'E' && score[3] == 'E' && score[4] == 'B' && score[6] == 'B')
            return alcoholSprites[1];
        if (score[1] == 'E' && score[2] == 'E' && score[4] == 'E' && score[3] == 'B' && score[6] == 'B')
            return alcoholSprites[7];
        if (score[4] == 'E' && score[6] == 'E' && score[7] == 'E' && score[1] == 'B' && score[3] == 'B')
            return alcoholSprites[8];
        if (score[3] == 'E' && score[5] == 'E' && score[6] == 'E' && score[1] == 'B' && score[4] == 'B')
            return alcoholSprites[2];

        return errorSprite;
    }
}
