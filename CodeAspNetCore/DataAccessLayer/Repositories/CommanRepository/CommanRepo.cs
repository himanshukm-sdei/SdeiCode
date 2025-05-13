using BussinessModels.Models.CompanyModel;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer.Repositories.CommanRepository
{
    public class CommanRepo
    {
        // Connection string from the configuration file
        string connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

        /// <summary>
        /// Get a list of countries from the database.
        /// </summary>
        /// <returns>List of countries.</returns>
        public List<CountryMasterDTO> GetCountryList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Execute stored procedure to get countries
                var countries = connection.Query<CountryMasterDTO>("GetCountryList", commandType: CommandType.StoredProcedure).ToList();
                return countries;
            }
        }

        /// <summary>
        /// Get a list of registered users' emails based on the search text from the database.
        /// </summary>
        /// <param name="searchText">Search text for filtering emails.</param>
        /// <returns>List of registered users.</returns>
        public List<RegisteredUsersDetailsDTO> GetRegisteredUsersEmails(string searchText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Execute stored procedure to get registered users by details
                var users = connection.Query<RegisteredUsersDetailsDTO>("GetUsersByDetails", new { searchText }, commandType: CommandType.StoredProcedure).ToList();
                return users;
            }
        }

        /// <summary>
        /// Get a list of active tags from the database.
        /// </summary>
        /// <returns>List of tags.</returns>
        public List<TagsDTO> GetTagsData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Execute stored procedure to get active tags
                var tags = connection.Query<TagsDTO>("GetActiveTags", commandType: CommandType.StoredProcedure).ToList();
                return tags;
            }
        }
    }
}
