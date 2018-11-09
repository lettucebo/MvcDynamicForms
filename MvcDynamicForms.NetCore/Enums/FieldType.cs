namespace MvcDynamicForms.NetCore.Enums
{
    /// <summary>
    /// Possible types of Field objects. This enumeration can be used when constructing Field objects from external data.
    /// </summary>
    public enum FieldType
    {
        TextBox = 1,
        TextArea = 2,
        CheckBox = 3,
        CheckBoxList = 4,
        RadioButtonList = 5,
        Select = 6,
        Literal = 7,
        FileUpload = 8
    }
}