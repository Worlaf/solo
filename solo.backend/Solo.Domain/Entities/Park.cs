namespace Solo.Domain.Entities
{
    public class Park : EntityBase
    {
        public string Name { get; set; }
        public string DescriptionMarkdown { get; set; }
        public string ImageUrl { get; set; }

        /// <summary>
        /// <see cref="Map.Region"/>
        /// </summary>
        public string RegionJson { get; set; }
    }
}
