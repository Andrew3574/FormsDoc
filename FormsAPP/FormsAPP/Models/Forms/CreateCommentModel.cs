namespace FormsAPP.Models.Forms
{
    public class CreateCommentModel
    {
        public int UserId { get; set; }
        public int FormId { get; set; }
        public string Text { get; set; } = null!;
    }
}
