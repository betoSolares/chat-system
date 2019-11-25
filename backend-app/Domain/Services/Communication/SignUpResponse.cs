using backend_app.Domain.Models;

namespace backend_app.Domain.Services.Communication
{
    public class SignUpResponse : BaseResponse
    {
        public Account Account { get; private set; }

        private SignUpResponse(bool succes, string message, Account account, int statuscode) : base(succes, message, statuscode)
        {
            Account = account;
        }

        /// <summary>Create a success response</summary>
        /// <param name="account">The new account</param>
        public SignUpResponse(Account account) : this(true, string.Empty, account, 0) { }

        /// <summary>Create a failed response</summary>
        /// <param name="message">The error message</param>
        public SignUpResponse(string message, int statuscode) : this(false, message, null, statuscode) { }
    }
}
