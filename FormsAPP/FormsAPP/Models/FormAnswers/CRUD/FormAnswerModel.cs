using FormsAPP.Models.Users;

namespace FormsAPP.Models.FormAnswers.CRUD
{
    public class FormAnswerModel
    {
        public FilterUserModel User { get; set; } = null!;
        public List<ShortTextAnswer> ShortTextAnswers { get; set; } = new List<ShortTextAnswer>();
        public List<LongTextAnswer> LongTextAnswers { get; set; } = new List<LongTextAnswer>();
        public List<IntegerAnswer> IntegerAnswers { get; set; } = new List<IntegerAnswer>();
        public List<CheckboxAnswer> CheckboxAnswers { get; set; } = new List<CheckboxAnswer>();
    }
}
