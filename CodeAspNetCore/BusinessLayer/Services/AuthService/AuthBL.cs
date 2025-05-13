using BussinessModels.Models.AuthModel;
using BussinessModels.Models.CommanResponse;
using BussinessModels.Models.Constants;
using DataAccessLayer.Repositories.AuthRepository;
using DataAccessLayer.Repositories.CommanRepository;

namespace BusinessLayer.Services.AuthService
{
    public class AuthBL
    {
        AuthRepo _context = new AuthRepo();
        CommanUtilities _utilities = new CommanUtilities();
        public CommonResponse<bool> IsEmailConfirmed(string IsEmail)
        { 
            CommonResponse<bool> response = _context.IsEmailConfirmed(IsEmail);
            return response;
        }

        public CommonResponse<bool> IsUserValid(LoginDTO request)
        {
            try
            {
                CommonResponse<string> result = _context.IsUserValid(request);
                string decryptedResult = _utilities.Decrypt(result.Data);

                if (string.IsNullOrEmpty(decryptedResult))
                {
                    return new CommonResponse<bool>(true, CommonConstants.UserNotValidMessage, false);
                }
                else if (decryptedResult == request.Password)
                {
                    return new CommonResponse<bool>(true, CommonConstants.UserValidMessage, true);
                }
                else
                {
                    return new CommonResponse<bool>(true, CommonConstants.UserNotValidMessage, false);
                }
            }
            catch
            {
                return new CommonResponse<bool>(false, CommonConstants.ValidationError, false);
            }
        }

        public CommonResponse<int> GetUserId(LoginDTO request)
        {
            try
            {
                CommonResponse<int> response = _context.GetUserId(request);

                if (response.Data == 0)
                {
                    return new CommonResponse<int>(true, CommonConstants.UserNotFoundMessage, 0);
                }
                else
                {
                    return new CommonResponse<int>(true, CommonConstants.UserIdRetrievedMessage, response.Data);
                }
            }
            catch
            {
                return new CommonResponse<int>(false, CommonConstants.RetrievalError, 0);
            }
        }

        public CommonResponse<bool> RegisterUser(RegisterDTO request)
        {
            try
            {
                // Encrypt the text
                request.Password = _utilities.encrypt(request.Password);

                CommonResponse<bool> result = _context.RegisterUser(request);

                if (result.Success == true)
                {
                    return new CommonResponse<bool>(true, CommonConstants.RegistrationSuccessMessage, true);
                }
                else
                {
                    return new CommonResponse<bool>(true, CommonConstants.RegistrationErrorMessage, false);
                }
            }
            catch
            {

                return new CommonResponse<bool>(false, CommonConstants.RegistrationErrorMessage, false);
            }
        }


    }

}
