using SAC_VALES.Common.Models;

namespace SAC_VALES.Web.Helpers
{
	public interface IMailHelper
	{
		Response SendMail(string to, string subject, string body);
	}
}
