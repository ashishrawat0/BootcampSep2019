using Microsoft.Extensions.Primitives;

namespace Examportal.Controllers
{
    internal class FileDetails
    {
        public StringSegment Filename { get; set; }
        public string Content { get; set; }
    }
}