namespace JobExchange.Models
{
    public class Employer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set;}
        public string Phone { get; set;}
        public string AddressOfCompany { get; set;}
        public ICollection<JobInfo> Jobs { get; set;}
        public StoreUser User { get; set; }
    }
}
