using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTiles_Auto : Tile {

    [SerializeField]
    private Sprite[] backgroundSprites;

    [SerializeField]
    private Sprite preview;

    [SerializeField]
    private Sprite errorSprite;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/Tiles/BackgroundTile")]
    public static void CreateBackgroundTile()
    {
        string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Background Tile", "New Background Tile", "asset", "Save Background Tile", "Assets");
        if (path == "")
            return;
        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BackgroundTiles_Auto>(), path);
    }
#endif

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int nPos = new Vector3Int(position.x + i, position.y + j, position.z);
                if (isBackground(tilemap, nPos))
                    tilemap.RefreshTile(nPos);
            }
        }
    }

    private bool isBackground(ITilemap map, Vector3Int pos)
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
                    if (isBackground(tilemap, nPos))
                        score += "B";
                    else
                        score += "E";
                }
            }
        }

        tileData.sprite = getTile(score);
        if (tileData.sprite == backgroundSprites[2])
            tileData.colliderType = ColliderType.None;
        else
            tileData.colliderType = ColliderType.Sprite;
    }

    private Sprite getTile(string score)
    {
        if (score[0] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return backgroundSprites[17];
        if (score[5] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return backgroundSprites[16];
        if (score[7] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return backgroundSprites[10];
        if (score[2] == 'E' && score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return backgroundSprites[11];

        if (score[1] == 'B' && score[3] == 'B' && score[4] == 'B' && score[6] == 'B')
            return backgroundSprites[2];
        if (score[3] == 'B' && score[4] == 'E' && score[1] == 'B' && score[6] == 'B')
            return backgroundSprites[7];
        if (score[3] == 'E' && score[4] == 'B' && score[1] == 'B' && score[6] == 'B')
            return backgroundSprites[0];
        if (score[3] == 'B' && score[4] == 'B' && score[1] == 'E' && score[6] == 'B')
            return backgroundSprites[3];
        if (score[3] == 'B' && score[4] == 'B' && score[1] == 'B' && score[6] == 'E')
            return backgroundSprites[5];
        if (score[0] == 'E' && score[1] == 'E' && score[3] == 'E' && score[4] == 'B' && score[6] == 'B')
            return backgroundSprites[1];
        if (score[1] == 'E' && score[2] == 'E' && score[4] == 'E' && score[3] == 'B' && score[6] == 'B')
            return backgroundSprites[8];
        if (score[4] == 'E' && score[6] == 'E' && score[7] == 'E' && score[1] == 'B' && score[3] == 'B')
            return backgroundSprites[6];
        if (score[3] == 'E' && score[5] == 'E' && score[6] == 'E' && score[1] == 'B' && score[4] == 'B')
            return backgroundSprites[4];

        return errorSprite;
    }
}
