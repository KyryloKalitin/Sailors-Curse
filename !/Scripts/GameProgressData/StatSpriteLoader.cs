using UnityEngine;

public static class StatSpriteLoader
{
    private static StatTypesSprites _cachedStatTypesSprites;

    private static string _resourcePath = "Data/StatTypesSprites";

    private static StatTypesSprites LoadStatTypesSprites()
    {
        if(_cachedStatTypesSprites == null)
        {
            _cachedStatTypesSprites = Resources.Load<StatTypesSprites>(_resourcePath);
        }

        return _cachedStatTypesSprites;
    }

    public static Sprite GetSpriteByType(StatsType type)
    {
        var statTypesSprites = LoadStatTypesSprites();

        if (statTypesSprites == null) return null;

        switch (type)
        {
            case StatsType.Food:
                return statTypesSprites.foodSprite.sprite;
            case StatsType.Water:
                return statTypesSprites.waterSprite.sprite;
            case StatsType.Material:
                return statTypesSprites.materialsSprite.sprite;
            default:
                return null;
        }
    }
}



