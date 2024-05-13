namespace MPT.Vending.Domains.Identity.Abstractions
{
    public class IdentityData
    {
        public const string AdminUserClaimName = "admin";
        public const string AdminUserPolicyName = "Admin";

        public const string KioskClaimName = "kiosk";
        public const string KioskUserPolicyName = "Kiosk";

        public const string ManagerClaimName = "manager";
        public const string ManagerPolicyName = "Manager";

        public const string InsiderClaimName = "insider";
        public const string InsiderPolicyName = "Insider";
    }
}
