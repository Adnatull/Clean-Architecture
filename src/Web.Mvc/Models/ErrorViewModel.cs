namespace Web.Mvc.Models
{
    /// <summary>
    /// Error View Model
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Show ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
