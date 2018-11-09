namespace MvcDynamicForms.NetCore
{
    public static class RegexPatterns
    {
        private const string HTMLID = @"^[a-zA-Z][-_\da-zA-Z]*$";
        private const string HTMLINPUTNAME = @"^[a-zA-Z][-_:.\da-zA-Z]*$";
        private const string EMAILADDRESS = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        /// <summary>
        /// Matches a well-formed HTML element's id attribute value. 
        /// Naming rules: Must begin with a letter A-Z or a-z.  
        /// Can be followed by: letters (A-Za-z), digits (0-9), hyphens ("-"), and underscores ("_").
        /// </summary>
        public static string HtmlId
        {
            get { return HTMLID; }
        }

        /// <summary>
        /// Matches a well-formed HTML input element's name attribute value. 
        /// Naming rules: Must begin with a letter A-Z or a-z.  
        /// Can be followed by: letters (A-Za-z), digits (0-9), hyphens ("-"), and underscores ("_"), colons (":"), and periods (".").
        /// </summary>
        public static string HtmlInputName
        {
            get { return HTMLINPUTNAME; }
        }

        /// <summary>
        /// A practical regular expression used to validate email addresses.
        /// This will match the vast majority of email addresses in use.
        /// This is not RFC 2822 compliant.
        /// See http://www.regular-expressions.info/email.html for more information.
        /// </summary>
        public static string EmailAddress
        {
            get { return EMAILADDRESS; }
        }
    }
}