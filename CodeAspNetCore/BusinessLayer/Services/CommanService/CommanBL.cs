using BussinessModels.Models.CompanyModel;
using DataAccessLayer.Repositories.CommanRepository;
using System.Collections.Generic;

namespace BusinessLayer.Services.CommanService
{
    public class CommanBL
    {
        private readonly CommanRepo _context;

        // Constructor to initialize CommanRepo
        public CommanBL()
        {
            _context = new CommanRepo();
        }

        /// <summary>
        /// Get a list of countries.
        /// </summary>
        /// <returns>List of countries.</returns>
        public List<CountryMasterDTO> GetCountryList()
        {
            return _context.GetCountryList();
        }

        /// <summary>
        /// Get a list of registered users' emails based on the search text.
        /// </summary>
        /// <param name="searchText">Search text for filtering emails.</param>
        /// <returns>List of emails.</returns>
        public List<RegisteredUsersDetailsDTO> GetRegisteredUsersEmails(string searchText)
        {
            return _context.GetRegisteredUsersEmails(searchText);
        }

        /// <summary>
        /// Get a list of tags.
        /// </summary>
        /// <returns>List of tags.</returns>
        public List<TagsDTO> GetTagsData()
        {
            return _context.GetTagsData();
        }
    }
}
