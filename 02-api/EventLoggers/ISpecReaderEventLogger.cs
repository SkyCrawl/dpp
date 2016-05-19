namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface that defines events for specification reading.
    /// </summary>
    public interface ISpecReaderEventLogger
    {
        /// <summary>
        /// A new specification reading task has commenced.
        /// </summary>
        /// <param name="specOrigin">Origin of the newly parsed specification.</param>
        void NewSpecification(string specOrigin);
    }
}
