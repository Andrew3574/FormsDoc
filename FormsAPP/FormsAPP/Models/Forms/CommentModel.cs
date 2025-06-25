namespace FormsAPP.Models.Forms
{
    public class CommentModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
    }
}
