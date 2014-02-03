namespace CC.TheBench.Frontend.Web.Data.ReadModel
{
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class User
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string HashAndSalt { get; set; }

        [DataMember]
        public string DisplayName { get; set; }
    }
}