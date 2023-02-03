namespace DemoApplication.Contracts.User
{
    public enum MockRole
    {
        Client = 1,
        VipClient = 2
    }

    public static class GetRoleName
    {
        public static string GetRoleNameByCode(this MockRole role)
        {
            switch (role)
            {
                case MockRole.Client:
                    return "Client";
                case MockRole.VipClient:
                    return "Vip Client";
                default:
                    throw new Exception("This role code not found");
            }
        }
    }

    
}
