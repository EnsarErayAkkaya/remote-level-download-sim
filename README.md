# remote-level-download-sim

- Clear PlayerPrefs and data from EEA Tools in the top bar.
- Enable or disable logs from ServiceManagerSettings.
- A potential improvement would be to write a custom parser for level data instead of using JsonUtility, or to use MessagePack by serializing all levels into binary data. MessagePack offers significantly better performance than the JsonUtility library, but for now I’ll leave it as is.
