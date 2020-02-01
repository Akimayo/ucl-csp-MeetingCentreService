using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingCentreService.Models.Data
{
    public enum DocumentFormat
    {
        /// <summary>
        /// Application should use XML interface.
        /// </summary>
        XML,
        /// <summary>
        /// Application should use JSON interface.
        /// </summary>
        JSON,
        /// <summary>
        /// Application should use interface for legacy CSV-style files;
        /// </summary>
        CSVStyle
    }
}
