namespace BussinessModels.Models.Company
{
    public class CompanyDTO
    {
        public int Companyid { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }
       
        public string CreatedBy { get; set; }
        public string SharedWithName { get; set; }

        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public int TotalRows { get; set; }
    }
}
