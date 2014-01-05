﻿namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Nancy;

    public class RegisterModule : NancyModule
    {
        public RegisterModule() : base("/account/register")
        {
            Get["/"] = x =>
            {
                return View["account/register"];
            };

            Post["/"] = _ => "register";
        }
    }
}