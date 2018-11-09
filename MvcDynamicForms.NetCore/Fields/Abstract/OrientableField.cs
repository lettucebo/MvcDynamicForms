using System;
using MvcDynamicForms.NetCore.Enums;

namespace MvcDynamicForms.NetCore.Fields.Abstract
{
    /// <summary>
    /// Represents and input field that can be displayed either vertically or horizontally.
    /// </summary>
    [Serializable]
    public abstract class OrientableField : ListField
    {
        protected string _inputLabelClass = "MvcDynamicListFieldInputLabel";
        protected string _verticalClass = "MvcDynamicVertical";
        protected string _horizontalClass = "MvcDynamicHorizontal";
        protected string _listClass = "MvcDynamicOrientableList";
        protected Orientation _orientation = Orientation.Vertical;

        /// <summary>
        /// The direction that the choices will be displayed.
        /// </summary>
        public Orientation Orientation
        {
            get { return this._orientation; }
            set { this._orientation = value; }
        }

        /// <summary>
        /// The class attribute of the label element used to display the user's choice.
        /// </summary>
        public string InputLabelClass
        {
            get { return this._inputLabelClass; }
            set { this._inputLabelClass = value; }
        }

        /// <summary>
        /// The class attribute that is added to the ul element when using vertical orientation.
        /// </summary>
        public string VerticalClass
        {
            get { return this._verticalClass; }
            set { this._verticalClass = value; }
        }

        /// <summary>
        /// The class attribute that is added to the ul element when using horizontal orientation.
        /// </summary>
        public string HorizontalClass
        {
            get { return this._horizontalClass; }
            set { this._horizontalClass = value; }
        }

        /// <summary>
        /// The class attribute that is added to the ul element containing the choices.
        /// </summary>
        public string ListClass
        {
            get { return this._listClass; }
            set { this._listClass = value; }
        }
    }
}