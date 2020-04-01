using BLL.Models;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents publisher details ViewModel for business logic and presentation layers
    /// </summary>
    public class PublisherDetailsViewModel
    {
        public Publisher Publisher { get; set; }
        public string UserId { get; set; }
    }
}