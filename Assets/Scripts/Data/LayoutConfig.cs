public static class LayoutConfig
{
    public static int Rows { get; private set; }
    public static int Columns { get; private set; }

    public static void SetLayout(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
    }
}
