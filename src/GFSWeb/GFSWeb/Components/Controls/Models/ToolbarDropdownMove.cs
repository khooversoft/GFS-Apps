using Toolbox.Types;

namespace GFSWeb.Components.Controls;

public enum PossibleMove
{
    None,
    Up,
    Down,
    Both,
}

public static class ToolbarDropdownMove
{
    public const string Up = "up";
    public const string Down = "down";
    public const string Top = "top";
    public const string Bottom = "bottom";

    public static readonly IReadOnlyList<KeyValue<string>> MoveList = [
        (Down, "Move down"),
        (Bottom, "Move to bottom"),
        (Top, "Move to top"),
        (Up, "Move up"),
    ];

    public static IReadOnlyList<KeyValue<string>> GetMoveList(PossibleMove possibleMove)
    {
        return possibleMove switch
        {
            PossibleMove.None => [],
            PossibleMove.Up => [.. MoveList.Where(x => x.Key == Up || x.Key == Top)],
            PossibleMove.Down => [.. MoveList.Where(x => x.Key == Down || x.Key == Bottom)],
            PossibleMove.Both => MoveList,
            _ => throw new ArgumentOutOfRangeException(nameof(possibleMove), "Invalid enum value"),
        };
    }

    public static PossibleMove CanMove(int index, int listCount)
    {
        if (index < 0 || index >= listCount) return PossibleMove.None;
        if (listCount <= 0) return PossibleMove.None;

        if (index == 0 && index == listCount - 1) return PossibleMove.None;
        if (index == 0) return PossibleMove.Down;
        if (index == listCount - 1) return PossibleMove.Up;

        return PossibleMove.Both;
    }
}