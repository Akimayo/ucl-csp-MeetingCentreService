using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingCentreService.Views.Forms
{
    /// <summary>
    /// Actions requested in forms
    /// </summary>
    public enum CloseAction
    {
        /// <summary>
        /// Indicates no action; cancellation or closing of the window
        /// </summary>
        None,
        /// <summary>
        /// Requests a save for the form
        /// </summary>
        Save,
        /// <summary>
        /// Requests the form's entity to be deleted
        /// </summary>
        Delete
    }
}
