using System;
using UnityEditor;
using UnityEngine;

namespace Bose.Wearable.Editor.Inspectors
{
	[CustomPropertyDrawer(typeof(DynamicDeviceInfo))]
	internal sealed class DynamicDeviceInfoDrawer : PropertyDrawer
	{
		private const float TopPadding = 5f;
		private const string StatusValuesField = "deviceStatus._value";
		private const string AvailableAnrModesField = "availableActiveNoiseReductionModes";
		private const string CncLevelField = "controllableNoiseCancellationLevel";
		private const string CncEnabledField = "controllableNoiseCancellationEnabled";
		private const string TotalCncLevelsField = "totalControllableNoiseCancellationLevels";
		private const string CurrentAnrModeField = "activeNoiseReductionMode";

		private const string AvailableAnrModesHeading = "Available ANR Modes";
		private const string CurrentAnrModeLabel = "Current ANR Mode";
		private const string TotalCncLevelsLabelText = "Total CNC Levels";
		private const string CncLevelLabelText = "Current CNC Lavel";
		private const string DeviceStatusHeading = "Device Status";
		private const string AnrHeading = "Active Noise Reduction";
		private const string CncHeading = "Controllable Noise Cancellation";
		private const string CncEnabledLabel = "CNC Enabled";

		private GUIContent _totalCncLevelsLabel;

		public DynamicDeviceInfoDrawer()
		{
			_totalCncLevelsLabel = new GUIContent(TotalCncLevelsLabelText);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			EditorGUI.BeginProperty(position, label, property);
			Rect line = new Rect(
				position.x,
				position.y,
				position.width,
				WearableConstants.SingleLineHeight);

			// Device Status
			EditorGUI.LabelField(line, DeviceStatusHeading);
			line.y += WearableConstants.SingleLineHeight;

			Rect box = new Rect(
				line.x,
				line.y,
				line.width,
				line.height * (WearableConstants.DeviceStatusFlags.Length - 2)); // Flag count less "None" and "Suspended"
			GUI.Box(box, GUIContent.none);

			var statusValueProp = property.FindPropertyRelative(StatusValuesField);
			DeviceStatus status = statusValueProp.intValue;
			for (int i = 0; i < WearableConstants.DeviceStatusFlags.Length; i++)
			{
				DeviceStatusFlags flag = WearableConstants.DeviceStatusFlags[i];
				if (flag == DeviceStatusFlags.None ||
				    flag == DeviceStatusFlags.SensorServiceSuspended)
				{
					continue;
				}

				using (new EditorGUI.DisabledScope(flag == DeviceStatusFlags.SensorServiceSuspended))
				{
					bool value = EditorGUI.Toggle(
						line,
						flag.ToString(),
						status.GetFlagValue(flag));

					status.SetFlagValue(flag, value);
				}

				line.y += WearableConstants.SingleLineHeight;
			}
			statusValueProp.intValue = status;


			// Transmission period
			// No-op

			// ANR header
			line.y += WearableConstants.SingleLineHeight * 0.5f;
			EditorGUI.LabelField(line, AnrHeading);
			line.y += WearableConstants.SingleLineHeight;
			box = new Rect(
				line.x,
				line.y,
				line.width,
				WearableConstants.SingleLineHeight * (WearableConstants.ActiveNoiseReductionModes.Length + 1));
			GUI.Box(box, GUIContent.none);


			// ANR current mode (read-only)
			using (new EditorGUI.DisabledScope(true))
			{
				var anrModeProperty = property.FindPropertyRelative(CurrentAnrModeField);
				var anrMode = (ActiveNoiseReductionMode) anrModeProperty.intValue;
				EditorGUI.LabelField(line, CurrentAnrModeLabel, anrMode.ToString());
				line.y += WearableConstants.SingleLineHeight;
			}

			// ANR available modes
			EditorGUI.LabelField(line, AvailableAnrModesHeading);
			line.y += WearableConstants.SingleLineHeight;

			EditorGUI.indentLevel++;
			var availableAnrModesProperty = property.FindPropertyRelative(AvailableAnrModesField);
			int oldAnrModes = availableAnrModesProperty.intValue;
			int newAnrModes = 0;
			for (int i = 0; i < WearableConstants.ActiveNoiseReductionModes.Length; i++)
			{
				ActiveNoiseReductionMode mode = WearableConstants.ActiveNoiseReductionModes[i];

				if (mode == ActiveNoiseReductionMode.Invalid)
				{
					continue;
				}

				int flag = (1 << (int) mode);
				bool selected = EditorGUI.Toggle(line, mode.ToString(), (flag & oldAnrModes) != 0);
				line.y += WearableConstants.SingleLineHeight;
				if (selected)
				{
					newAnrModes |= flag;
				}
			}

			EditorGUI.indentLevel--;

			if (newAnrModes != oldAnrModes)
			{
				availableAnrModesProperty.intValue = newAnrModes;
			}

			// CNC header
			line.y += WearableConstants.SingleLineHeight * 0.5f;
			EditorGUI.LabelField(line, CncHeading);
			line.y += WearableConstants.SingleLineHeight;
			box = new Rect(
				line.x,
				line.y,
				line.width,
				WearableConstants.SingleLineHeight * 3);
			GUI.Box(box, GUIContent.none);

			using (new EditorGUI.DisabledScope(true))
			{
				// CNC Level (read-only)
				var cncLevelProperty = property.FindPropertyRelative(CncLevelField);
				EditorGUI.LabelField(line, CncLevelLabelText, cncLevelProperty.intValue.ToString());
				line.y += WearableConstants.SingleLineHeight;

				// CNC Enabled (read-only)
				var cncEnabledProperty = property.FindPropertyRelative(CncEnabledField);
				EditorGUI.Toggle(line, CncEnabledLabel, cncEnabledProperty.boolValue);
				line.y += WearableConstants.SingleLineHeight;
			}

			// Total CNC levels
			var totalCncLevelsProperty = property.FindPropertyRelative(TotalCncLevelsField);
			EditorGUI.PropertyField(line, totalCncLevelsProperty, _totalCncLevelsLabel);
			line.y += WearableConstants.SingleLineHeight;
			if (totalCncLevelsProperty.intValue < 0)
			{
				totalCncLevelsProperty.intValue = 0;
			}

			EditorGUI.EndProperty();
			property.serializedObject.ApplyModifiedProperties();
			EditorGUI.indentLevel = indent;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return WearableConstants.SingleLineHeight * (
				       WearableConstants.DeviceStatusFlags.Length - 2 + // Flag count less "None" and "Suspended"
				       WearableConstants.ActiveNoiseReductionModes.Length - 1 + // Mode count less "Invalid"
				       9) + // Device Status Header + ANR header + ANR mode + CNC header + CNC level + CNC enabled + 3x spacing
				       TopPadding;
		}
	}
}
