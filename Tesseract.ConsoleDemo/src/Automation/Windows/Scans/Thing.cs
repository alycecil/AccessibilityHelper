namespace runner
{
    public class Thing
    {
        public Thing(int x, int y, VerbWindow verbWindow, string name)
        {
            this.x = x;
            this.y = y;
            this.verbWindow = verbWindow;
            this.name = name;
        }

        public int x, y;
        public VerbWindow verbWindow;
        public string name;
    }
}