namespace JobExchange.Models
{
    public class TypeJob
    {
        public int Id { get; set; }
        public string NameJob { get; set; }
        public ICollection<JobInfo> JobInfos { get; set; }
    }
}
