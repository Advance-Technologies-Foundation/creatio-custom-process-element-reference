using ATF.Repository;
using ATF.Repository.Attributes;
using Newtonsoft.Json;
namespace UsrCustomProcessElement.IntegrationTests.ProcessModels{

	[BusinessProcess("UsrArithmetic_Add")]
	public class UsrArithmetic_Add : IBusinessProcess {

		/// <summary>
		/// FirstAddend
		/// </summary>
		[BusinessProcessParameter("FirstAddendParameter", BusinessProcessParameterDirection.Input)]
		public System.Single FirstAddendParameter{ get; set; }

		/// <summary>
		/// SecondAddend
		/// </summary>
		[BusinessProcessParameter("SecondAddendParameter", BusinessProcessParameterDirection.Input)]
		public System.Single SecondAddendParameter{ get; set; }

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
