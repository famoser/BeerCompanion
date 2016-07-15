namespace Famoser.BeerCompanion.View.Models
{
    public class Color
    {
        public Color(string value, string name)
        {
            ColorValue = value;
            ColorName = name;
        }

        public string ColorValue { get; set; }
        public string ColorName { get; set; }
    }
}
