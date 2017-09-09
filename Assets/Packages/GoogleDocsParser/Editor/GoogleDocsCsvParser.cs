using System;
using System.Collections;
using System.IO;
using System.Threading;
using CsvHelper;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using csv;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

public class GoogleDocsCsvParser
{
    private string[] _fieldNames;
    private Dictionary<string, Values> _loadedObjects = new Dictionary<string, Values>();
    private string _type;
    private static Type[] unityTypes = new[] { typeof(Component), typeof(GameObject), typeof(ScriptableObject) };


    public void Load(string url, string type, CsvParseMode mode, string postfix)
    {
        EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", 0f);
        Debug.Log("Loading csv from: " + url);

        _type = type;

        var www = new WWW(url);
        while (!www.isDone)
        {
            EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", www.progress);

            Thread.Sleep(100);
        }
//
//        ContinuationManager.Add(() =>
//            {
//                EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", www.progress);
//                return www.isDone;
//            },
//            () =>
//            {
        EditorUtility.ClearProgressBar();

        // Let's parse this CSV!
        TextReader sr = new StringReader(www.text);
        try
        {
            if (mode == CsvParseMode.ObjectPerRow)
            {
                ParseObjectPerRow(sr, type, postfix);
            }
            else
            {
                ParseObjectPerTable(sr, type, postfix);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
//            });

        www.Dispose();
    }

    [Conditional("UNITY_EDITOR")]
    public void GenerateInfoFiles()
    {
        AssetDatabase.StartAssetEditing();

        foreach (var each in _loadedObjects)
        {
            var instance = GetOrCreate(_type, each.Key);

            if (instance == null)
            {
                continue;
            }

            var csvConfigurable = instance as ICsvConfigurable;

            ParseObjectFieldsAndProperties(csvConfigurable, each.Value);
            ProcessObject(csvConfigurable, each.Value);

            Debug.Log($"Data object {instance.name} saved to {AssetDatabase.GetAssetPath(instance)}", instance);
        }

        AssetDatabase.StopAssetEditing();
    }

    public void GenerateInfoClassFiles()
    {
        var fieldTypes = new List<Type>();
        foreach (var each in _fieldNames)
        {
            var firstNonNullValue = _loadedObjects.Values
                .Select(_ => _.Get<string>(each))
                .FirstOrDefault(_ => !string.IsNullOrEmpty(_));

            var fieldType = ObjectInfoCodeGenerator.DeduceType(firstNonNullValue);
            fieldTypes.Add(fieldType);
        }

        var path = Application.dataPath + "/Scripts/Info/" + _type + ".cs";
        ObjectInfoCodeGenerator.Generate(path, _type, _fieldNames, fieldTypes);
    }

    private void ParseObjectPerRow(TextReader csvReader, string type, string postfix)
    {
        var parser = new CsvParser(csvReader);
        var row = parser.Read(); // get first row and

        //Here we need to read spreadsheet name and use it for type
//        if (string.IsNullOrEmpty(type))
//        {
//            // Read Type info
//            if (row[0] == "type")
//            {
//                row = parser.Read();
//            }
//            else
//            {
//                Debug.LogError("Worksheet must declare 'Type' in first wor");
//                return;
//            }
//        }

        _fieldNames = row;

        row = parser.Read();

        while (row != null)
        {
            if (row.Length < 2 || string.IsNullOrEmpty(row[0]))
            {
                row = parser.Read();
                continue;
            }

            var instanceName = csv.Utility.FixName(row[0], postfix);
            _loadedObjects.Add(instanceName, CreateValues(_fieldNames, row, startingIndex: 0));

            row = parser.Read();
        }
    }

    private void ParseObjectPerTable(TextReader csvReader, string type = null, string postfix = null)
    {
        var parser = new CsvParser(csvReader);

        var fieldNames = new List<string>();
        var fieldValues = new List<string>();

        var row = parser.Read(); // get first row and
        // Read fields
        while (row != null) // && row[0] != "ID")
        {
            fieldNames.Add(row[0]);
            fieldValues.Add(row[1]);

            row = parser.Read();
        }

        var instanceName = csv.Utility.FixName(type, postfix);
        _loadedObjects.Add(instanceName, CreateValues(fieldNames, fieldValues, startingIndex: 0));

        _fieldNames = fieldNames.ToArray();
    }

    private static Values CreateValues(IList<string> fieldNames, IList<string> fieldValues, int startingIndex)
    {
        var valuesDictionary = new Dictionary<string, string>();

        for (var i = startingIndex; i < fieldValues.Count; i++)
        {
            var fieldName = fieldNames[i].TrimEnd(' ').ToLower();
            if (valuesDictionary.ContainsKey(fieldName))
            {
                Debug.LogFormat("They key is duplicate: {0}:{1}", fieldName, fieldValues[i]);
                continue;
            }

            var lowerRow = fieldValues[i].ToLower();
            if (lowerRow == "yes" || lowerRow == "y") fieldValues[i] = "true";
            if (lowerRow == "no" || lowerRow == "n") fieldValues[i] = "false";

            valuesDictionary[fieldName] = fieldValues[i];
        }

        return new Values(valuesDictionary);
    }

    private static ScriptableObject GetOrCreate(string typeName, string instanceName)
    {
        var assembly = Assembly.GetAssembly(typeof(ICsvConfigurable));
        var type = assembly
            .GetExportedTypes()
            .FirstOrDefault(_ => _.FullName.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));

        if (type == null)
        {
            Debug.LogWarningFormat("Type {0} not found", typeName);
            return null;
        }

        var assetPath = Path.Combine("Assets/Data/Remote Data/", type.Name);
        var assetPathWithName = assetPath + "/" + instanceName + ".asset";

        var instance = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathWithName);

        if (instance == null)
        {
            instance = ScriptableObject.CreateInstance(type);
            Directory.CreateDirectory(assetPath).Attributes = FileAttributes.Normal;
            AssetDatabase.CreateAsset(instance, assetPathWithName);
        }

        EditorUtility.SetDirty(instance);

        return instance;
    }

