using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SupportSync
{
    public class PropertyUtils
    {
        private PropertyUtils() { }



        /// --------------------------------------------------------------------
        /// <summary>
        /// Determine if a property exists in an object
        /// </summary>
        /// <param name="propertyName">Name of the property </param>
        /// <param name="srcObject">the object to inspect</param>
        /// <returns>true if the property exists, false otherwise</returns>
        /// <exception cref="ArgumentNullException">if srcObject is null</exception>
        /// <exception cref="ArgumentException">if propertName is empty or null </exception>
        /// --------------------------------------------------------------------
        public static bool Exists(string propertyName, object srcObject)
        {
            if (srcObject == null)
                throw new System.ArgumentNullException("srcObject");

            if ((propertyName == null) || (propertyName == String.Empty) || (propertyName.Length == 0))
                throw new System.ArgumentException("Property name cannot be empty or null.");

            PropertyInfo propInfoSrcObj = srcObject.GetType().GetProperty(propertyName);

            return (propInfoSrcObj != null);
        }


        /// --------------------------------------------------------------------
        /// <summary>
        /// Determine if a property exists in an object
        /// </summary>
        /// <param name="propertyName">Name of the property </param>
        /// <param name="srcObject">the object to inspect</param>
        /// <param name="ignoreCase">ignore case sensitivity</param>
        /// <returns>true if the property exists, false otherwise</returns>
        /// <exception cref="ArgumentNullException">if srcObject is null</exception>
        /// <exception cref="ArgumentException">if propertName is empty or null </exception>
        /// --------------------------------------------------------------------
        public static bool Exists(string propertyName, object srcObject, bool ignoreCase)
        {
            if (!ignoreCase)
                return Exists(propertyName, srcObject);

            if (srcObject == null)
                throw new System.ArgumentNullException("srcObject");

            if ((propertyName == null) || (propertyName == String.Empty) || (propertyName.Length == 0))
                throw new System.ArgumentException("Property name cannot be empty or null.");


            PropertyInfo[] propertyInfos = srcObject.GetType().GetProperties();

            propertyName = propertyName.ToLower();
            foreach (PropertyInfo propInfo in propertyInfos)
            {
                if (propInfo.Name.ToLower().Equals(propertyName))
                    return true;
            }
            return false;
        }
    }

}