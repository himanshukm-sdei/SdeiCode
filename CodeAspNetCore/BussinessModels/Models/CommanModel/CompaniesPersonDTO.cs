using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessModels.Models.CommanModel
{
    public class CompaniesPersonDTO
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }=string.Empty;
        public string CompanyName { get; set; }
    }
}
