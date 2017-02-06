using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;


namespace dcUserQuery
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DcUserQuery" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DcUserQuery.svc or DcUserQuery.svc.cs at the Solution Explorer and start debugging.
    public class DcUserQuery : IDcUserQuery
    {
        public string GetUserGroups(string serverAddress, string domain, string userName, string password)
        {
            string result = "";
            try
            {
                //DirectoryEntry entry = new DirectoryEntry(serverAddress, userName, password);
                //DirectorySearcher searcher = new DirectorySearcher(entry.);

                PrincipalSearchResult<Principal> groups = UserPrincipal.Current.GetGroups();
                var displayName = UserPrincipal.Current.DisplayName;
                var emailAddress = UserPrincipal.Current.EmailAddress;
                var authGroups = UserPrincipal.Current.GetAuthorizationGroups();
                var ab = UserPrincipal.Current.GetGroups();

                IEnumerable<string> groupNames = groups.Select(x => x.SamAccountName);
                foreach (var groupName in groupNames)
                {
                    result += "|" + groupName;
                }
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        
        public List<string> GetUserDepartments(string serverAddress, string domain, string userName, string password)
        {
            var departments = new List<string>();
            //=================================================

            string department = string.Empty;

            // if you do repeated domain access, you might want to do this *once* outside this method, 
            // and pass it in as a second parameter!
            var userDomain = new PrincipalContext(ContextType.Domain);

            // find the user
            UserPrincipal user = UserPrincipal.FindByIdentity(userDomain, userName);

            // if user is found
            if (user != null)
            {
                // get DirectoryEntry underlying it
                DirectoryEntry de = (user.GetUnderlyingObject() as DirectoryEntry);

                if (de != null)
                {
                    if (de.Properties.Contains("department"))
                    {
                        department = de.Properties["department"][0].ToString();
                    }
                }
            }
            departments.Add(department);
            return departments;

            //================================================
            return departments;
        }

        public string LoginUser(string ldapAddress, string domain, string userName, string password)
        {
            
            string result = string.Empty;
            string authMessage = string.Empty;

            if (Authenticate(ldapAddress, domain, userName, password, out authMessage))
            {
                result = "OK";
            }
            else
            {
                result = authMessage;
            }

            return result;
        }


        /// <summary>
        /// Will attempt to authenticate the user against the AD directory
        /// </summary>
        /// <param name="address"></param>
        /// <param name="domain"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <param name="authMsg"></param>
        /// <returns></returns>
        private bool Authenticate(string address, string domain, string userName, string userPassword, out string authMsg)
        {
            bool authentic = false;
            authMsg = string.Empty;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(address, userName, userPassword);
                object nativeObject = entry.NativeObject;
                authentic = true;
                //authMsg = entry.
            }
            catch (DirectoryServicesCOMException ex)
            {
                authMsg = ex.Message;
            }
            return authentic;
        }

        //private bool Authenticate(string address, string domain, string userName, string password)
        //{
        //    bool authentic = false;
        //    try
        //    {
        //        DirectoryEntry entry = new DirectoryEntry("LDAP://" + address, userName, password);
        //        object nativeObject = entry.NativeObject;
        //        authentic = true;
        //    }
        //    catch (DirectoryServicesCOMException) { }
        //    return authentic;
        //}


        //private bool Authenticate(string address, string domain, string userName, string password)
        //{
        //    bool authentic = false;
        //    try
        //    {
        //        string ldapStr = string.Empty;
        //        ldapStr += "LDAP://" + ConfigurationManager.AppSettings["authServerAddress"] + "/";
        //        ldapStr += "CN=Users";
        //        ldapStr += ",DC=TST,DC=test";

        //        DirectoryEntry entry = new DirectoryEntry(ldapStr, userName, password);
        //        //DirectorySearcher searcher = new DirectorySearcher(entry);
        //        //var results = searcher.FindAll();

        //        object nativeObject = entry.NativeObject;
        //        authentic = true;
        //    }
        //    catch (DirectoryServicesCOMException) { }
        //    return authentic;
        //}

        private string GetLdapConnectionPath()
        {
            return "";
        }
        /*
        /// <summary>
        /// from stack overflow
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<GroupPrincipal> GetGroups(string userName)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();

            // establish domain context
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain);

            // find your user
            UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, userName);

            // if found - grab its groups
            if (user != null)
            {
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

                // iterate over all groups
                foreach (Principal p in groups)
                {
                    // make sure to add only group principals
                    if (p is GroupPrincipal)
                    {
                        result.Add((GroupPrincipal)p);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// from stack overflow
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public string GetDepartment(Principal principal)
        {
            string result = string.Empty;

            DirectoryEntry de = (principal.GetUnderlyingObject() as DirectoryEntry);

            if (de != null)
            {
                if (de.Properties.Contains("department"))
                {
                    result = de.Properties["department"][0].ToString();
                }
            }

            return result;
        }
        */
        //public string GetDepartment_v2(string username)
        //{
        //    string result = string.Empty;
            
        //    // if you do repeated domain access, you might want to do this *once* outside this method, 
        //    // and pass it in as a second parameter!
        //    var userDomain = new PrincipalContext(ContextType.Domain);

        //    // find the user
        //    UserPrincipal user = UserPrincipal.FindByIdentity(userDomain, username);

        //    // if user is found
        //    if (user != null)
        //    {
        //        // get DirectoryEntry underlying it
        //        DirectoryEntry de = (user.GetUnderlyingObject() as DirectoryEntry);

        //        if (de != null)
        //        {
        //            if (de.Properties.Contains("department"))
        //            {
        //                result = de.Properties["department"][0].ToString();
        //            }
        //        }
        //    }

        //    return result;
        //}


        //public ArrayList Groups()
        //{
        //    ArrayList groups = new ArrayList();
        //    foreach (System.Security.Principal.IdentityReference group in
        //        System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
        //    {
        //        groups.Add(group.Translate(typeof
        //            (System.Security.Principal.NTAccount)).ToString());
        //    }
        //    return groups;
        //}


        public ArrayList AttributeValuesMultiString(string attributeName, string objectDn, ArrayList valuesCollection, bool recursive)
        {
            DirectoryEntry ent = new DirectoryEntry(objectDn);
            PropertyValueCollection ValueCollection = ent.Properties[attributeName];
            IEnumerator en = ValueCollection.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current != null)
                {
                    if (!valuesCollection.Contains(en.Current.ToString()))
                    {
                        valuesCollection.Add(en.Current.ToString());
                        if (recursive)
                        {
                            AttributeValuesMultiString(attributeName, "LDAP://" +
                            en.Current.ToString(), valuesCollection, true);
                        }
                    }
                }
            }
            ent.Close();
            ent.Dispose();
            return valuesCollection;
        }

        #region Public Methods
        ///Contains Public methods to access AD.
        /// Futher description goes here
        /// and here


        public DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry directoryEntry;
            directoryEntry = new DirectoryEntry("LDAP://raven", "mikey", "HotSaoop1", AuthenticationTypes.Secure);
            return directoryEntry;
        }

        public DirectoryEntry GetUser(string userName)
        {
            DirectoryEntry directoryEntry = GetDirectoryObject();
            DirectorySearcher searcher = new DirectorySearcher();
            searcher.SearchRoot = directoryEntry;

            searcher.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
            searcher.SearchScope = SearchScope.Subtree;
            SearchResult results = searcher.FindOne();

            if (results != null)
            {
                directoryEntry = new DirectoryEntry(results.Path, "mikey", "HotSoop1", AuthenticationTypes.Secure);
                return directoryEntry;
            }
            else
            {
                return null;
            }
        }

        #endregion


    }
}
