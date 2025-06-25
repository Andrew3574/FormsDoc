namespace FormsAPI.ModelsDTO.Forms
{
    public class CreateCommentDTO
    {
        public int UserId { get; set; }
        public int FormId { get; set; }
        public string Text { get; set; } = null!;

    }
}
