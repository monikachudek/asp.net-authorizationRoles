# asp.net-authorizationRoles

ASP.NET Core web app with user data protected by authorization. Based on [microsoft docs](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-3.1).

Roles and privileges:
* **Registered users** can view all the approved data and can edit/delete their own data.
* **Managers** can view, approve or reject all contact data but they can edit just their own data.
* **Administrators** can approve/reject and edit/delete any data.

---

The Authorization project has been modyfied and extended with functionalities from tutorial [Razor Pages with Entity Framework Core](https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-3.0&tabs=visual-studio).

However, the authorization applies only to Student data.

