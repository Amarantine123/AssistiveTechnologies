
using System;

namespace NFine.Code
{
    /// <summary>
    /// operator  model
    /// </summary>
    public class OperatorModel
    {
        /// <summary>
        /// user id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// user code
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// user name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// user password
        /// </summary>
        public string UserPwd { get; set; }
        /// <summary>
        /// company id
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// department id
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// role id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// login ip address
        /// </summary>
        public string LoginIPAddress { get; set; }
        /// <summary>
        /// login ip address name
        /// </summary>
        public string LoginIPAddressName { get; set; }
        /// <summary>
        /// login token
        /// </summary>
        public string LoginToken { get; set; }
        /// <summary>
        /// login time
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// judge if system
        /// </summary>
        public bool IsSystem { get; set; }
    }
}
