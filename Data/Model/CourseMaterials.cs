using CampusCollabFPI.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Model
{
    public class CourseMaterials
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }


        public GroupModel Group { get; set; }
        public int GroupId { get; set; }

        public string UploadedBy { get; set; }
    }
}
