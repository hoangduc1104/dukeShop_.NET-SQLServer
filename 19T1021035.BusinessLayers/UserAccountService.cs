using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _19T1021035.DomainModels;
using _19T1021035.DataLayers;
using _19T1021035.DataLayers.SQLServer;
using System.Web;
using System.Configuration;



namespace _19T1021035.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL EmployeeAccountDb;
        private static readonly IUserAccountDAL CustomerAccountDb;

        /// <summary>
        ///     Constructor of UserAccountService
        /// </summary>
        static UserAccountService()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            EmployeeAccountDb = new EmployeeAccountDAL(connectionString);
            CustomerAccountDb = new CustomerAccountDAL(connectionString);
        }

        /// <summary>
        ///     Check username and password to authenticate
        /// </summary>
        /// <param name="type"> Account type </param>
        /// <param name="userName"> Username </param>
        /// <param name="password"> Password </param>
        /// <returns> UserAccount object if authentication is successful, otherwise null </returns>
        public static UserAccount Authenticate(AccountTypes type, string userName, string password)
        {
            return type == AccountTypes.Employee ? EmployeeAccountDb.Authenticate(userName, password) : CustomerAccountDb.Authenticate(userName, password);
        }

        /// <summary>
        ///     Change password of user
        /// </summary>
        /// <param name="type"> Account type </param>
        /// <param name="userName"> Username </param>
        /// <param name="oldPassword"> Old password </param>
        /// <param name="newPassword"> New password </param>
        /// <returns> True if change password is successful, otherwise false </returns>
        public static bool ChangePassword(AccountTypes type, string userName, string oldPassword, string newPassword)
        {
            return type == AccountTypes.Employee ? EmployeeAccountDb.ChangePassword(userName, oldPassword, newPassword) : CustomerAccountDb.ChangePassword(userName, oldPassword, newPassword);
        }
    }
}
