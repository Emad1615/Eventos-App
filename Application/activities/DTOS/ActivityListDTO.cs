
namespace Application.activities.DTOS
{
    public class ActivityListDTO:BaseActivityDTO
    {
        public string ID { get; set; }=string.Empty;
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
        public string Status => IsCancelled ? "Cancelled ✖️" : "Active ✔️";
    }
}
