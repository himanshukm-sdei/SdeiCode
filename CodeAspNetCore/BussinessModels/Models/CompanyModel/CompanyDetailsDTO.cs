using System.Collections.Generic;

namespace BussinessModels.Models.CompanyModel
{
    public class finalCompanyDTO
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CustomerCode { get; set; }
        public string UserName { get; set; }
        public int CreatedBy { get; set; }
        public int PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public bool IsOldCompany { get; set; }
        public List<TagDTO> Tags { get; set; }
        public List<SharedWith> SharedWith { get; set; }
        public List<BranchDTO> Branches { get; set; }
    }
    public class TagDTO
    {
        public int TagsMappingId { get; set; }
        public string TagName { get; set; }
    }
    public class SharedWith
    {
        public int SharedWithId { get; set; }
        public string SharedWithname { get; set; }
        public string Email { get; set; }=string.Empty;

    }
    public class BranchDTO
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public bool IsHeadOffice { get; set; }
        public List<Branchcontact> Branchcontacts { get; set; }
        public List<Branchaddress> Branchaddresses { get; set; }
        public List<Branchmember> Branchmemberes { get; set; }

    }
    public class Branchcontact
    {
        public int BranchId { get; set; }
        public string PhoneNumber { get; set; }
       
    }
    public class Branchaddress
    {
        public int BranchId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
       
        public string CountryName { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
    }
    public class Branchmember
    {
        public int BranchId { get; set; }
        public string JobTitle { get; set; }
        public bool IsPrimary { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }

}


