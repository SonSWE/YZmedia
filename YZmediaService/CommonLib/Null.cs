using System;
using System.Reflection;

namespace CommonLib
{
    public class Null
    {

        // define application encoded null values
        public static int NullInteger
        {
            get { return -1; }
        }
        public static System.DateTime NullDate
        {
            get { return System.DateTime.MinValue; }
        }
        public static string NullString
        {
            get { return ""; }
        }
        public static bool NullBoolean 
        {
            get { return false; }
        }
        public static Guid NullGuid
        {
            get { return Guid.Empty; }
        }

        // sets a field to an application encoded null value ( used in Presentation layer )
        public static object SetNull(object objField)
        {
            object functionReturnValue = null;
            if ((objField != null))
            {
                if (objField is int)
                {
                    functionReturnValue = NullInteger;
                }
                else if (objField is float)
                {
                    functionReturnValue = NullInteger;
                }
                else if (objField is double)
                {
                    functionReturnValue = NullInteger;
                }
                else if (objField is decimal)
                {
                    functionReturnValue = NullInteger;
                }
                else if (objField is System.DateTime)
                {
                    functionReturnValue = NullDate;
                }
                else if (objField is string)
                {
                    functionReturnValue = NullString;
                }
                else if (objField is bool)
                {
                    functionReturnValue = NullBoolean;
                }
                else if (objField is Guid)
                {
                    functionReturnValue = NullGuid;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            else
            {
                // assume string
                functionReturnValue = NullString;
            }
            return functionReturnValue;
        }

        public static object SetNull(Type objType)
        {
            object functionReturnValue = null;
            if ((objType != null))
            {
                if (objType == typeof(int))
                {
                    functionReturnValue = NullInteger;
                }
                else if (objType == typeof(float))
                {
                    functionReturnValue = NullInteger;
                }
                else if (objType == typeof(double))
                {
                    functionReturnValue = NullInteger;
                }
                else if (objType == typeof(decimal))
                {
                    functionReturnValue = NullInteger;
                }
                else if (objType == typeof(System.DateTime))
                {
                    functionReturnValue = NullDate;
                }
                else if (objType == typeof(string))
                {
                    functionReturnValue = NullString;
                }
                else if (objType == typeof(bool))
                {
                    functionReturnValue = NullBoolean;
                }
                else if (objType == typeof(Guid))
                {
                    functionReturnValue = NullGuid;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            else
            {
                // assume string
                functionReturnValue = NullString;
            }
            return functionReturnValue;
        }

        // sets a field to an application encoded null value ( used in BLL layer )
        public static object SetNull(PropertyInfo objPropertyInfo)
        {
            object functionReturnValue = null;
            switch (objPropertyInfo.PropertyType.ToString())
            {
                case "System.Int16":
                    break;
                case "System.Int32":
                    break;
                case "System.Int64":
                    break;
                case "System.Single":
                    break;
                case "System.Double":
                    break;
                case "System.Decimal":
                    functionReturnValue = (Decimal)NullInteger;
                    break;
                case "System.DateTime":
                    functionReturnValue = NullDate;
                    break;
                case "System.String":
                    break;
                case "System.Char":
                    functionReturnValue = NullString;
                    break;
                case "System.Boolean":
                    functionReturnValue = NullBoolean;
                    break;
                case "System.Guid":
                    functionReturnValue = NullGuid;
                    break;
                case "System.Byte[]":
                   // functionReturnValue = new Byte[];
                      break;
                default:
                    // Enumerations default to the first entry
                    Type pType = objPropertyInfo.PropertyType;
                    if (pType.BaseType.Equals(typeof(System.Enum)))
                    {
                        System.Array objEnumValues = System.Enum.GetValues(pType);
                        Array.Sort(objEnumValues);
                        functionReturnValue = System.Enum.ToObject(pType, objEnumValues.GetValue(0));
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }

                    break;
            }
            return functionReturnValue;
        }

        // convert an application encoded null value to a database null value ( used in DAL )
        public static object GetNull(object objField, object objDBNull)
        {
            object functionReturnValue = null;
            functionReturnValue = objField;
            if (objField == null)
            {
                functionReturnValue = objDBNull;
            }
            else if (objField is int)
            {
                if (Convert.ToInt32(objField) == NullInteger)
                {
                    functionReturnValue = objDBNull;
                }
            }
            else if (objField is float)
            {
                if (Convert.ToSingle(objField) == NullInteger)
                {
                    functionReturnValue = objDBNull;
                }
            }
            else if (objField is double)
            {
                if (Convert.ToDouble(objField) == NullInteger)
                {
                    functionReturnValue = objDBNull;
                }
            }
            else if (objField is decimal)
            {
                if (Convert.ToDecimal(objField) == NullInteger)
                {
                    functionReturnValue = objDBNull;
                }
            }
            else if (objField is System.DateTime)
            {
                if (Convert.ToDateTime(objField) == NullDate)
                {
                    functionReturnValue = objDBNull;
                }
            }
            else if (objField is string)
            {
                if (objField == null)
                {
                    functionReturnValue = objDBNull;
                }
                else
                {
                    if (objField.ToString() == NullString)
                    {
                        functionReturnValue = objDBNull;
                    }
                }
            }
            else if (objField is bool)
            {
                if (Convert.ToBoolean(objField) == NullBoolean)
                {
                    functionReturnValue = objDBNull;
                }
            }
            else if (objField is Guid)
            {
                if (((System.Guid)objField).Equals(NullGuid))
                {
                    functionReturnValue = objDBNull;
                }
            }
            else
            {
                throw new NullReferenceException();
            }
            return functionReturnValue;
        }

        // checks if a field contains an application encoded null value
        public static bool IsNull(object objField)
        {
            bool functionReturnValue = false;
            if (objField.Equals(SetNull(objField)))
            {
                functionReturnValue = true;
            }
            else
            {
                functionReturnValue = false;
            }
            return functionReturnValue;
        }

        public static string GetNullStringValue(Type objType)
        {
            string functionReturnValue = "";
            if ((objType != null))
            {
                if (objType == typeof(int))
                {
                    functionReturnValue = NullInteger.ToString();
                }
                else if (objType == typeof(float))
                {
                    functionReturnValue = NullInteger.ToString();
                }
                else if (objType == typeof(double))
                {
                    functionReturnValue = NullInteger.ToString();
                }
                else if (objType == typeof(decimal))
                {
                    functionReturnValue = NullInteger.ToString();
                }
                else if (objType == typeof(System.DateTime))
                {
                    functionReturnValue = NullDate.ToString();
                }
                else if (objType == typeof(string))
                {
                    functionReturnValue = NullString;
                }
                else if (objType == typeof(bool))
                {
                    functionReturnValue = NullBoolean.ToString();
                }
                else if (objType == typeof(Guid))
                {
                    functionReturnValue = NullGuid.ToString();
                }
                else
                {
                    //throw new NullReferenceException();
                    functionReturnValue = NullString;
                }
            }
            else
            {
                // assume string
                functionReturnValue = NullString;
            }
            return functionReturnValue;
        }

    }
}
