using BussinessModels.Models.Company;
using BussinessModels.Models.CompanyModel;
using BussinessModels.Models.Constants;
using Dapper;
using DataAccessLayer.DBModels;
using DataAccessLayer.Repositories.CommanRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer.Repositories.CompanyRepository
{
    public class CompanyRepo
    {
        // Utility class for common functions
        CommanUtilities _utilities = new CommanUtilities();
        // Connection string from the configuration file
        string connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

        // Add or Update a company in the database
        public CommonResponse AddEditCompany(CompanyModel data)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (DBContextDataContext dbContext = new DBContextDataContext())
                {
                    TimeZoneInfo ausTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                    DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, ausTimeZone);

                    // Check if the company already exists
                    Company company = dbContext.Companies.FirstOrDefault(x => x.CompanyId == data.CompanyId && x.IsActive == true && x.IsDeleted == false);

                    if (company == null)
                    {
                        response = CreateNewCompany(data, dbContext, currentTime);
                    }
                    else
                    {
                        response = UpdateExistingCompany(data, company, dbContext, currentTime);
                    }
                }
            }
            catch (Exception ex)
            {
                _utilities.LogException(ex);
            }
            return response;
        }

        // Create a new company in the database
        private CommonResponse CreateNewCompany(CompanyModel data, DBContextDataContext dbContext, DateTime currentTime)
        {
            CommonResponse response = new CommonResponse();
            Company newCompany = new Company
            {
                CompanyName = data.CompanyName,
                CustomerCode = data.CustomerCode,
                PaymentTermId = Convert.ToInt32(data.PaymentTerms),
                IsActive = true,
                IsDeleted = false,
                CreatedBy = data.CreatedBy,
                CreatedDate = currentTime,
                ModifiedBy = null,
                ModifiedDate = null
            };
            dbContext.Companies.InsertOnSubmit(newCompany);
            dbContext.SubmitChanges();
            int companyId = newCompany.CompanyId;
            response.Id = companyId;
            if (data.SharedWith.Count > 0 && data.SharedWith != null)
            {
                AddSharedWith(data.SharedWith, companyId, dbContext, data.CreatedBy, currentTime);
            }
            if (data.Tags.Count > 0 && data.Tags != null)
            {
                AddTags(data.Tags, companyId, dbContext, data.CreatedBy, currentTime);
            }
            if (data.Branch.Count > 0 && data.Branch != null)
            {
                AddBranches(data.Branch, companyId, dbContext, data.CreatedBy, currentTime);
            }
            response.message = CommonConstants.CompanyAddedSuccesfully;
            return response;
        }

        // Update an existing company in the database
        private CommonResponse UpdateExistingCompany(CompanyModel data, Company company, DBContextDataContext dbContext, DateTime currentTime)
        {
            CommonResponse response = new CommonResponse();
            company.CompanyName = data.CompanyName;
            company.CustomerCode = data.CustomerCode;
            company.PaymentTermId = Convert.ToInt32(data.PaymentTerms);
            company.IsActive = true;
            company.IsDeleted = false;
            company.ModifiedBy = data.CreatedBy;
            company.ModifiedDate = currentTime;
            dbContext.SubmitChanges();

            int companyId = company.CompanyId;
            UpdateSharedWith(data.SharedWith, companyId, dbContext, data.CreatedBy, currentTime);
            UpdateTags(data.Tags, companyId, dbContext, data.CreatedBy, currentTime);
            UpdateBranches(data.Branch, companyId, dbContext, data.CreatedBy, currentTime);
            response.Id = company.CompanyId;
            response.message = CommonConstants.CompanyUpdateSuccesfully;
            return response;
        }

        // Add shared-with details for a company
        private void AddSharedWith(List<CompanySharedWithModel> sharedWith, int companyId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            foreach (var share in sharedWith)
            {
                CompanySharedWith newCompanySharedWith = new CompanySharedWith
                {
                    SharedWithId = share.SharedWithId,
                    CompanyId = companyId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreatedDate = currentTime,
                    ModifiedBy = null,
                    ModifiedDate = null
                };
                dbContext.CompanySharedWiths.InsertOnSubmit(newCompanySharedWith);
            }
            dbContext.SubmitChanges();
        }

        // Update shared-with details for a company
        private void UpdateSharedWith(List<CompanySharedWithModel> sharedWith, int companyId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            dbContext.CompanySharedWiths.Where(ct => ct.CompanyId == companyId).ToList().ForEach(record =>
            {
                record.IsDeleted = true;
                record.IsActive = false;
                record.ModifiedBy = createdBy;
                record.ModifiedDate = currentTime;
            });
            dbContext.SubmitChanges();
            if (sharedWith.Count > 0 && sharedWith != null)
            {
                AddSharedWith(sharedWith, companyId, dbContext, createdBy, currentTime);
            }
        }

        // Add tags for a company
        private void AddTags(List<CompanyTagModel> tags, int companyId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            foreach (var tag in tags)
            {
                CompanyTag newCompanyTag = new CompanyTag
                {
                    TagsMappingId = tag.TagsMappingId,
                    CompanyId = companyId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreatedDate = currentTime,
                    ModifiedBy = null,
                    ModifiedDate = null
                };
                dbContext.CompanyTags.InsertOnSubmit(newCompanyTag);
            }
            dbContext.SubmitChanges();
        }

        // Update tags for a company
        private void UpdateTags(List<CompanyTagModel> tags, int companyId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            dbContext.CompanyTags.Where(ct => ct.CompanyId == companyId).ToList().ForEach(record =>
            {
                record.IsDeleted = true;
                record.IsActive = false;
                record.ModifiedBy = createdBy;
                record.ModifiedDate = currentTime;
            });
            dbContext.SubmitChanges();

            if (tags.Count > 0 && tags != null)
            {
                AddTags(tags, companyId, dbContext, createdBy, currentTime);
            }
        }

        // Add branches for a company
        private void AddBranches(List<BranchModel> branches, int companyId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            foreach (var branch in branches)
            {
                Branch newBranch = new Branch
                {
                    CompanyId = companyId,
                    BranchName = branch.BranchName,
                    IsHeadOffice = branch.HeadOffice,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreatedDate = currentTime,
                    ModifiedBy = null,
                    ModifiedDate = null
                };
                dbContext.Branches.InsertOnSubmit(newBranch);
                dbContext.SubmitChanges();
                int branchId = newBranch.BranchId;
                if (branch.Contact.Count > 0)
                {
                    AddContacts(branch.Contact, branchId, dbContext, createdBy, currentTime);
                }
                if (branch.Addresses.Count > 0)
                {
                    AddAddresses(branch.Addresses, branchId, dbContext, createdBy, currentTime);
                }
                if (branch.Members.Count > 0)
                {
                    AddMembers(branch.Members, branchId, dbContext, createdBy, currentTime);
                }
            }
        }

        // Update branches for a company
        private void UpdateBranches(List<BranchModel> branches, int companyId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            dbContext.Branches
                .Where(ct => ct.CompanyId == companyId)
                .ToList()
                .ForEach(record =>
                {
                    dbContext.BranchMembers.Where(bm => bm.BranchId == record.BranchId).ToList().ForEach(member =>
                    {
                        member.IsDeleted = true;
                        member.IsActive = false;
                        member.ModifiedBy = createdBy;
                        member.ModifiedDate = currentTime;
                    });
                    record.IsDeleted = true;
                    record.IsActive = false;
                    record.ModifiedBy = createdBy;
                    record.ModifiedDate = currentTime;
                });
            dbContext.SubmitChanges();
            if (branches.Count > 0 && branches != null)
            {
                AddBranches(branches, companyId, dbContext, createdBy, currentTime);
            }
        }

        // Add contacts for a branch
        private void AddContacts(List<CompanyPhoneNumberModel> contacts, int branchId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            foreach (var contact in contacts)
            {
                BranchContact newBranchPhoneNumber = new BranchContact
                {
                    BranchId = branchId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreatedDate = currentTime,
                    ModifiedBy = null,
                    ModifiedDate = null,
                    PhoneNumber = contact.PhoneNumber
                };
                dbContext.BranchContacts.InsertOnSubmit(newBranchPhoneNumber);
            }
            dbContext.SubmitChanges();
        }

        // Add addresses for a branch
        private void AddAddresses(List<CompanyAddressModel> addresses, int branchId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            foreach (var address in addresses)
            {
                BranchAddress newBranchAddress = new BranchAddress
                {
                    BranchId = branchId,
                    AddressTypeId = Convert.ToInt32(address.AddressTypeId),
                    ZipCode = address.ZipCode,
                    Country = address.Country,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreatedDate = currentTime,
                    ModifiedBy = null,
                    ModifiedDate = null,
                    State = address.State,
                    City = address.City,
                    Street = address.Street.Replace(Environment.NewLine, "<br/>")
                };
                dbContext.BranchAddresses.InsertOnSubmit(newBranchAddress);
            }
            dbContext.SubmitChanges();
        }

        // Add members for a branch
        private void AddMembers(List<CompanyMemberDetailModel> members, int branchId, DBContextDataContext dbContext, int createdBy, DateTime currentTime)
        {
            foreach (var member in members)
            {
                int memberId = member.MemberId;
                if (memberId == 0)
                {
                    Member newMember = new Member
                    {
                        Name = member.MemberName,
                        Email = member.Email,
                        IsUser = true,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = createdBy,
                        CreatedDate = currentTime,
                        ModifiedBy = null,
                        ModifiedDate = null
                    };
                    dbContext.Members.InsertOnSubmit(newMember);
                    dbContext.SubmitChanges();
                    memberId = newMember.MemberId;
                }
                else
                {
                    Member newMember = dbContext.Members.Where(x => x.MemberId == memberId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    newMember.Name = member.MemberName;
                    newMember.Email = member.Email;
                    newMember.IsUser = true;
                    newMember.IsActive = true;
                    newMember.IsDeleted = false;
                    newMember.ModifiedBy = createdBy;
                    newMember.ModifiedDate = currentTime;
                    dbContext.SubmitChanges();
                }
                var PrimaryPhoneExist = dbContext.PersonPhones.Where(x => x.MemberID == memberId && x.IsPrimary == true && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                if (PrimaryPhoneExist != null)
                {
                    PrimaryPhoneExist.PhoneNumber = member.PhoneNumber;
                    PrimaryPhoneExist.ModifiedBy = createdBy;
                    PrimaryPhoneExist.ModifiedDate = currentTime;
                    dbContext.SubmitChanges();
                }
                else
                {
                    PersonPhone personPhone = new PersonPhone
                    {
                        MemberID = memberId,
                        PhoneNumber = member.PhoneNumber,
                        CreatedBy = createdBy,
                        IsPrimary = true,
                        IsActive = true,
                        IsDeleted = false,
                    };
                    dbContext.PersonPhones.InsertOnSubmit(personPhone);
                }

                BranchMember newBranchMember = new BranchMember
                {
                    BranchId = branchId,
                    MemberId = memberId,
                    JobTitle = member.JobTitle,
                    IsPrimary = member.IsPrimary,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreatedDate = currentTime,
                    ModifiedBy = null,
                    ModifiedDate = null
                };
                dbContext.BranchMembers.InsertOnSubmit(newBranchMember);
            }
            dbContext.SubmitChanges();
        }

        // Get a list of payment terms from the database
        public List<PaymentTermsDTO> GetPaymentTerms()
        {
            SqlDataAdapter adap = new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("GetPaymentTerms", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable paymentTerm = new DataTable();
                        adapter.Fill(paymentTerm);
                        List<PaymentTermsDTO> list = paymentTerm.Rows.Cast<DataRow>()
                           .Select(dr => new PaymentTermsDTO
                           {
                               PaymentTermId = Convert.ToInt32(dr["PaymentTermId"]),
                               PaymentTermName = Convert.ToString(dr["PaymentTermName"])
                           }).ToList();
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    _utilities.LogException(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Get paginated list of all companies from the database
        public List<CompanyDTO> GetAllCompanies(int pageNumber, int pageSize)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                var companies = connection.Query<CompanyDTO>("GetCompanyDataDetails", parameters, commandType: CommandType.StoredProcedure).ToList();
                return companies;
            }

        }
        public List<CompanyDTO> GetAllCompanies()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var companies = connection.Query<CompanyDTO>("GetCompanyAllDataDetails", commandType: CommandType.StoredProcedure).ToList();
                return companies;
            }
        }
        public finalCompanyDTO GetCompanyDetailsById(int companyId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.QueryMultiple("GetCompaniesDetailsById", new { CompanyId = companyId }, commandType: CommandType.StoredProcedure);
                var mainCompanyDetails = result.Read<finalCompanyDTO>().SingleOrDefault();
                var tags = result.Read<TagDTO>().ToList();
                var sharedWith = result.Read<SharedWith>().ToList();
                var branches = result.Read<BranchDTO>().ToList();
                var branchescontact = result.Read<Branchcontact>().ToList();
                var branchesaddress = result.Read<Branchaddress>().ToList();
                var branchesmember = result.Read<Branchmember>().ToList();

                if (mainCompanyDetails != null)
                {
                    mainCompanyDetails.Tags = tags;
                    mainCompanyDetails.SharedWith = sharedWith;
                    mainCompanyDetails.Branches = branches;

                    foreach (var branch in mainCompanyDetails.Branches)
                    {
                        branch.Branchcontacts = branchescontact.Where(bc => bc.BranchId == branch.BranchId).ToList();
                        branch.Branchaddresses = branchesaddress.Where(ba => ba.BranchId == branch.BranchId).ToList();
                        branch.Branchmemberes = branchesmember.Where(bm => bm.BranchId == branch.BranchId).ToList();
                    }

                    return mainCompanyDetails;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsCustomerCodeExist(string customerCode)
        {
            DBContextDataContext dBContext = new DBContextDataContext();
            var result = dBContext.Companies.Where(x => x.CustomerCode == customerCode).FirstOrDefault();
            return result != null ? true : false;
        }


        public List<CompanyMembersDto> GetCompanyMembers(int branchId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<CompanyMembersDto> companyMembersDtos = connection.Query<CompanyMembersDto>("GetActiveCompanyMembers", commandType: CommandType.StoredProcedure).ToList();
                return companyMembersDtos;
            }
        }
        public bool DeleteCompany(int companyId)
        {
            using (var dbContext = new DBContextDataContext())
            {
                var companyToUpdate = dbContext.Companies.Where(cmp => cmp.CompanyId == companyId).FirstOrDefault();

                if (companyToUpdate != null)
                {
                    companyToUpdate.IsActive = false;
                    companyToUpdate.IsDeleted = true;
                    var branchToDelete = dbContext.Branches.Where(cmp => cmp.CompanyId == companyId).ToList();
                    if (branchToDelete != null)
                    {
                        foreach (var branch in branchToDelete)
                        {
                            var branchMemberTodelete = dbContext.BranchMembers.Where(br => br.BranchId == branch.BranchId).ToList();
                            if (branchMemberTodelete != null)
                            {
                                foreach (var mem in branchMemberTodelete)
                                {
                                    mem.IsActive = false;
                                    mem.IsDeleted = true;
                                }
                            }
                            branch.IsActive = false;
                            branch.IsDeleted = true;
                        }
                    }
                    dbContext.SubmitChanges();
                    return true;
                }

            }

            return false;

        }

        public bool DeletePerson(int personId)
        {
            using (var dbContext = new DBContextDataContext())
            {
                var PersonToDelete = dbContext.Members.Where(mem => mem.MemberId == personId).FirstOrDefault();

                if (PersonToDelete != null)
                {
                    PersonToDelete.IsActive = false;
                    PersonToDelete.IsDeleted = true;
                    dbContext.BranchMembers.Where(x => x.MemberId == personId).ToList().ForEach(record =>
                    {
                        record.IsDeleted = true;
                        record.IsActive = false;
                    });
                    dbContext.SubmitChanges();
                    return true;
                }

            }
            return false;
        }
    }
}


