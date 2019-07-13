namespace Bose.Wearable.Editor
{
	/// <summary>
	/// An internal helper class to contain constants for editor usage.
	/// </summary>
	internal static class WearableEditorConstants
	{
		// Compiler Override
		public static readonly string[] COMPILER_OVERRIDE_ARGUMENTS;

		// Files and folders
		public const string ASSETS_FOLDER_NAME = "Assets";
		public const string CSC_FILENAME = "csc.rsp";
		public const string MCS_FILENAME = "mcs.rsp";

		// Logs
		public const string DELETED_COMPILER_FILE_SUCCESS_FORMAT
			= "[Bose Wearable] Deleted compiler override file: [{0}].";

		public const string DELETED_UNUSED_COMPILER_FILE_SUCCESS_FORMAT
			= "[Bose Wearable] Deleted unused compiler override file: [{0}].";

		public const string CREATED_COMPILER_FILE_SUCCESS_FORMAT
			= "[Bose Wearable] Created compiler override file: [{0}].";

		public const string COMPILER_ARGUMENT_NOT_FOUND_FORMAT
			= "[Bose Wearable] Required compiler argument {0} not found in [{1}], please add this manually.";

		// Editor Prefs
		public const string USE_COMPILER_OVERRIDE_FILE_KEY = "bose_use_compiler_override_key";

		public const bool DEFAULT_AUTO_GENERATE_COMPILER_FILE_PREF = true;

		static WearableEditorConstants()
		{
			COMPILER_OVERRIDE_ARGUMENTS = new[]
			{
				"-unsafe"
			};
		}

		// UI
		public const string OK_BUTTON_LABEL = "OK";
		public const string CANCEL_BUTTON_LABEL = "Cancel";

		public const string COMPILER_OVERRIDE_FILE_DELETION_WARNING_TITLE = "Compiler Override File Exists";

		public const string COMPILER_OVERRIDE_FILE_DELETION_WARNING_FORMAT
			= "Regenerating the compiler override will delete the existing file [{0}], is it " +
			  "OK to proceed?";
	}
}
