using ATF.Repository;
using ATF.Repository.Attributes;
using Newtonsoft.Json;
namespace UsrCustomProcessElement.IntegrationTests.ProcessModels{

	[BusinessProcess("UsrArithmetic_Divide")]
	public class UsrArithmetic_Divide : IBusinessProcess {

		/// <summary>
		/// Dividend
		/// </summary>
		[BusinessProcessParameter("DividendParameter", BusinessProcessParameterDirection.Input)]
		public System.Single DividendParameter{ get; set; }

		/// <summary>
		/// Divisor
		/// </summary>
		[BusinessProcessParameter("DivisorParameter", BusinessProcessParameterDirection.Input)]
		public System.Single DivisorParameter{ get; set; }

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
