namespace MapEditor.Models
{
    public class MapEntity : MapObject
    {
        protected override string GetStringValue()
        {
            var positionString = $"{this.X}:{this.Y}:{this.Level}";
            var pagesString = this.Pages.StringValue;
            return string.Join("\r\n", new[]
            {
               $"ID:{this.ID}",
               $"position:{positionString}",
               pagesString
            });
        }
    }
}
