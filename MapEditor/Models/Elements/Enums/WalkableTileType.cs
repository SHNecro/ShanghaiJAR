namespace MapEditor.Models.Elements.Enums
{
    public enum WalkableTileType
    {
        Empty = 0,
        Full = 1,
        BottomLeftSlantMajority = 2,
        TopRightSlantMajority = 3,
        TopLeftSlantMajority = 4,
        BottomRightSlantMajority = 5,
        TopRightSlantFiller = 6,
        BottomLeftSlantFiller = 7,
        BottomRightSlantFiller = 8,
        TopLeftSlantFiller = 9,
        RightRamp = 10,
        DownRamp = 11,
        LeftRamp = 12,
        UpRamp = 13,
        LeftRightRampTop = -10,
        UpDownRampTop = -11,
        LeftRightRampBottom = -12,
        UpDownRampBottom = -13,
        ConveyorNorth = 14,
        ConveyorSouth = 15,
        ConveyorWest = 16,
        ConveyorEast = 17,
    }
}
