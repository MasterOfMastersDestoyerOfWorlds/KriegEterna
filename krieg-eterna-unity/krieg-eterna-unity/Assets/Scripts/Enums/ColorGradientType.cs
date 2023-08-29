using TMPro;
public class ColorGradientType
{
    public static VertexGradient Power = new VertexGradient(new UnityEngine.Color(0.7333f, 0.5098f, 1f), new UnityEngine.Color(0.7333f, 0.5098f, 1f),
    new UnityEngine.Color(0.5215f, 0.2313f, 0.8156f), new UnityEngine.Color(0.5215f, 0.2313f, 0.8156f));
    public static VertexGradient Melee = new VertexGradient(new UnityEngine.Color(0.9450f, 0.4039f, 0.4039f), new UnityEngine.Color(0.9450f, 0.4039f, 0.4039f),
    new UnityEngine.Color(0.7176f, 0f, 0f), new UnityEngine.Color(0.7176f, 0f, 0f));
    public static VertexGradient Ranged = new VertexGradient(new UnityEngine.Color(1f, 0.7176f, 0.4588f), new UnityEngine.Color(1f, 0.7176f, 0.4588f),
    new UnityEngine.Color(0.7176f, 0.3529f, 0f), new UnityEngine.Color(0.7176f, 0.3529f, 0f));
    public static VertexGradient Siege = new VertexGradient(new UnityEngine.Color(1f, 0.9647f, 0.6823f), new UnityEngine.Color(1f, 0.9647f, 0.6823f),
    new UnityEngine.Color(0.7411f, 0.5568f, 0.0078f), new UnityEngine.Color(0.7411f, 0.5568f, 0.0078f));
    public static VertexGradient Spy = new VertexGradient(new UnityEngine.Color(0.4705f, 0.4666f, 0.4666f), new UnityEngine.Color(0.4705f, 0.4666f, 0.4666f),
    new UnityEngine.Color(0.0941f, 0.0862f, 0.0862f), new UnityEngine.Color(0.0941f, 0.0862f, 0.0862f));
    public static VertexGradient Switch = new VertexGradient();
    public static VertexGradient Default = new VertexGradient(new UnityEngine.Color(1f, 0.9137f, 0f), new UnityEngine.Color(1f, 0.9137f, 0f),
    new UnityEngine.Color(1f, 0f, 0f), new UnityEngine.Color(1f, 0f, 0f));

    public static UnityEngine.Color BrightOutline = new UnityEngine.Color(1f, 1f, 1f);
    public static UnityEngine.Color DefaultOutline = new UnityEngine.Color(0.3460012f, 0.3460012f, 0.3460012f);

    public static UnityEngine.Color DarkOutline2 = new UnityEngine.Color(0.1886792f, 0.1886792f, 0.1886792f);
    public static UnityEngine.Color DefaultOutline2 = new UnityEngine.Color(0.1886792f, 0.1886792f, 0.1886792f);

    public static VertexGradient getTextGradient(CardType type)
    {
        switch (type)
        {
            case CardType.Melee: return Melee;
            case CardType.Ranged: return Ranged;
            case CardType.Siege: return Siege;
            case CardType.Switch: return Melee;
            case CardType.Spy: return Spy;
            case CardType.Power: return Power;
        }
        return Default;
    }
    public static UnityEngine.Color getTextOutline(CardType type)
    {
        switch (type)
        {
            case CardType.Spy: return BrightOutline;
            case CardType.Power: return BrightOutline;
        }
        return DefaultOutline;
    }

    public static UnityEngine.Color getTextOutline2(CardType type){
        switch (type)
        {
            case CardType.Spy: return DarkOutline2;
            case CardType.Power: return DarkOutline2;
        }
        return DefaultOutline2;
    }
}

