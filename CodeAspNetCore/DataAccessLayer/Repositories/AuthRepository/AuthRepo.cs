using BussinessModels.Models.AuthModel;
using BussinessModels.Models.CommanResponse;
using BussinessModels.Models.Constants;
using DataAccessLayer.DBModels;
using DataAccessLayer.Repositories.CommanRepository;
using System;
using System.Configuration;
using System.Data.Linq;
using System.Linq;

namespace DataAccessLayer.Repositories.AuthRepository
{
    public class AuthRepo : IDisposable
    {
        private readonly DataContext context;
        private readonly CommanUtilities _utilities;

        public AuthRepo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            context = new DataContext(connectionString);
            _utilities = new CommanUtilities();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public CommonResponse<bool> IsEmailConfirmed(string email)
        {
            try
            {
                var users = context.GetTable<User>();
                bool isConfirmed = users.Any(u => u.Email == email);
                return new CommonResponse<bool>(true, CommonConstants.EmailConfirmationSuccessMessage, isConfirmed);
            }
            catch (Exception ex)
            {
                _utilities.LogException(ex);
                return new CommonResponse<bool>(false, CommonConstants.EmailConfirmationErrorMessage, false);
            }
        }

        public CommonResponse<string> IsUserValid(LoginDTO request)
        {
            try
            {
                var users = context.GetTable<User>();
                var user = users.FirstOrDefault(u => u.Email == request.EmailId);

                if (user != null)
                {
                    return new CommonResponse<string>(true, CommonConstants.UserValidMessage, user.Password);
                }
                else
                {
                    return new CommonResponse<string>(true, CommonConstants.UserNotFoundMessage, string.Empty);
                }
            }
            catch (Exception ex)
            {
                _utilities.LogException(ex);
                return new CommonResponse<string>(false, CommonConstants.ValidationErrorMessage, CommonConstants.SomethingWentWrongMessage);
            }
        }

        public CommonResponse<bool> RegisterUser(RegisterDTO request)
        {
            try
            {
                var users = context.GetTable<User>();
                var newUser = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = request.Password,
                    Phonenumber = request.PhoneNumber,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    UserTypeId = 1,
                    IsEmailConfirmed = true,
                };

                users.InsertOnSubmit(newUser);
                context.SubmitChanges();

                return new CommonResponse<bool>(true, CommonConstants.RegistrationSuccessMessage, true);
            }
            catch (Exception ex)
            {
                _utilities.LogException(ex);
                return new CommonResponse<bool>(false, CommonConstants.RegistrationErrorMessage, false);
            }
        }

        public CommonResponse<int> GetUserId(LoginDTO request)
        {
            try
            {
                var users = context.GetTable<User>();
                var user = users.FirstOrDefault(u => u.Email == request.EmailId);

                if (user != null)
                {
                    return new CommonResponse<int>(true, CommonConstants.UserIdRetrievedMessage, user.UserID);
                }
                else
                {
                    return new CommonResponse<int>(true, CommonConstants.UserNotFoundMessage, 0);
                }
            }
            catch (Exception ex)
            {
                _utilities.LogException(ex);
                return new CommonResponse<int>(false, CommonConstants.RetrievalErrorMessage, 0);
            }
        }
    }
}

