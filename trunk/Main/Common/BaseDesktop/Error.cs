using System.Runtime.Serialization;

namespace UtnEmall.Server.EntityModel
{
    /// <summary>
    /// Representa un error ocurrido en una propiedad.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error"), DataContract]
    public class Error
    {
        #region Constants, Variables and Properties

        private string name;
        private string property;
        private string description;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Property
        {
            get { return property; }
            set { property = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        #region Constructors

        public Error(string name, string property, string description)
        {
            this.name = name;
            this.property = property;
            this.description = description;
        }

        public Error()
        {

        }

        #endregion
    }
}
