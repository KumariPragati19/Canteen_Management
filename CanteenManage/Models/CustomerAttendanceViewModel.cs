namespace CanteenManage.Models
{
    public class CustomerAttendanceViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateOnly Date { get; set; }
        public string PreviousStatus { get; set; }
        public string CurrentStatus { get; set; }
        public string NewStatus { get; set; }
    }
}
