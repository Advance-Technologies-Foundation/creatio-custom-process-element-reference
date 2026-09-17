using ATF.Repository;
using ATF.Repository.Attributes;
using Newtonsoft.Json;
namespace UsrCustomProcessElement.IntegrationTests.ProcessModels{

	[BusinessProcess("UsrArithmetic_Subtract")]
	public class UsrArithmetic_Subtract : IBusinessProcess {

		/// <summary>
		/// Minuend
		/// </summary>
		[BusinessProcessParameter("MinuendParameter", BusinessProcessParameterDirection.Input)]
		public System.Single MinuendParameter{ get; set; }

		/// <summary>
		/// Subtrahend
		/// </summary>
		[BusinessProcessParameter("SubtrahendParameter", BusinessProcessParameterDirection.Input)]
		public System.Single SubtrahendParameter{ get; set; }

		/// <summary>
		/// Result
		/// </summary>
		[BusinessProcessParameter("ResultParameter", BusinessProcessParameterDirection.Output)]
		public System.Single ResultParameter{ get; set; }

		/// <summary>
		/// IsError
		/// </summary>
		[BusinessProcessParameter("IsErrorParameter", BusinessProcessParameterDirection.Output)]
		public System.Boolean IsErrorParameter{ get; set; }

		/// <summary>
		/// ErrorMessage
		/// </summary>
		[BusinessProcessParameter("ErrorMessageParameter", BusinessProcessParameterDirection.Output)]
		public System.String ErrorMessageParameter{ get; set; }


	}

}
