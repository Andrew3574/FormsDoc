using FormsAPP.Models.Forms;

namespace FormsAPP.Models.FormAnswers
{
    public class CreateFormAnswer
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public int FormId { get; set; }

        public List<ShortTextAnswer> ShortTextAnswers { get; set; } = new List<ShortTextAnswer>();
        public List<LongTextAnswer> LongTextAnswers { get; set; } = new List<LongTextAnswer>();
        public List<IntegerAnswer> IntegerAnswers { get; set; } = new List<IntegerAnswer>();
        public List<CheckboxAnswer> CheckboxAnswers { get; set; } = new List<CheckboxAnswer>();

    }
}
