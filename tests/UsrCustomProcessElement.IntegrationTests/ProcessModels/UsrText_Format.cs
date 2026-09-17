using ATF.Repository;
using ATF.Repository.Attributes;
using Newtonsoft.Json;
namespace UsrCustomProcessElement.IntegrationTests.ProcessModels{

	[BusinessProcess("UsrText_Format")]
	public class UsrText_Format : IBusinessProcess {

		/// <summary>
		/// Input text
		/// </summary>
		[BusinessProcessParameter("InputTextParameter", BusinessProcessParameterDirection.Input)]
		public System.String InputTextParameter{ get; set; }

		/// <summary>
		/// Formatted text
		/// </summary>
		[BusinessProcessParameter("FormattedTextParameter", BusinessProcessParameterDirection.Output)]
		public System.String FormattedTextParameter{ get; set; }

		/// <summary>
		/// Is error
		/// </summary>
		[BusinessProcessParameter("IsErrorParameter", BusinessProcessParameterDirection.Output)]
		public System.Boolean IsErrorParameter{ get; set; }

		/// <summary>
		/// Error message
		/// </summary>
		[BusinessProcessParameter("ErrorMessageParameter", BusinessProcessParameterDirection.Output)]
		public System.String ErrorMessageParameter{ get; set; }

		/// <summary>
		/// Prefix
		/// </summary>
		[BusinessProcessParameter("PrefixParameter", BusinessProcessParameterDirection.Input)]
		public System.String PrefixParameter{ get; set; }


	}

}
