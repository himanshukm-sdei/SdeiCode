using BussinessModels.Models.Company;
using BussinessModels.Models.CompanyModel;
using DataAccessLayer.Repositories.CompanyRepository;
using System.Collections.Generic;

namespace BusinessLayer.Services.CompanyService
{
    public class CompanyBL
    {
        CompanyRepo _context = new CompanyRepo();
       
        public CommonResponse AddEditCompany(CompanyModel data)
        {
            try
            {
                return _context.AddEditCompany(data);
            }
            catch
            {
                throw;
            }
        }
        public List<CompanyDTO> GetAllCompanies()
        {
            try
            {
               
                return _context.GetAllCompanies();
            }
            catch
            {
                throw;
            }
        }
        public List<CompanyDTO> GetAllCompanies(int pageNumber, int pageSize)
        {
            try
            {
                return _context.GetAllCompanies(pageNumber, pageSize);
            }
            catch
            {
                throw;
            }
        }
        public finalCompanyDTO GetCompanyDetailsById(int companyId)
        {
            try
            {
               
                return _context.GetCompanyDetailsById(companyId);
            }
            catch
            {
                return null;
            }
        }
        public List<PaymentTermsDTO> GetPaymentTerms()
        {
            try
            {
               
                return _context.GetPaymentTerms();
            }
            catch
            {
                return null;
            }
        }
        public bool IsCustomerCodeExist(string customerCode)
        {
            try
            {
                
                return _context.IsCustomerCodeExist(customerCode);
            }
            catch
            {
                return false;
            }
        }
        public List<CompanyMembersDto> GetCompanyMembers(int branchId)
        {
            try
            {
               
                return _context.GetCompanyMembers(branchId);
            }
            catch
            {
                return null;
            }
        }
        public bool DeletePerson(int personId)
        {
            try
            {

                return _context.DeletePerson(personId);
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteCompany(int companyId)
        {
            try
            {

                return _context.DeleteCompany(companyId);
            }
            catch
            {
                return false;
            }
        }


    }
}
