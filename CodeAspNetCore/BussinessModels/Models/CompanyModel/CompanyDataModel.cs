using System.Collections.Generic;

namespace BussinessModels.Models.CompanyModel
{

    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public List<CompanyTagModel> Tags { get; set; }
        public List<CompanySharedWithModel> SharedWith { get; set; }

        public string CustomerCode { get; set; }
        public string PaymentTerms { get; set; }
        public List<BranchModel> Branch { get; set; }
    }
    public class CompanyTagModel
    {
        public int TagsMappingId { get; set; }

    }
    public class CompanySharedWithModel
    {
        public int SharedWithId { get; set; }

    }
    public class CompanyPhoneNumberModel
    {
        public string PhoneNumber { get; set; }
        public string Ext { get; set; }
    }

    public class CompanyAddressModel
    {
        public string AddressTypeId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }



    public class CompanyMemberDetailModel
    {
        public int MemberId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string MemberName { get; set; }
        public string JobTitle { get; set; }
        public bool IsPrimary { get; set; }
    }



    public class BranchModel
    {
        public string BranchName { get; set; }
        public bool HeadOffice { get; set; }
        public List<CompanyPhoneNumberModel> Contact { get; set; }
        public List<CompanyAddressModel> Addresses { get; set; }
        public List<CompanyMemberDetailModel> Members { get; set; }
    }
}
