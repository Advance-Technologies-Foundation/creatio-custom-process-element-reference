using ATF.Repository;
using ATF.Repository.Attributes;
using Newtonsoft.Json;
namespace UsrCustomProcessElement.IntegrationTests.ProcessModels{

	[BusinessProcess("UsrArithmetic_Multiply")]
	public class UsrArithmetic_Multiply : IBusinessProcess {

		/// <summary>
		/// Multiplicand
		/// </summary>
		[BusinessProcessParameter("MultiplicandParameter", BusinessProcessParameterDirection.Input)]
		public System.Single MultiplicandParameter{ get; set; }

		/// <summary>
		/// Multiplier
		/// </summary>
		[BusinessProcessParameter("MultiplierParameter", BusinessProcessParameterDirection.Input)]
		public System.Single MultiplierParameter{ get; set; }

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
