namespace fields.request.passwordrecovery
{
    public class PasswordRecoveryRequest
    {
        public string Email { get; set; }
        
        public PasswordRecoveryRequest(string email)
        {
            Email = email;
        }
    }
}