namespace CC.TheBench.Frontend.Web.Security
{
    public static class Constants
    {
        public static readonly string TheBenchAuthType = "TheBench";

        /*
        PinpointTownes Personally, I would not return a null principal, but an anonymous one.
        PinpointTownes If you don't want using an external library, you can also use new ClaimsPrincipal(new ClaimsIdentity()) and store it in a static property/field.
        CumpsD then i need to remember to check for that, instead of nul
        PinpointTownes Did you know that IIdentity had an IsAuthenticated property? 
        PinpointTownes IPrincipal -> Identity -> IsAuthenticated...
        CumpsD how does it determine true/false?
        PinpointTownes Depends of the IIdentity implementation, but the default one checks AuthenticationType's value.
                       If null or empty, it returns false.
        */
        //public static readonly TheBenchPrincipal AnonymousPrincipal = new TheBenchPrincipal(new ClaimsIdentity());
    }
}