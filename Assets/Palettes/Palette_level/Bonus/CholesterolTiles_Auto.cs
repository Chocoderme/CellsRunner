using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CholesterolTiles_Auto : Tile {

    [SerializeField]
    private Sprite[] cholesterolSprites;

    [SerializeField]
    private Sprite preview;

    [SerializeField]
    private Sprite errorSprite;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/Tiles/CholesterolTile")]
    public static void CreateCholesterolTile()
    {
        string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Cholesterol Tile", "New Cholesterol Tile", "asset", "Save Cholesterol Tile", "Assets");
        if (path == "")
            return;
        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CholesterolTiles_Auto>(), path);
    }
#endif

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int nPos = new Vector3Int(position.x + i, position.y + j, position.z);
                if (isCholesterol(tilemap, nPos))
                    tilemap.RefreshTile(nPos);
            }
        }
    }

    private bool isCholesterol(ITilemap map, Vector3Int pos)
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
                    if (isCholesterol(tilemap, nPos))
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
            return cholesterolSprites[16];
        if (score[5] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return cholesterolSprites[15];
        if (score[7] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return cholesterolSprites[10];
        if (score[2] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return cholesterolSprites[11];

        if (score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return cholesterolSprites[3];
        if (score[3] == 'B' && score[4] == 'E' && score[1] == 'B' && score[6] == 'B')
            return cholesterolSprites[6];
        if (score[3] == 'E' && score[4] == 'B' && score[1] == 'B' && score[6] == 'B')
            return cholesterolSprites[0];
        if (score[3] == 'B' && score[4] == 'B' && score[1] == 'E' && score[6] == 'B')
            return cholesterolSprites[4];
        if (score[3] == 'B' && score[4] == 'B' && score[1] == 'B' && score[6] == 'E')
            return cholesterolSprites[5];
        if (score[0] == 'E' && score[1] == 'E' && score[3] == 'E' && score[4] == 'B' && score[6] == 'B')
            return cholesterolSprites[1];
        if (score[1] == 'E' && score[2] == 'E' && score[4] == 'E' && score[3] == 'B' && score[6] == 'B')
            return cholesterolSprites[7];
        if (score[4] == 'E' && score[6] == 'E' && score[7] == 'E' && score[1] == 'B' && score[3] == 'B')
            return cholesterolSprites[8];
        if (score[3] == 'E' && score[5] == 'E' && score[6] == 'E' && score[1] == 'B' && score[4] == 'B')
            return cholesterolSprites[2];

        return errorSprite;
    }
}
