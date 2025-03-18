using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Domain.Entities
{
    public class MonthlyVisit
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; } 
        public int Month { get; set; } 
        public int VisitCount { get; set; }
    }

}
