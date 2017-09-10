﻿//
// BIM IFC library: this library works with Autodesk(R) Revit(R) to export IFC files containing model geometry.
// Copyright (C) 2012  Autodesk, Inc.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;
using Revit.IFC.Export.Utility;
using Revit.IFC.Export.Toolkit;
using Revit.IFC.Common.Utility;

using GeometryGym.Ifc;

namespace Revit.IFC.Export.Exporter.PropertySet
{
    /// <summary>
    /// Provides static methods to create varies IFC properties.  Inherit from PropertyUtil for protected helper functions.
    /// </summary>
    public class FrequencyPropertyUtil : PropertyUtil
    {
        /// <summary>Create a FrequencyMeasure property.</summary>
        /// <param name="file">The IFC file.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="valueType">The value type of the property.</param>
        /// <returns>The created property handle.</returns>
        public static IfcProperty CreateFrequencyProperty(DatabaseIfc db, string propertyName, double value, PropertyValueType valueType)
        {
            return CreateCommonProperty(db, propertyName, new IfcFrequencyMeasure(value), valueType, null);
        }

        /// <summary>
        /// Create a Frequency measure property from the element's parameter.
        /// </summary>
        /// <param name="file">The IFC file.</param>
        /// <param name="exporterIFC">The ExporterIFC.</param>
        /// <param name="elem">The Element.</param>
        /// <param name="revitParameterName">The name of the parameter.</param>
        /// <param name="ifcPropertyName">The name of the property.</param>
        /// <param name="valueType">The value type of the property.</param>
        /// <returns>The created property handle.</returns>
        public static IfcProperty CreateFrequencyPropertyFromElement(DatabaseIfc db, Element elem,
            string revitParameterName, string ifcPropertyName, PropertyValueType valueType)
        {
            double propertyValue;
            if (ParameterUtil.GetDoubleValueFromElement(elem, null, revitParameterName, out propertyValue) != null)
                return CreateFrequencyProperty(db, ifcPropertyName, propertyValue, valueType);
            return null;
        }

        /// <summary>
        /// Create a Frequency measure property from the element's or type's parameter.
        /// </summary>
        /// <param name="file">The IFC file.</param>
        /// <param name="exporterIFC">The ExporterIFC.</param>
        /// <param name="elem">The Element.</param>
        /// <param name="revitParameterName">The name of the parameter.</param>
        /// <param name="revitBuiltInParam">The built in parameter to use, if revitParameterName isn't found.</param>
        /// <param name="ifcPropertyName">The name of the property.</param>
        /// <param name="valueType">The value type of the property.</param>
        /// <returns>The created property handle.</returns>
        public static IfcProperty CreateFrequencyPropertyFromElementOrSymbol(DatabaseIfc db, Element elem,
            string revitParameterName, BuiltInParameter revitBuiltInParam, string ifcPropertyName, PropertyValueType valueType)
        {
            IfcProperty propHnd = CreateFrequencyPropertyFromElement(db, elem, revitParameterName, ifcPropertyName, valueType);
            if(propHnd != null)
                return propHnd;

            if (revitBuiltInParam != BuiltInParameter.INVALID)
            {
                string builtInParamName = LabelUtils.GetLabelFor(revitBuiltInParam);
                propHnd = CreateFrequencyPropertyFromElement(db, elem, builtInParamName, ifcPropertyName, valueType);
                if(propHnd != null)
                    return propHnd;
            }

            // For Symbol
            Document document = elem.Document;
            ElementId typeId = elem.GetTypeId();
            Element elemType = document.GetElement(typeId);
            if (elemType != null)
                return CreateFrequencyPropertyFromElementOrSymbol(db, elemType, revitParameterName, revitBuiltInParam, ifcPropertyName, valueType);
            else
                return null;
        }
    }
}
