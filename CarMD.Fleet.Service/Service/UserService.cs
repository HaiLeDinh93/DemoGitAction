using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Common;
using CarMD.Fleet.Common.Helper;
using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Data.Email;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Repository.IRepository;
using CarMD.Fleet.Service.IService;
using CarMD.Fleet.Service.Mandrill;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using CarMD.Fleet.Common.Helpers;

namespace CarMD.Fleet.Service.Service
{
    public class UserService : IUserService
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ServiceResult<bool> ResetPassword(string email)
        {
            try
            {
                var user = _userRepository.GetUser(email);

                if (user == null)
                {
                    return new ServiceResult<bool>
                    {
                        ResultCode = ResultCode.E002000,
                        Message = "Username not found."
                    };
                }

                if (!user.IsActive && user.LastDeactiveDate.HasValue)
                {
                    return new ServiceResult<bool>
                    {
                        ResultCode = ResultCode.E002003,
                        Message = "This account is not activated."
                    };
                }

                var tempPassword = RandomStringUtility.Generate(8, false, false);
                user.TempPassword = tempPassword;
                var sendMail = false;
                var message = "";
                if (!user.IsActive)
                {
                    message = "Please activate your account first. An activation email has been sent.";
                    sendMail = SendRegistrationEmail(user);
                }
                else
                {
                    message = string.Format("The user's password has been reset to a temporary password. Email with new temporary password has been sent to email: {0}. The user will have to login and change password", user.Email);
                    sendMail = SendResetPasswordEmail(user);
                }

                if (sendMail)
                {
                    _userRepository.Update(user);

                    return new ServiceResult<bool>
                    {
                        ResultCode = ResultCode.Ok,
                        Message = message,
                        Data = true
                    };
                }
                else
                {
                    return new ServiceResult<bool>
                    {
                        ResultCode = ResultCode.E002004,
                        Message = "Cannot send Reset Password Email."
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[Reset Password] Rest password for user :- {0} - error : {1}", email, LogUtility.GetDetailsErrorMessage(ex));
            }

            return new ServiceResult<bool>
            {
                ResultCode = ResultCode.E002004,
                Message = "There is an error. Please contact to admin for support."
            };
        }

        private bool SendRegistrationEmail(User user)
        {
            var ticket = TicketHelper.Create(new TicketModel
            {
                Email = user.Email,
                Password = user.TempPassword,
                DateTime = DateTime.UtcNow,
                interval = CommonConfiguration.TicketInterval
            });

            var registrationEmailModel = new RegistrationEmailModel
            {
                UserEmail = user.Email,
                Emails = new List<string> { user.Email },
                Subject = CommonConfiguration.RegistrationEmailSubject,
                EmailTemplateImgPath = UrlHelper.Merge(CommonConfiguration.ShellWebRootUrl, CommonConfiguration.EmailTemplateImgPath),
                ResetLogintUrl = UrlHelper.Merge(CommonConfiguration.ShellWebRootUrl, string.Format("Home/ResetLogin?ticket={0}&isNewUser=true",  HttpUtility.UrlEncode(ticket)))
            };
            var sendMail = SendMandrillEmailHelper.SendRegistrationEmail(registrationEmailModel);

            return sendMail;
        }

        private bool SendResetPasswordEmail(User user)
        {
            var ticket = TicketHelper.Create(new TicketModel
            {
                Email = user.Email,
                Password = user.TempPassword,
                DateTime = DateTime.UtcNow,
                interval = CommonConfiguration.TicketInterval
            });

            var emailModel = new ResetPasswordEmailModel
            {
                UserEmail = user.Email,
                Emails = new List<string> { user.Email },
                Subject = CommonConfiguration.ResetPasswordEmailSubject,
                EmailTemplateImgPath = UrlHelper.Merge(CommonConfiguration.ShellWebRootUrl, CommonConfiguration.EmailTemplateImgPath),
                ResetLogintUrl = UrlHelper.Merge(CommonConfiguration.ShellWebRootUrl, string.Format("Home/ResetLogin?ticket={0}",HttpUtility.UrlEncode(ticket)))
            };
            var sendMail = SendMandrillEmailHelper.SendResetPasswordEmail(emailModel);

            return sendMail;
        }
    }
}
