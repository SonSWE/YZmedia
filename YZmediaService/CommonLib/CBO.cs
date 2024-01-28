using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;


namespace CommonLib
{
    internal class CBO
    {
        private static ArrayList GetPropertyInfo(Type objType)
        {

            ArrayList objProperties = new ArrayList();
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                objProperties.Add(objProperty);
            }

            return objProperties;
        }

        private static int[] GetOrdinals(ArrayList objProperties, IDataReader dr)
        {

            int[] arrOrdinals = new int[objProperties.Count + 1];

            if ((dr != null))
            {
                for (int intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++)
                {
                    arrOrdinals[intProperty] = -1;
                    try
                    {
                        arrOrdinals[intProperty] = dr.GetOrdinal(((PropertyInfo)objProperties[intProperty]).Name);
                    }
                    catch(Exception ex)
                    {
                        Logger.log.Error(ex.ToString());
                    }
                    // property does not exist in datareader
                }
            }


            return arrOrdinals;
        }

        private static object CreateObject(Type objType, IDataReader dr, ArrayList objProperties, int[] arrOrdinals)
        {

            object objObject = Activator.CreateInstance(objType);
            int intProperty = 0;

            // fill object with values from datareader
            for (intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++)
            {
                if (((PropertyInfo)objProperties[intProperty]).CanWrite)
                {
                    if (arrOrdinals[intProperty] != -1)
                    {
                        if (dr.GetValue(arrOrdinals[intProperty]) == System.DBNull.Value)
                        {
                            // translate Null value
                            ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, Null.SetNull((PropertyInfo)objProperties[intProperty]), null);
                        }
                        else
                        {
                            try
                            {
                                Type pType = ((PropertyInfo)objProperties[intProperty]).PropertyType;

                                switch (pType.FullName)
                                {
                                    case "System.Enum":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, System.Enum.ToObject(pType, dr.GetValue(arrOrdinals[intProperty])), null);
                                        break;
                                    case "System.String":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetString(arrOrdinals[intProperty]), null);
                                        break;
                                    case "System.Boolean":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetBoolean(arrOrdinals[intProperty]), null);
                                        break;
                                    case "System.Decimal":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetDecimal(arrOrdinals[intProperty]), null);
                                        break;
                                    case "System.Int16":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetInt16(arrOrdinals[intProperty]), null);
                                        break;
                                    case "System.Int32":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetInt32(arrOrdinals[intProperty]), null);
                                        break;
                                    case "System.Int64":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetInt64(arrOrdinals[intProperty]), null);
                                        break;
                                    case "System.DateTime":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetDateTime(arrOrdinals[intProperty]), null);
                                        break;
                                    case "System.Double":
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr.GetDouble(arrOrdinals[intProperty]), null);
                                        break;
                                    default:
                                        // try explicit conversion
                                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, Convert.ChangeType(dr.GetValue(arrOrdinals[intProperty]), pType), null);
                                        break;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        // property does not exist in datareader
                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, Null.SetNull((PropertyInfo)objProperties[intProperty]), null);
                    }
                }
            }


            return objObject;
        }

        public static object FillObject(IDataReader dr, Type objType)
        {

            object objFillObject = null;
            //Dim intProperty As Integer

            // get properties for type
            ArrayList objProperties = GetPropertyInfo(objType);

            // get ordinal positions in datareader
            int[] arrOrdinals = GetOrdinals(objProperties, dr);

            // read datareader
            if (dr.Read())
            {
                // fill business object
                objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals);
            }
            else
            {
                objFillObject = null;
            }

            // close datareader
            if ((dr != null))
            {
                dr.Close();
            }


            return objFillObject;
        }

        public static ArrayList FillCollection(IDataReader dr, Type objType)
        {

            ArrayList objFillCollection = new ArrayList();
            object objFillObject = null;
            //Dim intProperty As Integer

            // get properties for type
            ArrayList objProperties = GetPropertyInfo(objType);

            // get ordinal positions in datareader
            int[] arrOrdinals = GetOrdinals(objProperties, dr);

            // iterate datareader
            while (dr.Read())
            {
                // fill business object
                objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals);
                // add to collection
                objFillCollection.Add(objFillObject);
            }

            // close datareader
            if ((dr != null))
            {
                dr.Close();
            }


            return objFillCollection;
        }

        public static object InitializeObject(object objObject, Type objType)
        {

            int intProperty = 0;

            // get properties for type
            ArrayList objProperties = GetPropertyInfo(objType);

            // initialize properties
            for (intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++)
            {
                if (((PropertyInfo)objProperties[intProperty]).CanWrite)
                {
                    ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, Null.SetNull((PropertyInfo)objProperties[intProperty]), null);
                }
            }
            return objObject;
        }


        // dataset

        private static Hashtable GetOrdinalsFromDataSet(Hashtable hashProperties, DataSet dt)
        {

            Hashtable arrOrdinals = new Hashtable();

            if ((dt.Tables.Count > 0))
            {

                for (int i = 0; i < dt.Tables[0].Columns.Count; i++)
                {
                    if (hashProperties.ContainsKey(dt.Tables[0].Columns[i].ColumnName.ToUpper()))
                        arrOrdinals[dt.Tables[0].Columns[i].ColumnName.ToUpper()] = i;
                }
            }
            return arrOrdinals;
        }

        private static object CreateObjectFromDataSet(Type objType, DataRow dr, Hashtable objProperties, Hashtable arrOrdinals)
        {

            object objObject = Activator.CreateInstance(objType);


            // fill object with values from datareader
            string _fieldname = "";
            int _possition = -1;
            foreach (DictionaryEntry de in arrOrdinals)
            {
                _fieldname = de.Key.ToString();
                _possition = (int)de.Value;

                if (((PropertyInfo)objProperties[_fieldname]).CanWrite)
                {
                    if (dr[_fieldname] == System.DBNull.Value)
                    {
                        // translate Null value
                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Null.SetNull((PropertyInfo)objProperties[_fieldname]), null);
                    }
                    else
                    {
                        try
                        {
                            Type pType = ((PropertyInfo)objProperties[_fieldname]).PropertyType;

                            #region
                            switch (pType.FullName)
                            {
                                case "System.Enum":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, System.Enum.ToObject(pType, dr[_possition]), null);
                                    break;
                                case "System.String":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (string)dr[_possition], null);
                                    break;
                                case "System.Boolean":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (Boolean)dr[_possition], null);
                                    break;
                                case "System.Decimal":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToDecimal(dr[_possition].ToString()), null);
                                    break;
                                case "System.Int16":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToInt16(dr[_possition].ToString()), null);
                                    break;
                                case "System.Int32":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToInt32(dr[_possition].ToString()), null);
                                    break;
                                case "System.Int64":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToInt64(dr[_possition].ToString()), null);
                                    break;
                                case "System.DateTime":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (DateTime)dr[_possition], null);
                                    break;
                                case "System.Double":
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (Double)dr[_possition], null);
                                    break;
                                default:
                                    // try explicit conversion
                                    ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ChangeType(dr[_possition], pType), null);
                                    break;
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }

            return objObject;
        }

        public static ArrayList FillCollectionFromDataSet(DataSet ds, Type objType)
        {
            try
            {
                ArrayList objFillCollection = new ArrayList();
                object objFillObject = null;

                // get properties for type
                Hashtable objProperties = GetPropertyInfo_DataSet(objType);

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

                // iterate datareader

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // fill business object
                    objFillObject = CreateObjectFromDataSet(objType, dr, objProperties, arrOrdinals);
                    // add to collection
                    objFillCollection.Add(objFillObject);
                }

                return objFillCollection;
            }
            catch
            {
                return new ArrayList();
            }



        }

        private static Hashtable GetPropertyInfo_DataSet(Type objType)
        {
            Hashtable hashProperties = new Hashtable();
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                hashProperties[objProperty.Name.ToUpper()] = objProperty;
            }
            return hashProperties;
        }

        public static object FillObjectFromDataSet(DataSet ds, Type objType)
        {

            try
            {
                object objFillObject = null;
                //Dim intProperty As Integer

                // get properties for type
                Hashtable objProperties = GetPropertyInfo_DataSet(objType);

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

                // read datareader
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // fill business object
                    objFillObject = CreateObjectFromDataSet(objType, ds.Tables[0].Rows[0], objProperties, arrOrdinals);
                }
                else
                {
                    objFillObject = null;
                }


                return objFillObject;
            }
            catch
            {
                return null;
            }
        }

        //
        #region LinhNN them phan cho phep doc tu DataTable
        public static ArrayList FillCollection(DataTable dt, Type objType)
        {

            ArrayList objFillCollection = new ArrayList();
            object objFillObject = null;
            //Dim intProperty As Integer

            // get properties for type
            ArrayList objProperties = GetPropertyInfo(objType);

            foreach (DataRow dr in dt.Rows)
            {
                // fill business object
                objFillObject = CreateObject(objType, dr, objProperties);
                // add to collection
                objFillCollection.Add(objFillObject);
            }

            return objFillCollection;
        }

        public static object FillObject(DataTable dt, Type objType)
        {

            object objFillObject = null;
            //Dim intProperty As Integer

            // get properties for type
            ArrayList objProperties = GetPropertyInfo(objType);


            // read datareader
            if (dt.Rows.Count > 0)
            {
                // fill business object
                DataRow dr = dt.Rows[0];
                objFillObject = CreateObject(objType, dr, objProperties);
            }
            else
            {
                objFillObject = null;
            }

            return objFillObject;
        }

        private static object CreateObject(Type objType, DataRow dr, ArrayList objProperties)
        {

            object objObject = Activator.CreateInstance(objType);
            int intProperty = 0;
            bool hasDatarowColum = false;

            // fill object with values from datareader
            for (intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++)
            {
                if (((PropertyInfo)objProperties[intProperty]).CanWrite)
                {
                    //kiem tra xem trong datarow co thuoc tinh nay khong
                    hasDatarowColum = false;
                    try
                    {
                        object obj = dr[((PropertyInfo)objProperties[intProperty]).Name];
                        hasDatarowColum = true;
                    }
                    catch
                    {
                        hasDatarowColum = false;
                    }
                    //ket thuc: kiem tra xem trong datarow co thuoc tinh nay khong

                    if (hasDatarowColum == false)
                    {
                        // translate Null value
                        ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, Null.SetNull((PropertyInfo)objProperties[intProperty]), null);
                    }
                    else
                    {
                        try
                        {
                            // try implicit conversion first
                            ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, dr[((PropertyInfo)objProperties[intProperty]).Name], null);
                        }
                        catch
                        {
                            // data types do not match
                            try
                            {
                                Type pType = ((PropertyInfo)objProperties[intProperty]).PropertyType;
                                //need to handle enumeration conversions differently than other base types
                                if (pType.BaseType.Equals(typeof(System.Enum)))
                                {
                                    ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, System.Enum.ToObject(pType, dr[((PropertyInfo)objProperties[intProperty]).Name]), null);
                                }
                                else
                                {
                                    // try explicit conversion
                                    ((PropertyInfo)objProperties[intProperty]).SetValue(objObject, Convert.ChangeType(dr[((PropertyInfo)objProperties[intProperty]).Name], pType), null);
                                }
                            }
                            catch
                            {
                            }
                            // error assigning a datareader value to a property
                        }
                    }

                }
            }


            return objObject;
        }
        #endregion
        //
    }

    public class CBO<T>
    {
        public static ObservableCollection<T> FillCollectionFromDataSet_ob(DataSet ds)
        {
            ObservableCollection<T> _ob_T = new ObservableCollection<T>();

            // get properties for type
            Hashtable objProperties = GetPropertyInfo(typeof(T));

            // get ordinal positions in datareader
            Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

            // iterate datareader
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // fill business object
                    T objFillObject = (T)CreateObjectFromDataSet(typeof(T), dr, objProperties, arrOrdinals);

                    // add to collection
                    _ob_T.Add(objFillObject);
                }
            }
            return _ob_T;
        }

        public static List<T> FillCollection_FromDataSet(DataSet ds)
        {
            List<T> _list_T = new List<T>();
            try
            {
                // get properties for type
                Hashtable objProperties = GetPropertyInfo(typeof(T));

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

                // iterate datareader
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        // fill business object
                        T objFillObject = (T)CreateObjectFromDataSet(typeof(T), dr, objProperties, arrOrdinals);

                        // add to collection
                        _list_T.Add(objFillObject);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
            return _list_T;
        }

        public static List<T> FillCollection_FromDataTable(DataTable dt)
        {
            List<T> _list_T = new List<T>();
            if (dt == null)
            {
                return _list_T;
            }

            // get properties for type
            Hashtable objProperties = GetPropertyInfo(typeof(T));

            // get ordinal positions in datareader
            Hashtable arrOrdinals = GetOrdinalsFromDataTable(objProperties, dt);

            // iterate datareader
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    // fill business object
                    T objFillObject = (T)CreateObjectFromDataSet(typeof(T), dr, objProperties, arrOrdinals);

                    // add to collection
                    _list_T.Add(objFillObject);
                }
            }
            return _list_T;

        }

        public static ObservableCollection<T> FillCollectionFromDataTable_ob(DataTable dt)
        {
            ObservableCollection<T> _list_T = new ObservableCollection<T>();

            // get properties for type
            Hashtable objProperties = GetPropertyInfo(typeof(T));

            // get ordinal positions in datareader
            Hashtable arrOrdinals = GetOrdinalsFromDataTable(objProperties, dt);

            // iterate datareader
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    // fill business object
                    T objFillObject = (T)CreateObjectFromDataSet(typeof(T), dr, objProperties, arrOrdinals);

                    // add to collection
                    _list_T.Add(objFillObject);
                }
            }
            return _list_T;

        }

        public static T FillObjectFromDataSet(DataSet ds)
        {

            try
            {
                // get properties for type
                Hashtable objProperties = GetPropertyInfo(typeof(T));

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    // read datareader
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // fill business object
                        T _objResult = (T)CreateObjectFromDataSet(typeof(T), ds.Tables[0].Rows[0], objProperties, arrOrdinals);
                        return _objResult;
                    }
                }

                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public static T FillObject_FromDataTable(DataTable dt)
        {

            try
            {
                // get properties for type
                Hashtable objProperties = GetPropertyInfo(typeof(T));

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataTable(objProperties, dt);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        // fill business object
                        T _objResult = (T)CreateObjectFromDataSet(typeof(T), dt.Rows[0], objProperties, arrOrdinals);
                        return _objResult;
                    }
                }

                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        #region private

        /// <summary>
        /// Tạo một đối tượng từ datarow
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="dr"></param>
        /// <param name="objProperties"></param>
        /// <param name="arrOrdinals"></param>
        /// <returns></returns>
        private static object CreateObjectFromDataSet(Type objType, DataRow dr, Hashtable objProperties, Hashtable arrOrdinals)
        {
            string _fieldname = "";
            int _possition = -1;
            string _PropertyType_FullName = "";
            try
            {
                object objObject = Activator.CreateInstance(objType);
                foreach (DictionaryEntry de in arrOrdinals)
                {
                    _fieldname = de.Key.ToString();
                    _possition = (int)arrOrdinals[_fieldname];
                    PropertyInfo _PropertyInfo = (PropertyInfo)objProperties[_fieldname];

                    if (_PropertyInfo.CanWrite)
                    {
                        if (_possition == -1 || dr[_possition] == System.DBNull.Value)
                        {
                            //LinhNN sua 16/12/2013: không cần vì mặc định Activator.CreateInstance khi tạo đã set các thuộc tính về null
                            //   _PropertyInfo.SetValue(objObject, Null.SetNull(_PropertyInfo), null);
                        }
                        else
                        {
                            #region
                            _PropertyType_FullName = _PropertyInfo.PropertyType.FullName;
                            switch (_PropertyInfo.PropertyType.FullName)
                            {
                                case "System.Enum":
                                    _PropertyInfo.SetValue(objObject, System.Enum.ToObject(_PropertyInfo.PropertyType, dr[_possition]), null);
                                    break;
                                case "System.String":
                                    _PropertyInfo.SetValue(objObject, (string)dr[_possition], null);
                                    break;
                                case "System.Boolean":
                                    _PropertyInfo.SetValue(objObject, (Boolean)dr[_possition], null);
                                    break;
                                case "System.Decimal":
                                    _PropertyInfo.SetValue(objObject, Convert.ToDecimal(dr[_possition]), null);
                                    break;
                                case "System.Int16":
                                    _PropertyInfo.SetValue(objObject, Convert.ToInt16(dr[_possition]), null);
                                    break;
                                case "System.Int32":
                                    _PropertyInfo.SetValue(objObject, Convert.ToInt32(dr[_possition]), null);
                                    break;
                                case "System.Int64":
                                    _PropertyInfo.SetValue(objObject, Convert.ToInt64(dr[_possition]), null);
                                    break;
                                case "System.DateTime":
                                    _PropertyInfo.SetValue(objObject, Convert.ToDateTime(dr[_possition]), null);
                                    break;
                                case "System.Double":
                                    _PropertyInfo.SetValue(objObject, Convert.ToDouble(dr[_possition]), null);
                                    break;
                                default:
                                    // try explicit conversion
                                    _PropertyInfo.SetValue(objObject, Convert.ChangeType(dr[_possition], _PropertyInfo.PropertyType), null);
                                    break;
                            }
                            #endregion
                        }
                    }
                }

                return objObject;
            }
            catch
            {
                Logger.log.Error("_fieldname " + _fieldname + " _PropertyType_FullName " + _PropertyType_FullName);

                return Activator.CreateInstance(objType);
            }
        }

        /// <summary>
        /// Lấy tên thuộc tính của 1 kiểu dữ liệu
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static Hashtable GetPropertyInfo(Type objType)
        {
            Hashtable hashProperties = new Hashtable();
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                hashProperties[objProperty.Name.ToUpper()] = objProperty;
            }
            return hashProperties;
        }

        /// <summary>
        /// Gán thứ tự cột trong dataset theo tên thuộc tính
        /// </summary>
        /// <param name="hashProperties"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Hashtable GetOrdinalsFromDataSet(Hashtable hashProperties, DataSet dt)
        {

            Hashtable arrOrdinals = new Hashtable();

            if ((dt.Tables.Count > 0))
            {

                for (int i = 0; i < dt.Tables[0].Columns.Count; i++)
                {
                    if (hashProperties.ContainsKey(dt.Tables[0].Columns[i].ColumnName.ToUpper()))
                        arrOrdinals[dt.Tables[0].Columns[i].ColumnName.ToUpper()] = i;
                }
            }
            return arrOrdinals;
        }

        private static Hashtable GetOrdinalsFromDataTable(Hashtable hashProperties, DataTable dt)
        {

            Hashtable arrOrdinals = new Hashtable();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (hashProperties.ContainsKey(dt.Columns[i].ColumnName.ToUpper()))
                    arrOrdinals[dt.Columns[i].ColumnName.ToUpper()] = i;
            }

            return arrOrdinals;
        }

        //Hàm convert string to datetime dùng cho nhiều loại format
        //Vì hàm Convert.ToDateTime trong một số trường hợp không format được DateTime từ (string)datarow
        private static DateTime ConvertDateTime(string value)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                //Ưu tiên dd/MM/yyyy trước vì check từ đầu array -> cuối
                string[] formats = {
                            "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss tt",
                            "d/M/yyyy hh:mm:ss tt", "dd/MM/yyyy h:mm:ss tt",
                            "MM/dd/yyyy HH:mm:ss", "MM/d/yyyy HH:mm:ss.ffffff",
                            "M/d/yyyy h:mm: ss tt", "M / d / yyyy h: mm tt",
                            "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                            "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                            "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                            "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
                return DateTime.ParseExact(value, formats, provider);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
                return DateTime.MinValue;
            }
        }

        #endregion
    }
}
