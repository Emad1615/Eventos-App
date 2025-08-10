namespace Domain
{
    public class Activity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string City { get; set; } = string.Empty;
        public string Venue { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime InsertionDate { get; set; }
        public string InsertionUserID { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime DeletedDate { get; set; }
        public string DeletedUserID { get; set; } = string.Empty;
        public bool IsUpdated { get; set; } = false;
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedUserID { get; set; } = string.Empty;
        public bool IsCancelled { get; set; } = false;
        public DateTime CancellationDateTime { get; set; }
        public string CancellationUserID { get; set; } = string.Empty;

    }
}