    private static void ProcessObject(ICsvConfigurable target, Values values)
    {
        ParseObjectFieldsAndProperties(target, values);
        target.Configure(values);
    }

    private static void ParseObjectFieldsAndProperties(ICsvConfigurable target, Values values)
    {
        var type = target.GetType();

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

        foreach (var each in fields)
        {
            ParseObjectField(target, each, values);
        }
    }

    private static bool ParseListObjectField(ICsvConfigurable target,string name, FieldInfo fieldInfo, Values values)
    {
        var fieldType = fieldInfo.FieldType;
        if (!fieldType.IsGenericType || (fieldType.GetGenericTypeDefinition() != typeof(List<>)))
            return false;
        var genericType = fieldType.GetGenericArguments().FirstOrDefault();
        var isAssetGenericType = unityTypes.Any(x => x.IsAssignableFrom(genericType));
        if (!isAssetGenericType)
            return false;
        var list = Activator.CreateInstance(fieldType) as IList;
        var assets = values.GetScriptableObjects(genericType, name);
        if (assets == null || list==null)
            return false;
        foreach (var asset in assets)
        {
            if(asset)
                list.Add(asset);
        }
        fieldInfo.SetValue(target, list);
        return true;
    }

    private static string GetPropertyName(FieldInfo fieldInfo)
    {
        var attribute = fieldInfo
            .GetCustomAttributes(typeof(RemotePropertyAttribute), inherit: false)
            .OfType<RemotePropertyAttribute>()
            .FirstOrDefault();
        if (attribute == null)
            return null;
        var propertyName = string.IsNullOrEmpty(attribute.PropertyName) ? fieldInfo.Name : attribute.PropertyName;
        propertyName = propertyName.ToLower();
        return propertyName;
    }

    private static void ParseObjectField(ICsvConfigurable target, FieldInfo fieldInfo, Values values)
    {
        var propertyName = GetPropertyName(fieldInfo);
        if (propertyName != null)
        {
            var fieldType = fieldInfo.FieldType;
            if (ParseListObjectField(target, propertyName, fieldInfo, values))
            {
                return;
            }
            if (unityTypes.Any(_ => fieldType.IsSubclassOf(_)))
            {
                LoadUnityTypeValue(target, fieldInfo, values, propertyName);
            }
            else
            {
                LoadSimpleValue(target, fieldInfo, values, propertyName);
            }
        }
    }

    private static void LoadSimpleValue(ICsvConfigurable target, FieldInfo fieldInfo, Values values,
        string propertyName)
    {
        var value = values.Get(propertyName, string.Empty);

        var fieldValue = default(object);
        var type = fieldInfo.FieldType;

        if (value == string.Empty && type.IsValueType)
        {
            fieldValue = Activator.CreateInstance(type);
        }
        else if (type.IsEnum)
        {
            fieldValue = Enum.Parse(type, value, true);
        }
        else if (fieldInfo.FieldType.IsEnum)
        {
            fieldValue = Enum.Parse(fieldInfo.FieldType, value);
        }
        else
        {
            fieldValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        fieldInfo.SetValue(target, fieldValue);
    }

    private static void LoadUnityTypeValue(ICsvConfigurable target, FieldInfo fieldInfo, Values values,
        string propertyName)
    {
        var unityObjectInstance = values.FindUnityAsset(propertyName, fieldInfo.FieldType);
        fieldInfo.SetValue(target, unityObjectInstance);
    }
}