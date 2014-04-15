using System;
using System.Net.Configuration;


namespace SSW.Framework.Common
{
    public class SmtpZsValidationProvider: IZsValidateProvider
    {

        private readonly MailSettingsSectionGroup _mailSettings;

        public SmtpZsValidationProvider(MailSettingsSectionGroup mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public ZsValidateItem Validate()
        {
            var result = new SmtpZsValidateItem() { Title = "SMTP Settings" };


            if (_mailSettings == null)
            {
                result.Description = "Unable to read mail configuration";
                result.State = ZsValidateState.Fail;
                return result;
            }

            try
            {
                result.Server = _mailSettings.Smtp.Network.Host;
                result.Port = _mailSettings.Smtp.Network.Port;
                result.State = SmtpHelper.TestConnection(_mailSettings) ? ZsValidateState.Ok: ZsValidateState.Fail;
            }
            catch (Exception ex)
            {
                result.State = ZsValidateState.Fail;
                result.Description = ex.Message;
            }
            return result;
        }
    }


    public class SmtpZsValidateItem :  ZsValidateItem
    {
        public string Server { get; set; }
        public int Port { get; set; }
    }

}
