using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain  { get; set; }
        public string PublicId { get; set; }
        
        public int AppUserId { get; set; }
        // Cần thêm thuộc tính AppUser nhằm mục đích ví dụ truy vấn tên của chủ hình ảnh: p=> p.AppUser.Name (full ef relationship)
        public AppUser AppUser { get; set; }
    }
}