using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XafDTW.Helpers
{
    public class GeneralHelper
    {
        public static void CopyPropertyValue(object obj, object tobj, string columnname, string tcolumnname)
        {
            var property = obj.GetType().GetProperty(columnname);
            var tproperty = tobj.GetType().GetProperty(tcolumnname);
            if (property == null || tproperty == null) return;

            if (property.PropertyType != tproperty.PropertyType) throw new Exception("Source & Target object type does not match.");

            var svalue = property.GetValue(obj);
            if (svalue == null) return;

            if (typeof(DevExpress.Xpo.XPBaseObject).IsAssignableFrom(property.PropertyType))
            {
                string opcriteria = "";
                var tempprop = svalue.GetType().GetProperty("Oid");
                if (tempprop != null)
                {
                    opcriteria = "Oid=?";
                }
                else
                {
                    tempprop = svalue.GetType().GetProperty("BoKey");
                    if (tempprop != null)
                    {
                        opcriteria = "BoKey=?";
                    }
                }
                if (opcriteria == "") throw new Exception("Business Object's Key property is not valid.");

                SetPropertyValueCriteriaOperator(tobj, tcolumnname, DevExpress.Data.Filtering.CriteriaOperator.Parse(opcriteria, tempprop.GetValue(svalue)));
                return;
            }

            SetPropertyValue(tobj, tcolumnname, svalue);
        }
        public static void SetPropertyValueCriteriaOperator(object obj, string propertyname, DevExpress.Data.Filtering.CriteriaOperator op)
        {
            if (!typeof(DevExpress.Xpo.XPBaseObject).IsAssignableFrom(obj.GetType())) return;

            if (obj == null) return;

            var session = (obj as DevExpress.Xpo.XPBaseObject).Session;
            Type iostype = session.GetType();
            var method = iostype.GetMethods().Where(m => {
                if (m.Name != "FindObject") return false;
                var prms = m.GetParameters();
                if (prms.Length != 2) return false;
                if (prms[0].ParameterType != typeof(Type)) return false;
                return prms[1].ParameterType == typeof(DevExpress.Data.Filtering.CriteriaOperator);
            }).FirstOrDefault();

            System.Reflection.PropertyInfo property = obj.GetType().GetProperty(propertyname);
            if (property == null) return;
            if (!typeof(DevExpress.Xpo.XPBaseObject).IsAssignableFrom(property.PropertyType)) return;
            var value = method.Invoke(session, new object[] { property.PropertyType, op });
            if (value == null) throw new MyException($"{propertyname} [{op.LegacyToString()}] not found");
            SetPropertyValue(obj, propertyname, value);
        }

        public static void SetPropertyValue(object obj, string name, object value)
        {
            Type type = obj.GetType();
            type.GetProperty(name).SetValue(obj, value);
        }

        private static MethodInfo baseLoop(Type type, object obj, Func<MethodInfo, bool> condition, object[] prms)
        {
            MethodInfo method = null;

            do
            {
                method = type.GetMethods().Where(f => condition(f)).FirstOrDefault();
                if (method != null) return method;
                var rtn = interfaceLoop(type, obj, condition, prms);
                if (rtn != null) return rtn;
                type = type.GetBaseType();
            } while (type != null);

            return null;
        }
        private static MethodInfo interfaceLoop(Type type, object obj, Func<MethodInfo, bool> condition, object[] prms)
        {
            MethodInfo method = null;
            Type[] inferfaces = type.GetInterfaces();

            foreach (var each in inferfaces)
            {
                method = each.GetMethods().Where(f => condition(f)).FirstOrDefault();
                if (method != null) return method;
                var rtn = interfaceLoop(each, obj, condition, prms);
                if (rtn != null) return rtn;
                rtn = baseLoop(each, obj, condition, prms);
                if (rtn != null) return rtn;
            }

            return null;
        }
        public static MethodInfo GetFunction(object obj, string functionname)
        {
            return GetFunction(obj, functionname, null as object[]);
        }

        public static MethodInfo GetFunction(object obj, string functionname, params object[] prms)
        {
            Func<MethodInfo, bool> condition = (MethodInfo a) =>
            {
                if (a.Name != functionname) return false;

                var innerPrms = a.GetParameters();

                if (prms == null) return innerPrms == null || innerPrms.Length == 0;

                if (innerPrms.Length != prms.Length) return false;

                for (int i = 0; i < prms.Length; ++i)
                {
                    if (prms[i] == null) continue;
                    if (innerPrms[i].ParameterType != typeof(object) && innerPrms[i].ParameterType != prms[i].GetType())
                    {
                        return false;
                    }
                }

                return true;
            };

            Type type = obj.GetType();
            var rtn = baseLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            rtn = interfaceLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            throw new Exception("No valid method found");
        }


        public static MethodInfo GetFunction(object obj, string functionname, params Type[] prms)
        {
            Func<MethodInfo, bool> condition = (MethodInfo a) =>
            {
                if (a.Name != functionname) return false;

                var innerPrms = a.GetParameters();

                if (prms == null) return innerPrms == null || innerPrms.Length == 0;

                if (innerPrms.Length != prms.Length) return false;

                for (int i = 0; i < prms.Length; ++i)
                {
                    if (prms[i] == null) continue;
                    if (innerPrms[i].ParameterType != typeof(object) && innerPrms[i].ParameterType != prms[i])
                    {
                        return false;
                    }
                }

                return true;
            };

            Type type = obj.GetType();
            var rtn = baseLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            rtn = interfaceLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            throw new Exception("No valid method found");
        }
    }

}
