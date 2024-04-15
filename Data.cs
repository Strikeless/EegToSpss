namespace EegToSpss
{
    /// <summary>
    /// A collection of all the EEG data to be collected.
    /// Essentially a program state/context.
    /// </summary>
    /// <param name="Timepoints"></param>
    /// <param name="ElectrodeNames"></param>
    /// <param name="SubjectDatas"></param>
    internal record class GlobalData(List<int> Timepoints, List<string> ElectrodeNames, List<SubjectData> SubjectDatas)
    {
        /// <summary>
        /// All the timestamps found in the data.
        /// </summary>
        public List<int> Timepoints { get; } = Timepoints;

        /// <summary>
        /// All the electrode names found in the data.
        /// </summary>
        public List<string> ElectrodeNames { get; } = ElectrodeNames;

        /// <summary>
        /// Data readings for every individual subject.
        /// </summary>
        public List<SubjectData> SubjectDatas { get; } = SubjectDatas;

        public GlobalData() : this([], [], []) { }
    }

    /// <summary>
    /// All the data for a single subject/file.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="ElectrodeReadings"></param>
    internal record class SubjectData(string Name, Dictionary<Datapoint, float> ElectrodeReadings)
    {
        /// <summary>
        /// The name/ID of the subject.
        /// </summary>
        public string Name { get; } = Name;

        /// <summary>
        /// All the readings from the subject associated with the reading's time and electrode.
        /// </summary>
        public Dictionary<Datapoint, float> ElectrodeReadings { get; } = ElectrodeReadings;

        public SubjectData(string name) : this(name, []) { }
    }

    /// <summary>
    /// Information about which measurement electrode and when a reading was captured.
    /// Useful as a key for mapping readings.
    /// </summary>
    /// <param name="Timepoint"></param>
    /// <param name="ElectrodeName"></param>
    internal record class Datapoint(int Timepoint, string ElectrodeName)
    {
        /// <summary>
        /// The timestamp at which the data was captured.
        /// </summary>
        public int Timepoint { get; } = Timepoint;

        /// <summary>
        /// The name of the measurement electrode that captured the data.
        /// </summary>
        public string ElectrodeName { get; } = ElectrodeName;
    }
}
