using System.Drawing;

namespace Smurf_Village_Statistical_Office.DTO.ColorDtos
{
    public record ColorDto
    {
        public byte Red { get; init; }
        public byte Green { get; init; }
        public byte Blue { get; init; }
        public byte Alpha {  get; init; }

        public static ColorDto FromColor(Color color)
        {
            return new ColorDto
            {
                Red = color.R,
                Green = color.G,
                Blue = color.B,
                Alpha = color.A
            };
        }

        public static Color ToColor(ColorDto color)
        {
            return Color.FromArgb(
                color.Alpha,
                color.Red,
                color.Green,
                color.Blue);
        }
    }
}
