using System;
using static Quoridor.Board;

namespace Quoridor;

public static class Preset
{
    public static readonly PlayerType[] Preset1 = new PlayerType[]
    {
        PlayerType.Human,
        PlayerType.Human,
        PlayerType.Human,
        PlayerType.Human,
    };

    public static readonly PlayerType[] Preset2 = new PlayerType[]
    {
        PlayerType.Human,
        PlayerType.Human
    };
}